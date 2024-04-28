using System;
using UnityEngine;

public class PlayerEntity : LivingEntity
{
    
    [SerializeField] private int maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnDamageTaken?.Invoke(_currentHealth/(float)maxHealth);
        Game.Instance.CheckGameOver(_currentHealth);
    }
    
}