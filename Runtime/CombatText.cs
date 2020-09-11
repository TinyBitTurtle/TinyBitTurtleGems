using EZObjectPools;
using UnityEngine;

namespace TinyBitTurtle.Gems
{
    public class CombatText : MonoBehaviour
    {
        public CombatTextSettings settings;

        private float parametric;
        private float lifeSpan;
        private float age;
        private bool alive;
        private float alpha;
        private PooledObject pooledObject;

        public void Init(string message, float duration, float speed, float angle)
        {
            pooledObject = GetComponent<PooledObject>();

            GetComponent<UILabel>().text = message;
            parametric = 1;

            age = lifeSpan = duration;

            Vector3 speedVector = Vector3.up * speed;
            Vector3 ejectionVector = Quaternion.AngleAxis(angle, Vector3.forward) * speedVector;

            Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = settings.gravity;
            rigidbody2D.AddForce(ejectionVector, ForceMode2D.Impulse);

            alpha = GetComponent<UILabel>().alpha;

            alive = true;
        }

        private void Update()
        {
            if (alive == false)
            {
                gameObject.SetActive(false);
                return;
            }

            age -= Time.deltaTime;
            parametric = age / lifeSpan;
            // alpha
            alpha = settings.fadeCurve.Evaluate(parametric);
            // size
            float newSize = settings.sizeCurve.Evaluate(parametric) * settings.size;
            transform.localScale = new Vector3(newSize, newSize, newSize);
            // life span
            if (age < 0)
            {
                pooledObject.Disable();
                alive = false;
            }
        }
    }
}