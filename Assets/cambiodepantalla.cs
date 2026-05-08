using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiodepantalla : MonoBehaviour
{
    public int numeroEscena;

    public void CambiarEscena()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(numeroEscena);
    }
}