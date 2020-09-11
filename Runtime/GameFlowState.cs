using UnityEngine;
using UnityEngine.SceneManagement;

namespace TinyBitTurtleGems
{
    public partial class GameFlowCtrl
    {
        [System.Serializable]
        public class GameFlowState : FSM.FSMState
        {
            public UIPanel panel;
            public Object scene;

            private GameObject panelInstance;

            /// <summary>
            /// reset all panels
            /// </summary>
            private void Awake()
            {
                panelInstance = GameObject.Find(panel.name);

                // position at 0,0,0 and make panel invisible
                if (panelInstance)
                {
                    panelInstance.transform.position = Vector3.zero;
                    panelInstance.SetActive(false);
                }
            }

            override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                if (panelInstance != null)
                    panelInstance.SetActive(true);

                if (scene != null)
                    SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
            }

            override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                if (panelInstance != null)
                    panelInstance.SetActive(false);

                if (scene != null)
                    SceneManager.UnloadSceneAsync(scene.name);
            }
        }
    }
}