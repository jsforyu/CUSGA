using UnityEngine;
using System.Collections;

public class CameraShake : Singleton<CameraShake>
{
    public IEnumerator Shake(float duration, float frequency, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;
        float cycle = 1f / frequency;
        float elapsed_in_cycle = cycle;

        while (elapsed < duration)
        {
            if (elapsed_in_cycle >= cycle)
            {
                elapsed_in_cycle = 0.0f;
                // Υπ¶―
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            }

            elapsed += Time.deltaTime;
            elapsed_in_cycle += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}