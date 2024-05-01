using System;
using CreatureS;
using UnityEngine;

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
        if (transform.position.y < 0) Destroy(gameObject);
        if (!_target) return;
        _rb.velocity = speed * (transform.position - _target.transform.position);
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Creature creature))
        {
            creature.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}