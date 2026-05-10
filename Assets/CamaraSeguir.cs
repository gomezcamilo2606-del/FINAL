using UnityEngine;

public class CamaraSeguir: MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    void Start()
    {
        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        Vector3 destino = jugador.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            destino,
            suavizado * Time.deltaTime
        );
    }
}