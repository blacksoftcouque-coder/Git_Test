using UnityEngine;

public class Gun : MonoBehaviour
{
    // 총을 발사하는 로직.
    // ShootPosition을 가지고, Bullet을 소환.

    private ShootPosition shootPosition = null;
    private AudioSource shootSound = null;

    [SerializeField] private GameObject bullet = null;

    private void Awake()
    {
        shootPosition = GetComponentInChildren<ShootPosition>();
        shootSound = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        if (bullet != null)
        {
            shootSound?.Play();
            Instantiate(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
        }
    }
}
