using UnityEngine;

public class Daño : MonoBehaviour
{
    public int daño = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.RecibirDaño(daño);
            }
        }
    }
}