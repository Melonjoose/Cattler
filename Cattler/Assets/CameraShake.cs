using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private void Awake()
    {
        instance = this;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeSeq(duration, magnitude));
    }

    public IEnumerator ShakeSeq(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Progress from 0 > 1
            float progress = elapsed / duration;

            // Invert progress so it starts at 1 and goes to 0
            float intensity = (1f - progress) * magnitude;

            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}