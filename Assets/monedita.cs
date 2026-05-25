using UnityEngine;
using TMPro;

public class monedita : MonoBehaviour
{
    private static int puntuacion = 0;
    private TextMeshProUGUI puntuacionTexto;

    void Start()
    {
        GameObject texto = GameObject.Find("TextoPuntuacion");

        if (texto != null)
        {
            puntuacionTexto = texto.GetComponent<TextMeshProUGUI>();
            puntuacionTexto.text = puntuacion.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            puntuacion++;

            if (puntuacionTexto != null)
            {
                puntuacionTexto.text = puntuacion.ToString();
            }

            Destroy(gameObject);
        }
    }
}