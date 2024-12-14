using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TowerHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public static event Action<int> OnDamageTaken;    

    public void TakeDamage(int amount)
    {
        health -= amount;
        OnDamageTaken?.Invoke(health);

        if (health <= 0)
        {
            Invoke(nameof(Restart), 2f);
            gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}