using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    public Transform jugador;   // Arrastra aquí el Player
    public float suavizado = 5f;

    public Vector3 offset;      // Ajuste de posición (ej: 0, 2, -10)

    void LateUpdate()
    {
        if (jugador == null) return;

        Vector3 posicionDeseada = jugador.position + offset;
        Vector3 posicionSuave = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        transform.position = posicionSuave;
    }
}