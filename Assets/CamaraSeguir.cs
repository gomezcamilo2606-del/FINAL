using UnityEngine;
using System.Collections;

public class CamaraSeguir : MonoBehaviour
{
    public static CamaraSeguir instancia;

    public Transform jugador;
    public float suavizado = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 shakeOffset = Vector3.zero;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        if (jugador == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                jugador = player.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (jugador == null)
            return;

        Vector3 destino = jugador.position + offset + shakeOffset;

        transform.position = destino;
    }

    public void Shake(float duracion, float intensidad)
    {
        StopAllCoroutines();
        StartCoroutine(HacerShake(duracion, intensidad));
    }

    IEnumerator HacerShake(float duracion, float intensidad)
    {
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            shakeOffset = new Vector3(
                Random.Range(-intensidad, intensidad),
                Random.Range(-intensidad, intensidad),
                0f
            );

            tiempo += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}