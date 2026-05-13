using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float veloMov = 5f;
    public float fuerzaDeSalto = 10f;
    public int saltosMaximos = 2;

    public int vida = 100;
    public float tiempoInvencible = 1f;

    public int daño = 20;
    public Transform puntoGolpe;
    public float radioGolpe = 1f;
    public LayerMask capaEnemigo;

    public float fuerzaDash = 15f;
    public float duracionDash = 0.2f;

    private Rigidbody2D rb;
    private Animator animacos;

    private float x;
    private int saltosRestantes;
    private bool atacando = false;
    private bool haciendoDash = false;
    private bool muerto = false;
    private bool puedeRecibirDaño = true;
    private bool yaGolpeo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animacos = GetComponent<Animator>();
        saltosRestantes = saltosMaximos;
    }

    void Update()
    {
        if (muerto || atacando)
            return;

        if (!haciendoDash)
        {
            x = Input.GetAxisRaw("Horizontal");

            if (animacos != null)
                animacos.SetBool("estacorriendo", x != 0);

            if (x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            if (x > 0)
                transform.localScale = new Vector3(1, 1, 1);

            rb.linearVelocity = new Vector2(x * veloMov, rb.linearVelocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaDeSalto);
                saltosRestantes--;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
        }

        if (animacos != null)
            animacos.SetBool("estaSaltando", saltosRestantes < saltosMaximos);

        if (Input.GetMouseButtonDown(0) && !atacando && !haciendoDash)
        {
            Atacar();
        }
    }

    IEnumerator Dash()
    {
        haciendoDash = true;

        if (animacos != null)
            animacos.SetTrigger("dash");

        float direccion = transform.localScale.x;
        float gravedadOriginal = rb.gravityScale;

        rb.gravityScale = 0f;

        float tiempo = 0f;

        while (tiempo < duracionDash)
        {
            rb.linearVelocity = new Vector2(direccion * fuerzaDash, 0f);
            tiempo += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = gravedadOriginal;
        haciendoDash = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            saltosRestantes = saltosMaximos;

            if (animacos != null)
                animacos.SetBool("estaSaltando", false);
        }
    }

    public void RecibirDaño(int dañoRecibido)
    {
        if (!puedeRecibirDaño || muerto)
            return;

        vida -= dañoRecibido;

        if (CamaraSeguir.instancia != null)
        {
            CamaraSeguir.instancia.Shake(0.15f, 0.15f);
        }

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