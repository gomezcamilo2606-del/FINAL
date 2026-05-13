using UnityEngine;
using TMPro;

public class monedita : MonoBehaviour
{
    public TextMeshProUGUI puntuacionTexto;
    private static int puntuacion = 0;

    private void Start()
    {
        puntuacionTexto.text = puntuacion.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            puntuacion++;
            puntuacionTexto.text = puntuacion.ToString();
            Destroy(gameObject);
        }
    }
}