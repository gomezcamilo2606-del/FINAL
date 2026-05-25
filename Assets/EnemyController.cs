using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public Transform puntoA;
    public Transform puntoB;
    private Transform objetivo;

    [Header("Combate")]
    public int vida = 5;
    public int daño = 1;

    public float rangoDeteccion = 4f;
    public float rangoAtaque = 1.5f;
    public float tiempoEntreAtaques = 1f;

    private float tiempoSiguienteAtaque;
    private Transform jugador;

    void Start()
    {
        objetivo = puntoB;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            jugador = playerObj.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= rangoAtaque)
        {
            Atacar();
        }
        else if (distancia <= rangoDeteccion)
        {
            SeguirJugador();
        }
        else
        {
            Patrullar();
        }
    }

    void Patrullar()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, objetivo.position) < 0.2f)
        {
            objetivo = (objetivo == puntoA) ? puntoB : puntoA;
            Girar();
        }
    }

    void SeguirJugador()
    {
        Vector2 objetivoSuelo = new Vector2(
            jugador.position.x,
            transform.position.y
        );

        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivoSuelo,
            velocidad * Time.deltaTime
        );

        if (jugador.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Atacar()
    {
        if (Time.time >= tiempoSiguienteAtaque)
        {
            tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;

            PlayerController player = jugador.GetComponent<PlayerController>();

            if (player != null)
            {
                player.RecibirDaño(daño);
            }
        }
    }

    public void RecibirDaño(int dañoRecibido)
    {
        vida -= dañoRecibido;

        Debug.Log("Vida enemigo: " + vida);

        if (vida <= 0)
        {
            Debug.Log("ENEMIGO MUERTO");
            Destroy(gameObject);
        }
    }

    void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}