using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro text; // <- Changed from TextMeshProUGUI
    private float lifetime = 2f;
    public float floatSpeed = 1.0f;
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
            transform.position += Vector3.up * floatSpeed * Time.deltaTime; //floats upwards
            Color c = originalColor;
            c.a = Mathf.Lerp(1, 0, timer / lifetime);
            text.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        DamageNumberPool.Instance.ReturnToPool(this);
    }
}
