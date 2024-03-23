using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public int health;

    private EnemySpawner enemySpawner;

    private void Start()
    {
        GameObject enemySpawnerObject = GameObject.Find("EnemySpawner");

        enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Killed");
            enemySpawner.DecreaseEnemyCount();
        }
    }


}
//Update