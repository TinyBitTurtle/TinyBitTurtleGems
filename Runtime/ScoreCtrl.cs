using TinyBitTurtle.Core;

namespace TinyBitTurtle.Gems
{
    public partial class ScoreCtrl : SingletonMonoBehaviour<ScoreCtrl>
    {
        private int Score;
        private int HiScore;


        void OnEnable()
        {
            // hook up the action to the callback func
            ActionCtrl.Instance.actionScoreChange += UpdateScore;
        }

        void OnDisable()
        {
            // de-register all events
            ActionCtrl.Instance.actionScoreChange -= UpdateScore;
        }

        public void UpdateScore(int _Score)
        {
            Score += _Score;

            HiScore = Score > HiScore ? Score : HiScore;
        }

        public void Reset()
        {
            Score = 0;
        }
    }
}
