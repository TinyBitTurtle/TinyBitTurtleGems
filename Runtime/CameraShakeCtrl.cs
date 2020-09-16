using System.Collections;
using UnityEngine;

public class CameraShakeCtrl : SingletonMonoBehaviour<CameraShakeCtrl>
{
    public float duration;
    public float amplitude;

    private float timer = 0;
    private Vector3 shake = new Vector3();

    public void StartShake()
    {
        timer = duration;

        // add sound
    }
    // Use this for initialization
    private void Start()
    {
        shake.z = gameObject.transform.position.z;
        StartCoroutine("Shake");
    }

    // Update is called once per frame
    IEnumerator Shake()
    {
        while (true)
        {
            if (timer > 0)
            {
                float normalizedAmplitude = amplitude * (timer / duration);
                shake.x = Random.Range(-normalizedAmplitude, normalizedAmplitude);
                shake.y = Random.Range(-normalizedAmplitude, normalizedAmplitude);

                gameObject.transform.position = shake;

                timer -= Time.deltaTime;
                if (timer < 0.0f)
                    timer = 0.0f;
            }

            yield return null;
        }
    }
}