using UnityEngine;
using System;
using TMPro;

public class TowerAi : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float attackRange;
    [SerializeField] private float speedRotateTower;
    [SerializeField] private float rotatinModifier;
    [SerializeField] private float minimalDistanceForDetected;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private bool towerInAttackRange;
    [SerializeField] private Bullet bullet;
  //  [SerializeField] private TMP_Text distanceDisplay;

    private bool _alreadyAttacked = true;
    private Transform _transform;
    private EnemyAi _closestEnemyPosition;
    private float _distanceToEnemy;
    private float _timeBetweenAttacks = 2;
    private int _bulletDamage;

    private void Start()
    {
        _transform = GetComponent<Transform>();       
    }

    private void Update()
    {
        AttackTower();
        FindClosestEnemy();
        LookToClosestEnemy();
        UpgradeTower();
    }    

    private void AttackTower()
    {
        towerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if (towerInAttackRange)
        {
            if (_alreadyAttacked)
            {
                TowerCombat();

                _alreadyAttacked = false;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }
        }       
    }

    private void ResetAttack()
    {
        _alreadyAttacked = true;
    }

    private void TowerCombat()
    {
        var spawnedBullet = Instantiate(bullet, _transform.position, _transform.rotation);
        spawnedBullet.BulletSpeed = bulletSpeed;
        spawnedBullet.BulletDamage = _bulletDamage;
    }

    private void FindClosestEnemy()
    {
        if (_distanceToEnemy < minimalDistanceForDetected)
        {
           // distanceDisplay.color = Color.red;
        }
        else
        {
          //  distanceDisplay.color = new Color(1, 0.7f, 0);
        }

        float distanceToClosestEnemy = Mathf.Infinity;
        EnemyAi closestEnemy = null;
        EnemyAi[] allEnemiees = GameObject.FindObjectsOfType<EnemyAi>();

        foreach (EnemyAi currentEnemy in allEnemiees)
        {         
            _distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;

            if (_distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = _distanceToEnemy;
                closestEnemy = currentEnemy;
                _closestEnemyPosition = closestEnemy;

                Vector3 ClosestEnemyPositionOnFloat = _closestEnemyPosition.transform.position;
                _distanceToEnemy = ClosestEnemyPositionOnFloat.magnitude;

              //  distanceDisplay.text = "Distance To Closest Enemy: " + _distanceToEnemy.ToString("0.0") + "m";
            }      
        }
    }

    private void LookToClosestEnemy()
    {
        if(_closestEnemyPosition != null)
        {
            Vector3 vectorToTarget = _closestEnemyPosition.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotatinModifier;
            Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * speedRotateTower);
        }       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void UpgradeTower()
    {
      //  _bulletDamage = PlayerPrefs.GetInt("BulletDamage");
       // _timeBetweenAttacks = PlayerPrefs.GetInt("AttackSpeed");
       // attackRange = PlayerPrefs.GetInt("AttackRange");
    }
}