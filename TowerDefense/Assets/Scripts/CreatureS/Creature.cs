using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : LivingEntity
{
    private NavMeshAgent _navMeshAgent;
    private CreatureSO _data;
    private int _currentHealth;
    public Action<Creature> OnCreatureEliminated;
    public Action<Creature> OnCreatureReachedEnd;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        OnCreatureEliminated += Game.Instance.HandleCreatureEliminated;
        OnCreatureReachedEnd += Game.Instance.HandleCreatureReachedEnd;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnDamageTaken?.Invoke(_currentHealth / (float)_data.health);
        if (_currentHealth <= 0)
        {
            OnCreatureEliminated?.Invoke(this);
        }
    }

    public void MoveTo(Vector3 target)
    {
        _navMeshAgent.destination = target;
    }
}