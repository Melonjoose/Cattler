using UnityEngine;
using UnityEngine.UI;

public class HealthbarUIElement : MonoBehaviour
{
    private Healthbar target;
    private Slider slider;
    private Camera mainCam;

    public void Initialize(Healthbar target, Slider slider, Camera cam)
    {
        this.target = target;
        this.slider = slider;
        this.mainCam = cam;
    }

    public void UpdatePosition(Vector3 healthBarLocation)
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(healthBarLocation);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            screenPos,
            mainCam,
            out Vector2 localPoint
        );

        transform.localPosition = localPoint;
    }

    public void UpdateHealth(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    public void RemoveHealthbar()
    {
        Destroy(gameObject);
    }
}
