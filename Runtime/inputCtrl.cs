using UnityEngine;
using TinyBitTurtle.Core;

namespace TinyBitTurtle.Gems
{
    enum ButtonState
    {
        none = 0,
        down,
        released,
    }

    public partial class InputCtrl : SingletonMonoBehaviour<InputCtrl>
    {
        // vars
        public float swipeSpeed = 2.0f;
        public float tapDistance = 10.0f;

        // internal bookkeeping
        private Vector2 prevDist = new Vector2(0, 0);
        private Vector2 curDist = new Vector2(0, 0);
        private Vector2 touchOutofBound = new Vector2(-1, -1);
        private Vector2 lastTouchDown = new Vector2(-1, -1);
        private Vector3 lastSwipe = new Vector3(0, 0, 0);
        private Vector2 lastTouchUp = new Vector2(-1, -1);

        private Vector3 lastTouchPos = new Vector3();
        private eAction lastAction = eAction.NONE;
        private int fingerCount;
        private int pressCount;
        private int releaseCount;
        private Touch currentTouch;
        private Touch lastTap;
        private Vector2 cameraSwipe;
        private GameObject camera = null;

        struct Button
        {
            public ButtonState m_ButtonState;
            public Vector3 m_ScreenPos;
        }

        enum eAction
        {
            NONE = 0,
            PRESS,
            RELEASE,
            SWIPE
        }

        private void Start()
        {
            camera = GameObject.FindGameObjectWithTag("UI Camera");
        }

        public void SetEnabled(bool enabled)
        {
            InputCtrl.Instance.enabled = enabled;
        }

        /// <summary>
        /// Buffers the windows input.
        /// </summary>
        private void CaptureWindowInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("input: GetMouseButtonDown");
                lastAction = eAction.PRESS;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("input: GetMouseButtonUp");
                // release position doesn't match the initial touch position
                // need to use some slack
                releaseCount++;
                lastTouchUp = Input.mousePosition;
                Vector3 vDistance = lastTouchUp - lastTouchDown;
                float fTouchDistance = Mathf.Abs(vDistance.magnitude);
                if (fTouchDistance > tapDistance)
                {
                    // pack swipe data
                    // position x,y; normalized distance z
                    lastSwipe.x = vDistance.x;
                    lastSwipe.y = vDistance.y;
                    lastSwipe.z = fTouchDistance / 0.1f;
                    if (lastSwipe.z > 1.0f)
                        lastSwipe.z = 1.0f;

                    lastAction = eAction.SWIPE;
                }
                else
                {
                    lastTouchPos = camera.GetComponent<Camera>().ScreenToViewportPoint(lastTouchUp);

                    lastAction = eAction.RELEASE;
                }

                // reset touch down
                lastTouchDown = touchOutofBound;
            }

            // left button + directional keys
            if (Input.GetMouseButton(0))
            {
                Debug.Log("input: GetMouseButton");
                if (lastTouchDown == touchOutofBound)
                    lastTouchDown = Input.mousePosition;

                float inputX = Input.GetAxis("Mouse X");
                float inputY = Input.GetAxis("Mouse Y");

                Vector2 CameraSwipe = new Vector2(inputX, inputY);
                CameraSwipe *= Time.deltaTime * -50;
            }

            // scroll wheel
            //float fZoom = Input.GetAxis("Mouse ScrollWheel");
        }

        private void CaptureMobileInputs()
        {
            //  list of unsync screen touches
            foreach (Touch touch in Input.touches)
            {
                // put finger down
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    fingerCount++;
                    currentTouch = touch;
                }

                // release static finger
                if (touch.phase == TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    releaseCount++;
                    lastTap = touch;
                }

                // swipe
                if (touch.phase == TouchPhase.Moved)
                {
                    cameraSwipe = touch.deltaPosition * touch.deltaTime * -swipeSpeed;
                }
            }


        }

        private void ConsummeInputs()
        {
            // we have some click/tap
            if (releaseCount == 1)
            {
                // release position doesn't match exactly the initial finger down position
                // need to use some slack
                Vector3 vDistance = lastTouchDown - lastTap.position;
                float fTouchDistance = Mathf.Abs(vDistance.magnitude);
                if (fTouchDistance < tapDistance)
                {
                    lastAction = eAction.RELEASE;

                    lastTouchPos = camera.GetComponent<Camera>().ScreenToViewportPoint(lastTap.position);

                    ActionCtrl.Instance.OnClicked?.Invoke(Vector2.zero);
                }
                else
                {
                    lastAction = eAction.SWIPE;
                }

                lastTouchDown = touchOutofBound;
            }
            // mobile swipe
            else if (fingerCount == 1)
            {
                //print("swipe "+CurrentTouch.position);
                if (lastTouchDown == touchOutofBound)
                    lastTouchDown = currentTouch.position;

                //if (OnSwiped != null)
                //    OnSwiped(Vector2.zero, Vector2.zero);

            }
            // mobile 2 finger tap
            else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {

                curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
                prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions

                //if (OnPinched != null)
                //    OnPinched();
            }
        }

        /// <summary>
        /// Resets the input.
        /// </summary>
        private void ResetInput()
        {
            // reset buffered action
            lastAction = eAction.NONE;

            fingerCount = 0;
            pressCount = 0;
            releaseCount = 0;
            currentTouch = new Touch();
            lastTap = new Touch();
            cameraSwipe = Vector2.zero;
        }

        // Update is called once per frame
        private void Update()
        {
            ResetInput();

#if (UNITY_IOS || UNITY_ANDROID)
        CaptureMobileInputs();
#else
            CaptureWindowInputs();
#endif
            ConsummeInputs();
        }
    }
}