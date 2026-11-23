using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab = null;

    private SpawnPoint[] spawnPointArray = null;
    private List<Enemy> enemies = null;

    private Action<int> OnEnemyDie = null;

    private void Awake()
    {
        spawnPointArray = GetComponentsInChildren<SpawnPoint>();
        enemies = new List<Enemy>();
    }

    public void Init(Player player, Action<int> onEnemyDie)
    {
        SpawnEnemies(player);
        this.OnEnemyDie = onEnemyDie;
    }

    private void SpawnEnemies(Player player)
    {
        StartCoroutine(CoSpawnEnemy(player));
    }

    private void SpawnEnemy(Player player)
    {
        // Enemy를 소환하려면 Prefab을 등록, 게임 오브젝트를 소환한 다음에 Enemy의 Init 실행.
        int randomIndex = UnityEngine.Random.Range(0, spawnPointArray.Length);

        GameObject enemy = Instantiate(enemyPrefab, spawnPointArray[randomIndex].transform.position,
            spawnPointArray[randomIndex].transform.rotation, transform);

        Enemy enemyComponemet = enemy.GetComponent<Enemy>();
        if (enemyComponemet != null)
        {
            enemy.GetComponent<Enemy>()?.Init(player, OnEnemyDied);
            enemies.Add(enemy.GetComponent<Enemy>());
        }
    }

    private IEnumerator CoSpawnEnemy(Player player)
    {
        while (true)
        {
            if (enemies.Count < 5)
            {
                SpawnEnemy(player);

                yield return new WaitForSecondsRealtime(2f);
            }

            yield return null;
        }
    }

    private void OnEnemyDied(Enemy enemy, int score)
    {
        RemoveEnemy(enemy);
        OnEnemyDie?.Invoke(score);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
