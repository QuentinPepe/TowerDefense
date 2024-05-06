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
        private Action<Creature> _onCreatureEliminated;
        private Action<Creature> _onCreatureReachedEnd;

        private AudioSource _audioSource;

        public void Initialize(CreatureSO creatureData)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Data = creatureData;
            _navMeshAgent.speed = Data.speed;
            _currentHealth = Data.health;
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = Data.hitSound;
        }

        private void Start()
        {
            _onCreatureEliminated += Game.Instance.HandleCreatureEliminated;
            _onCreatureReachedEnd += Game.Instance.HandleCreatureReachedEnd;
        }

        public void OnEnable()
        {
            if (Data != null)
                _currentHealth = Data.health;
        }

        public override void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnDamageTaken?.Invoke(_currentHealth / (float)Data.health);
            _audioSource.Play();
            if (_currentHealth <= 0)
            {
                _onCreatureEliminated?.Invoke(this);
            }
        }

        public void MoveTo(Vector3 target)
        {
            _navMeshAgent.destination = target;
        }
        public void TeleportTo(Vector3 position)
        {
            _navMeshAgent.Warp(position);
        }

        private void Update()
        {
            if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= 0.1f)
            {
                _onCreatureReachedEnd?.Invoke(this);
            }
        }
    }
}