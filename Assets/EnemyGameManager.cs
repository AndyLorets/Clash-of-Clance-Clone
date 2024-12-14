using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyGameManager : MonoBehaviour
{
    [SerializeField] private int attackRange;
    [SerializeField] private Text goldForEnemy;
    [SerializeField] private EnemyAi enemy;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float waveInetrval;

    [SerializeField] private float XPosMin, XPosMax;
    [SerializeField] private float YPosMin,YPosMax;

    public static int Gold;

    private Transform _transform;
    private int _enemyCount;   
    private int _attackSpeed;
    private int _bulletDamage;
    private int _counter;

    public int EnemyCount
    {
        get => _enemyCount;
        set => _enemyCount = value;
    }

    private void Start()
    {
        attackRange = 5;
        _attackSpeed = 1;
        _bulletDamage = 25;

        _transform = GetComponent<Transform>();

        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
       // goldForEnemy.text = "Gold: " + Gold.ToString();
    }

    private void OnEnable()
    {
        EnemyAi.EnemyDied += AddScore;
    }

    private void OnDisable()
    {
        EnemyAi.EnemyDied -= AddScore;
    }

    private void AddScore()
    {
        Gold += 10;
        if (goldForEnemy == null) return;
        goldForEnemy.text = "Gold: " + Gold.ToString();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
           // if (EnemyCount <= 0)
            {
                float Xpos;
                float Ypos;
                for (int i = 0; i < enemiesPerWave; i++)
                {
                       Ypos = Random.Range(YPosMin, YPosMax);
                       Xpos = Random.Range(XPosMin, XPosMax);

                    //  float posX = _transform.position.x + (Mathf.Cos(randDirection * Mathf.Deg2Rad) * randDistance);
                    //  float posY = _transform.position.y + (Mathf.Sin(randDirection * Mathf.Deg2Rad) * randDistance);
                    // var spawnedEnemy = Instantiate(enemy, new Vector3(posX, posY, 0), Quaternion.identity);
                    var spawnedEnemy = Instantiate(enemy, new Vector3(Xpos, Ypos, 0), Quaternion.identity);
                    spawnedEnemy.Initialize(playerTransform, this);
                    EnemyCount++;
                    yield return new WaitForSeconds(spawnInterval);
                }
            }
            yield return new WaitForSeconds(waveInetrval);
        }
    }  
}