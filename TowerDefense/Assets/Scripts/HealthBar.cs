using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fill;
    private Image _fillImage;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetComponent<LivingEntity>().OnDamageTaken += UpdateHealth;
        _fillImage = fill.GetComponent<Image>();
    }

    private void UpdateHealth(float normalizedHealth)
    {
        fill.anchorMax = new(fill.anchorMax.x, normalizedHealth);
        _fillImage.color = Color.Lerp(Color.red, Color.green, normalizedHealth);
    }
}