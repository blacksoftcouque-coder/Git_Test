using System;
using System.Collections;
using UnityEngine;

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

    private Player player = null;
    private AudioSource audioSource = null;

    private Action<Enemy, int> OnEnemyDie = null;

    private void Awake()
    {
        hp = maxHP;
        audioSource = GetComponent<AudioSource>();
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
            // 플레이어를 위치 쫓아가는 로직.
            Vector3 direction =  (player.Position - transform.position).normalized;
            float distance = (player.Position - transform.position).magnitude;

            if (distance >= 3f)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
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
