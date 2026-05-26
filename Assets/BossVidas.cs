using UnityEngine;
using UnityEngine.UI;

public class BossVidas : MonoBehaviour
{
    public Image rellenoBarraVida;
    public FinalBoss boss;

    private float vidaMaxima;

    void Start()
    {
        if (boss != null)
        {
            vidaMaxima = boss.maxHealth;
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (boss == null || rellenoBarraVida == null)
            return;

        if (boss.peleaIniciada && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        rellenoBarraVida.fillAmount =
            (float)boss.currentHealth / vidaMaxima;
    }
}