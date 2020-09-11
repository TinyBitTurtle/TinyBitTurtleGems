namespace TinyBitTurtle.Gems
{
    public partial class GameFlowCtrl : FSM
    {
        // register the transition delegate
        void OnEnable()
        {
            TransitionCtrl.OnMidFade += UISwitchPanel;
        }

        void OnDisable()
        {
            TransitionCtrl.OnMidFade -= UISwitchPanel;
        }

        // switch state
        public void UISwitchPanel(UnityEngine.Object triggerName)
        {
            animator.ResetTrigger(triggerName.name);
            animator.SetTrigger(triggerName.name);
        }
    }
}