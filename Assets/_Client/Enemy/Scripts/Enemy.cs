using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    // 데미지를 받을 수 있음.
    // 체력.
    // 죽으면 줄 점수.

    [SerializeField] 
    private float hp = 0f;
    private float maxHP = 100f;
    private int score = 10; 
    
    [SerializeField]
    private float moveSpeed = 3f;

    // 공격을 시작하는 거리.
    [SerializeField]
    private float attackRange = 3f;

    private Player player = null;
    private AudioSource audioSource = null;
    private NavMeshAgent agent = null;

    private Action<Enemy, int> OnEnemyDie = null;

    private void Awake()
    {
        hp = maxHP;
        audioSource = GetComponent<AudioSource>();

        // 기둥, 상자 같은 장애물을 피해서 이동하도록 NavMeshAgent 사용.
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange * 0.9f;
    }

    public void Init(Player player, Action<Enemy, int> OnEnemyDie)
    {
        this.player = player;
        this.OnEnemyDie = OnEnemyDie;
        StartCoroutine(CoChasePlayer());
    }

    private IEnumerator CoChasePlayer()
    {
        while (true)
        {
            // 플레이어를 위치 쫓아가는 로직. (NavMesh 경로로 장애물을 돌아감)
            float distance = (player.Position - transform.position).magnitude;

            if (distance >= attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.Position);
            }
            else
            {
                agent.isStopped = true;
                player.TakeDamage(10f);
                yield return new WaitForSecondsRealtime(2f);
            }

            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        audioSource.Play();

        if (hp <= 0f)
        {
            Died();
        }
    }

    private void Died()
    {
        OnEnemyDie?.Invoke(this, score);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();

    }
}
