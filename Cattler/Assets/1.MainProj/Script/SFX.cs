using UnityEngine;
using TMPro;

public class StatFX : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;
    public float lifetime = 2f;
    public float floatSpeed = 1f;
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        text = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalColor = text.color;
        StartCoroutine(FadeAndReturn());
    }

    public void SetIcon(Sprite sprite)
    {
        if (sprite == null)
        {
            spriteRenderer.sprite = null;    
        }

        else if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    public void SetText(float amount, string stat)
    {
        text.text = $"+{amount} {stat}"; 
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

        StatFXManager.instance.ReturnToPool(this);
    }
}
