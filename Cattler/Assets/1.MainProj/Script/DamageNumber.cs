using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro text; // <- Changed from TextMeshProUGUI
    private float lifetime = 3f;
    private Color originalColor;

    private void OnEnable()
    {
        originalColor = text.color;
        StartCoroutine(FadeAndReturn());
    }

    public void SetText(int amount)
    {
        text.text = amount.ToString();
    }

    private System.Collections.IEnumerator FadeAndReturn()
    {
        float timer = 0f;

        while (timer < lifetime)
        {
            Color c = originalColor;
            c.a = Mathf.Lerp(1, 0, timer / lifetime);
            text.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        DamageNumberPool.Instance.ReturnToPool(this);
    }
}
