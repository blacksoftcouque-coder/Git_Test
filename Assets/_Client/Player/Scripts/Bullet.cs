using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float liveTime = 3f;
    private float nowTime = 0f;
    
    // 소환됐을 때 앞으로 날라가기만 하기.
    private Rigidbody rb = null;
    [SerializeField] private GameObject effectPrefab = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
        StartCoroutine(CheckLive());
    }

    private IEnumerator CheckLive()
    {
        while (true)
        {
            nowTime += Time.deltaTime;

            if (nowTime > liveTime)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(50f);

        if(other.GetComponent<IDamageable>() != null)
            Instantiate(effectPrefab, other.ClosestPoint(transform.position), Quaternion.identity);


        Destroy(gameObject);
    }
}
