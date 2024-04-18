using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    public Action<float> OnDamageTaken;
    public abstract void TakeDamage(int damage);
}
