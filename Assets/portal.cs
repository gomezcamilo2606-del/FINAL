using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    public string nombreEscena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Cura.tieneCura)
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}