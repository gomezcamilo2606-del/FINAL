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

    public float fuerzaDash = 21f;
    public float duracionDash = 0.3f;
    public float cooldownDash = 0.4f;

    [Header("Disparo")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float velocidadProyectil = 15f;

    private Rigidbody2D rb;
    private Animator animacos;

    private float x;
    private int saltosRestantes;
    private bool atacando = false;
    private bool haciendoDash = false;
    private bool muerto = false;
    private bool puedeRecibirDaño = true;
    private bool puedeDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animacos = GetComponent<Animator>();
        saltosRestantes = saltosMaximos;
    }

    void Update()
    {
        if (muerto)
            return;

        if (!haciendoDash && !atacando)
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && puedeDash)
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

    void Atacar()
    {
        atacando = true;

        if (animacos != null)
            animacos.SetTrigger("golpear");

        Invoke(nameof(FinAtaque), 0.5f);
    }

    void FinAtaque()
    {
        atacando = false;
    }

    public void DispararProyectil()
    {
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning("Falta proyectil o punto de disparo");
            return;
        }

        GameObject bala = Instantiate(
            proyectilPrefab,
            puntoDisparo.position,
            Quaternion.identity
        );

        Rigidbody2D rbProyectil = bala.GetComponent<Rigidbody2D>();

        if (rbProyectil != null)
        {
            float direccion = transform.localScale.x;

            rbProyectil.linearVelocity = new Vector2(
                direccion * velocidadProyectil,
                0f
            );
        }
    }

    IEnumerator Dash()
    {
        puedeDash = false;
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

        yield return new WaitForSeconds(cooldownDash);

        puedeDash = true;
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

    void Morir()
    {
        muerto = true;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Invoke(nameof(ReiniciarNivel), 1f);
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
}