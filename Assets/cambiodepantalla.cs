using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiodepantalla : MonoBehaviour
{
    public int numeroEscena;

    public void CambiarEscena()
    {
        SceneManager.LoadScene(numeroEscena);
    }
}