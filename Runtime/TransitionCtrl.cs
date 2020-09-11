using System;
using System.Collections;
using UnityEngine;
using TinyBitTurtle.Core;

namespace TinyBitTurtle.Gems
{
    public class TransitionCtrl : SingletonMonoBehaviour<TransitionCtrl>
    {
        public Material TransitionMaterial;
        public float duration = 1;

        public static event Action<UnityEngine.Object> OnMidFade;
        public static event Action<UnityEngine.Object> OnFadeFinish;

        public void StartTransition(UnityEngine.Object trigger)
        {
            StartCoroutine("Transition", trigger);
        }

        IEnumerator Transition(UnityEngine.Object trigger)
        {
            float step = 1.0f / duration;

            // transition in
            float i = 0;
            while (i < duration)
            {
                i += step * Time.deltaTime;
                TransitionMaterial.SetFloat("_Cutoff", i);
                yield return null;
            }

            if (OnMidFade != null)
                OnMidFade(trigger);

            // transition out
            i = 1.0f;
            while (i >= 0.0f)
            {
                i -= step * Time.deltaTime;
                TransitionMaterial.SetFloat("_Cutoff", i);
                yield return null;
            }

            if (OnFadeFinish != null)
                OnFadeFinish(trigger);

            // make sure the value is zero out
            TransitionMaterial.SetFloat("_Cutoff", 0.0f);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (TransitionMaterial != null)
                Graphics.Blit(src, dst, TransitionMaterial);
        }
    }
}