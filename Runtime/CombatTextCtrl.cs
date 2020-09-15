using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TinyBitTurtle.Core;

namespace TinyBitTurtle.Gems
{
    public class CombatTextCtrl : SingletonMonoBehaviour<CombatTextCtrl>
    {
        public enum eLevel
        {
            fail = -1,
            normal,
            elevated,
            critical
        }

        [Serializable]
        public struct Level
        {
            public Color color;
            public string name;
            public float level;
        }

        public CombatText combatText;
        public float duration = 1;
        public float speed = 0.5f;
        [Range(0, 359)]
        public float ejectionAngle = 0.0f;
        [Range(-180, 180)]
        public float spreadAngle = 0.0f;
        public Level[] levels;

        private Stack<CombatText> scores = new Stack<CombatText>();

        public void add(string message, Vector3 pos, eLevel level)
        {
            GameObject newCombatText;

            combatText.NewText(pos, out newCombatText);

            float angle = Mathf.Clamp(ejectionAngle + UnityEngine.Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f), -180, 180);

            CombatText pooledCombatText = newCombatText.GetComponent<CombatText>();
            pooledCombatText.Init(message, duration, speed, angle);
        }

        // Update is called once per frame
        void Start()
        {
            StartCoroutine("ShowNewCombatTexts");
        }

        // continously show numbers
        IEnumerator ShowNewCombatTexts()
        {
            while (true)
            {
                if (scores.Count != 0)
                {
                    CombatText score = scores.Pop();
                }

                yield return null;
            }
        }
    }
}