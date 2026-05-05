using UnityEngine;
using UnityEngine.UI;

public class Vidas : MonoBehaviour
{
    public Image rellenoBarraVida;

    private PlayerController playerController;
    private float vidaMaxima;

    void Start()
    {
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            vidaMaxima = playerController.vida;
        }
        else
        {
            Debug.LogError("No se encontró el Player");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            rellenoBarraVida.fillAmount = (float)playerController.vida / vidaMaxima;
        }
    }
}