using UnityEngine;
using System.Collections;
using UnityEditor.Build.Content;
using JetBrains.Annotations;

public class CameraShake : Singleton<CameraShake>
{

    public IEnumerator Shake(float duration, float frequency, float magnitude)
    {
        Camera camera;
        float camSize = 2.5f;
        float zoomTime;
        float originSize;
        Vector2 playerPos;
        Vector2 cameraPos;
        Vector2 originPos;
        camera = GetComponent<Camera>();
        originSize = camera.orthographicSize;
        cameraPos = camera.transform.localPosition;
        originPos = cameraPos;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        Vector3 targetPos = new Vector3(playerPos.x + 2f, playerPos.y + 0.35f, -20f);
        this.transform.position = targetPos;
        camera.orthographicSize = camSize;
        //
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
                transform.localPosition = new Vector3(targetPos.x + x, targetPos.y + y, targetPos.z);
            }

            elapsed += Time.deltaTime;
            elapsed_in_cycle += Time.deltaTime;
            
            

            yield return null;
        }
        camera.orthographicSize = originSize;
        transform.localPosition = originalPosition;
        

    }
}