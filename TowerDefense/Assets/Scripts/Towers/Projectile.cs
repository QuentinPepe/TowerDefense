﻿using CreatureS;
using UnityEngine;

namespace Towers
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody _rb;
        private GameObject _target;
        private int _damage;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_target) return;
            _rb.velocity = -speed * (transform.position - _target.transform.position);
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ICreature creature))
            {
                creature.TakeDamage(_damage);
                Destroy(gameObject);
            }

            if (other.gameObject.layer.CompareTo(LayerMask.NameToLayer("Ground")) == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}