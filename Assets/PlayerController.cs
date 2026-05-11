using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float veloMov = 5f;
    public float fuerzaDeSalto = 7f;
    public int vida = 14;

    private float x;
    private Rigidbody2D rb;

    public Animator animacos;

    private int saltosMaximos = 2;
    private int saltosRestantes;

    private bool puedeRecibirDaño = true;
    public float tiempoInvencible = 1f;

    private bool muerto = false;
    private bool atacando = false;

   
    public float fuerzaDash = 12f;
    public float duracionDash = 0.2f;
    private bool haciendoDash = false;

    
    public Transform puntoGolpe;
    public float radioGolpe = 1.2f; 
    public LayerMask capaEnemigo;
    public int daño = 1;

    private bool yaGolpeo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        saltosRestantes = saltosMaximos;
    }

    void Update()
    {
        if (muerto || atacando || haciendoDash) return;

        x = Input.GetAxisRaw("Horizontal");

        if (animacos != null)
            animacos.SetBool("estacorriendo", x != 0);

        if (x < 0) transform.localScale = new Vector3(-1, 1, 1);
        if (x > 0) transform.localScale = new Vector3(1, 1, 1);

        rb.linearVelocity = new Vector2(x * veloMov, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaDeSalto);
            saltosRestantes--;
        }

        if (Input.GetMouseButtonDown(0) && !atacando && !muerto)
        {
            Atacar();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    System.Collections.IEnumerator Dash()
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

    public void RecibirDaño(int daño)
    {
        if (!puedeRecibirDaño || muerto) return;

        vida -= daño;

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

        Invoke("FinAtaque", 0.5f);
    }

    void FinAtaque()
    {
        atacando = false;
    }

    // 💥 DAÑO REAL
    public void HacerDaño()
    {
        Debug.Log("GOLPEANDO");

        if (yaGolpeo) return;
        yaGolpeo = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(puntoGolpe.position, radioGolpe);

        foreach (Collider2D hit in hits)
        {
            // Detecta por script
            EnemyController enemigo = hit.GetComponent<EnemyController>();

            // O por tag (extra seguridad)
            if (enemigo != null || hit.CompareTag("Enemy"))
            {
                if (enemigo != null)
                {
                    enemigo.RecibirDaño(daño);
                }
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

        Invoke("ReiniciarNivel", 1.5f);
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    System.Collections.IEnumerator Invencibilidad()
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