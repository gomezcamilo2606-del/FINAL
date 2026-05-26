using UnityEngine;

public class Cura : MonoBehaviour
{
    public GameObject iconoCura;
    public static bool tieneCura;

    void Start()
    {
        tieneCura = false;

        if (iconoCura != null)
            iconoCura.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tieneCura = true;

            if (iconoCura != null)
                iconoCura.SetActive(true);

            SFXManager.instancia.SonidoCura();

            Destroy(gameObject);
        }
    }
}