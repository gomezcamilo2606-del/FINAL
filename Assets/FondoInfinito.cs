using UnityEngine;

public class FondoInfinito : MonoBehaviour
{
    private Transform jugador;

    public float velocidad = 0.5f;

    private float ancho;
    private Vector3 posicionInicial;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        ancho = GetComponent<SpriteRenderer>().bounds.size.x;
        posicionInicial = transform.position;
    }

    void Update()
    {
        float movimiento = jugador.position.x * velocidad;

        transform.position = new Vector3(
            posicionInicial.x + movimiento,
            posicionInicial.y,
            posicionInicial.z
        );

        if (jugador.position.x > transform.position.x + ancho)
        {
            posicionInicial += new Vector3(ancho * 2f, 0, 0);
        }
    }
}