using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fill;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LivingEntity>().OnDamageTaken += UpdateHealth;
    }

    private void UpdateHealth(float normalizedHealth)
    {
        fill.anchorMax = new(normalizedHealth, fill.anchorMax.y);
    }
}