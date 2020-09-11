using UnityEngine;

namespace TinyBitTurtle.Gems
{
    public class InputZone : MonoBehaviour
    {
        private Rect deadZone;

        // Start is called before the first frame update
        void Start()
        {
            deadZone = new Rect(0, 0, Screen.width / 4, Screen.height / 4);
        }

        private void OnDrawGizmos()
        {
            GUI.Box(new Rect(10, 10, 1000, 90), "dead zone");
        }

        // Update is called once per frame
        bool isValid()
        {
            return deadZone.Contains(Input.mousePosition);
        }
    }
}