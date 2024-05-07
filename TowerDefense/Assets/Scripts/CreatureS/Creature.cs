using System;
using UnityEngine;
using UnityEngine.AI;

namespace CreatureS
{
    public class Creature : LivingEntity, ICreature
    {
        private NavMeshAgent _navMeshAgent;
        public CreatureSO Data { get; private set; }
        public int CurrentHealth { get; private set; }
        private Action<ICreature> _onCreatureEliminated;
        private Action<ICreature> _onCreatureReachedEnd;

        private AudioSource _audioSource;

        public void Initialize(CreatureSO creatureData)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Data = creatureData;
            _navMeshAgent.speed = Data.speed;
            CurrentHealth = Data.health;
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
                CurrentHealth = Data.health;
        }

        public override void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            OnDamageTaken?.Invoke(CurrentHealth / (float)Data.health);
            _audioSource.Play();
            if (CurrentHealth <= 0)
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
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        public bool IsActive()
        {
            return gameObject.activeSelf;
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