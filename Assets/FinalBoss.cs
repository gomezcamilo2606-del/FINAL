using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;

    [Header("Audio")]
    public AudioSource musicaNivel;

    [Header("Vida")]
    public int maxHealth = 300;
    public int currentHealth;

    [Header("Movimiento")]
    public float moveSpeed = 3f;
    public float stopDistance = 2f;

    [Header("Detección")]
    public float detectionRange = 20f;
    public float verticalDetection = 6f;

    [Header("Salto")]
    public float jumpForce = 10f;
    public float jumpCooldown = 2f;
    private bool canJump = true;

    [Header("Suelo")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Daño")]
    public int damage = 2;
    public float attackCooldown = 1f;
    private bool canAttack = true;

    [Header("Estado")]
    public bool phase2 = false;
    public bool peleaIniciada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null || groundCheck == null)
            return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        FlipToPlayer();

        if (currentHealth <= maxHealth / 2 && !phase2)
        {
            phase2 = true;
            moveSpeed = 5f;
            jumpCooldown = 1f;
            damage = 4;
        }

        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);

        if (horizontalDistance > detectionRange || verticalDistance > verticalDetection)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (!peleaIniciada)
        {
            peleaIniciada = true;

            if (musicaNivel != null)
            {
                musicaNivel.Stop();
            }

            if (SFXManager.instancia != null)
            {
                SFXManager.instancia.IniciarBossMusic();
            }
        }

        if (horizontalDistance > stopDistance)
        {
            float direction = player.position.x > transform.position.x ? 1f : -1f;

            rb.linearVelocity = new Vector2(
                direction * moveSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (isGrounded && canJump)
        {
            StartCoroutine(JumpAttack());
        }
    }

    IEnumerator JumpAttack()
    {
        canJump = false;

        float direction = player.position.x > transform.position.x ? 1f : -1f;

        rb.linearVelocity = new Vector2(
            direction * moveSpeed * 2f,
            jumpForce
        );

        yield return new WaitForSeconds(jumpCooldown);

        canJump = true;
    }

    IEnumerator DealDamage(PlayerController playerController)
    {
        canAttack = false;

        playerController.RecibirDaño(damage);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            PlayerController playerController =
                collision.gameObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                StartCoroutine(DealDamage(playerController));
            }
        }
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            if (SFXManager.instancia != null)
            {
                SFXManager.instancia.DetenerBossMusic();
            }

            Destroy(gameObject);
        }
    }

    void FlipToPlayer()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 size = new Vector3(
            detectionRange * 2,
            verticalDetection * 2,
            1
        );

        Gizmos.DrawWireCube(transform.position, size);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}