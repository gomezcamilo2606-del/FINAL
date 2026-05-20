using UnityEngine;

public class Cura : MonoBehaviour
{
    public GameObject iconoCura;
    public static bool tieneCura;

    void Start()
    {
        if (iconoCura != null)
            iconoCura.SetActive(tieneCura);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tieneCura = true;

            if (iconoCura != null)
                iconoCura.SetActive(true);

            Destroy(gameObject);
        }
    }
}