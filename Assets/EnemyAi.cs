using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private int damageAmount;

    private int _health = 100;
    private Transform _towerTransform;
    private EnemyGameManager _enemySpawner;
    private Rigidbody _rb;

    public static event Action EnemyDied;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        EnemyMoveToTarget();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<TowerHealth>();
            player.TakeDamage(damageAmount);
            TakeDamage(_health);
        }
    }

    public void Initialize(Transform player, EnemyGameManager spawner)
    {
        _towerTransform = player;
        _enemySpawner = spawner;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            _enemySpawner.EnemyCount--;
            EnemyDied?.Invoke();            
            Destroy(gameObject);
        }
    }

    private void EnemyMoveToTarget()
    {
        _rb.position = Vector3.MoveTowards(_rb.position, _towerTransform.position, walkSpeed * Time.deltaTime);
    }
}