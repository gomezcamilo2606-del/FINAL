using UnityEngine;

public class Cura : MonoBehaviour
{
    public GameObject iconoCura;
    public static bool tieneCura = false;

    void Start()
    {
        tieneCura = false;
        iconoCura.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tieneCura = true;
            iconoCura.SetActive(true);
            Destroy(gameObject);
        }
    }
}