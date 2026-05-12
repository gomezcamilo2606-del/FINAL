using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float veloMov = 5f;
    public float fuerzaDeSalto = 7f;

    [Header("Vida")]
    public int vida = 14;
    public float tiempoInvencible = 1f;

    [Header("Dash")]
    public float fuerzaDash = 12f;
    public float duracionDash = 0.2f;

    [Header("Ataque")]
    public Transform puntoGolpe;
    public float radioGolpe = 1.2f;
    public LayerMask capaEnemigo;
    public int daño = 1;

    private float x;
    private Rigidbody2D rb;
    private Animator animacos;

    private int saltosMaximos = 2;
    private int saltosRestantes;

    private bool puedeRecibirDaño = true;
    private bool muerto = false;
    private bool atacando = false;
    private bool haciendoDash = false;
    private bool yaGolpeo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animacos = GetComponent<Animator>(); // toma el animator automáticamente
        saltosRestantes = saltosMaximos;
    }

    void Update()
    {
        if (muerto || atacando || haciendoDash)
            return;

        x = Input.GetAxisRaw("Horizontal");

        // Animación correr
        if (animacos != null)
            animacos.SetBool("estacorriendo", x != 0);

        // Girar personaje
        if (x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        // Movimiento
        rb.linearVelocity = new Vector2(x * veloMov, rb.linearVelocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaDeSalto);
            saltosRestantes--;
        }

        // Ataque
        if (Input.GetMouseButtonDown(0) && !atacando)
        {
            Atacar();
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        haciendoDash = true;

        float direccion = transform.localScale.x;
        rb.linearVelocity = new Vector2(direccion * fuerzaDash, 0f);

        yield return new WaitForSeconds(duracionDash);

        haciendoDash = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            saltosRestantes = saltosMaximos;
        }
    }

    public void RecibirDaño(int dañoRecibido)
    {
        if (!puedeRecibirDaño || muerto)
            return;

        vida -= dañoRecibido;

        if (vida <= 0)
        {
            Morir();
            return;
        }

        StartCoroutine(Invencibilidad());
    }

    void Atacar()
    {
        atacando = true;
        yaGolpeo = false;

        if (animacos != null)
            animacos.SetTrigger("golpear");

        Invoke(nameof(FinAtaque), 0.5f);
    }

    void FinAtaque()
    {
        atacando = false;
    }

    public void HacerDaño()
    {
        if (yaGolpeo)
            return;

        yaGolpeo = true;

        if (puntoGolpe == null)
        {
            Debug.LogWarning("PuntoGolpe no asignado");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            puntoGolpe.position,
            radioGolpe,
            capaEnemigo
        );

        foreach (Collider2D hit in hits)
        {
            EnemyController enemigo = hit.GetComponent<EnemyController>();

            if (enemigo != null)
            {
                enemigo.RecibirDaño(daño);
            }
        }
    }

    void Morir()
    {
        muerto = true;

        if (animacos != null)
            animacos.SetTrigger("morir");

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Invoke(nameof(ReiniciarNivel), 1.5f);
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Invencibilidad()
    {
        puedeRecibirDaño = false;

        yield return new WaitForSeconds(tiempoInvencible);

        puedeRecibirDaño = true;
    }

    void OnDrawGizmosSelected()
    {
        if (puntoGolpe != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(puntoGolpe.position, radioGolpe);
        }
    }
}