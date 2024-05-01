using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private GameObject _target;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!_target) return;
        _rb.velocity = speed * (transform.position-_target.transform.position);
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}