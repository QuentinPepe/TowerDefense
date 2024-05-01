using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fill;
    private Image _fillImage;

    // Start is called before the first frame update
    void Awake()
    {
        transform.parent.GetComponent<LivingEntity>().OnDamageTaken += UpdateHealth;
        _fillImage = fill.GetComponent<Image>();
    }

    private void OnEnable()
    {
        fill.anchorMax = new Vector2(1, fill.anchorMax.y);
        _fillImage.color = Color.green;
    }


    private void UpdateHealth(float normalizedHealth)
    {
        fill.anchorMax = new(normalizedHealth, fill.anchorMax.y);
        _fillImage.color = Color.Lerp(Color.red, Color.green, normalizedHealth);
    }
}