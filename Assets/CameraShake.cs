using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instancia;

    private Vector3 offsetShake = Vector3.zero;

    void Awake()
    {
        instancia = this;
    }

    public Vector3 ObtenerOffset()
    {
        return offsetShake;
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
            offsetShake = new Vector3(
                Random.Range(-1f, 1f) * intensidad,
                Random.Range(-1f, 1f) * intensidad,
                0f
            );

            tiempo += Time.deltaTime;
            yield return null;
        }

        offsetShake = Vector3.zero;
    }
}