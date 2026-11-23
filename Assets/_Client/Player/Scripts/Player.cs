using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float hp = 0f;
    private float maxHp = 100f;

    [SerializeField]
    private float moveSpeed = 0f;

    [SerializeField]
    private float jumpPower = 0f;

    [SerializeField]
    private float currentPitch = 0f;


    private Gun gun = null;
    private Rigidbody rb = null;
    private PlayerCamera mainCamera = null;
    private MeshRenderer meshRenderer = null;

    private Color originColor = default;

    public Vector3 Position { get { return transform.position; } }

    private Action onPlayerDie = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<Gun>();
        mainCamera = GetComponentInChildren<PlayerCamera>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        originColor = meshRenderer.material.color;

        hp = maxHp;
    }

    public void Init(Action onPlayerDie)
    {
        originColor = meshRenderer.material.color;
        this.onPlayerDie = onPlayerDie;
    }

    private void Update()
    {
        HandleCursor();
        Shoot();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Rotate();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 총알 발사.
            gun.Attack();
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        StartCoroutine(CoDamageEffect());
        
        if (hp <= 0f)
        {
            Died();
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
    }

    private IEnumerator CoDamageEffect()
    {
        meshRenderer.material.color = Color.indianRed;
        yield return new WaitForSecondsRealtime(0.2f);
        meshRenderer.material.color = originColor;
    }

    private void Died()
    {
        onPlayerDie?.Invoke();
    }


    #region Control
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction
            = transform.forward * vertical + transform.right * horizontal;

        Vector3 normalizedDirection = direction.normalized;

        Vector3 velocity =
            new Vector3(normalizedDirection.x * moveSpeed, rb.linearVelocity.y, normalizedDirection.z * moveSpeed);

        rb.linearVelocity = velocity;
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentPitch -= mouseY * 2f;
        currentPitch
            = Mathf.Clamp(currentPitch, -90f, 90f);

        mainCamera.transform.localEulerAngles
            = new Vector3(currentPitch, 0f, 0f);


        float playerRotation = mouseX * 2f; 
        transform.Rotate(0f, playerRotation, 0f);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1.1f))
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(new Vector3(0f, 1f, 0f) * jumpPower, ForceMode.Impulse);
            }
        }
    }

    private void HandleCursor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
    #endregion
}
