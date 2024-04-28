using System;
using UnityEngine;
using UnityEngine.AI;

namespace CreatureS
{
    public class Creature : LivingEntity
    {
        private NavMeshAgent _navMeshAgent;
        public CreatureSO Data { get; private set; }
        private int _currentHealth;
        public Action<Creature> OnCreatureEliminated;
        public Action<Creature> OnCreatureReachedEnd;

        public void Initialize(CreatureSO creatureData)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Data = creatureData;
            _navMeshAgent.speed = Data.speed;
            _currentHealth = Data.health;
        }

        private void Start()
        {
            OnCreatureEliminated += Game.Instance.HandleCreatureEliminated;
            OnCreatureReachedEnd += Game.Instance.HandleCreatureReachedEnd;
        }

        public override void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnDamageTaken?.Invoke(_currentHealth / (float)Data.health);
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
}