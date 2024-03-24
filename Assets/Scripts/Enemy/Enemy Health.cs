using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public int health;

    private EnemySpawner enemySpawner;
    private PlayerStats playerStats;
    public int nextLevelReward;
    public int borricalReward; //50 For statues, 15 statues | 2 For Regular

    private void Start()
    {
        GameObject enemySpawnerObject = GameObject.Find("EnemySpawner");

        enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();

        GameObject playerStatsObject = GameObject.Find("Player");

        playerStats = playerStatsObject.GetComponent<PlayerStats>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Killed");
            enemySpawner.DecreaseEnemyCount();
            playerStats.borricals += borricalReward;
            playerStats.progressToNextLevel += nextLevelReward;
            playerStats.kills += 1;
        }
    }


}
//Update