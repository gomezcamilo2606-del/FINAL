using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public int daño = 20;
    public float tiempoDeVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemigo = other.GetComponent<EnemyController>();

        if (enemigo != null)
        {
            enemigo.RecibirDaño(daño);
        }

        Destroy(gameObject);
    }
}