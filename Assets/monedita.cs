using UnityEngine;
using TMPro;
public class monedita : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI puntuacionTexto;

    private int puntuacion;

    private string puntuacionCadena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            puntuacion = int.Parse(puntuacionTexto.text);
            puntuacion++;
            puntuacionCadena = puntuacion.ToString();
            puntuacionTexto.text = puntuacionCadena;
            Destroy(this.gameObject);

            Debug.Log("puntuacion =" + puntuacion.ToString());
        }
    }

}