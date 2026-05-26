using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogo : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelDialogo;
    public TMP_Text textoDialogo;

    [Header("Contenido")]
    [TextArea(2, 5)]
    public string[] lineas;

    [Header("Configuración")]
    public float velocidadTexto = 0.03f;

    private bool jugadorCerca = false;
    private bool dialogoActivo = false;
    private bool escribiendo = false;
    private int lineaActual = 0;
    private Coroutine escribirCoroutine;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogoActivo)
            {
                IniciarDialogo();
            }
            else
            {
                if (escribiendo)
                {
                    MostrarTextoCompleto();
                }
                else
                {
                    SiguienteLinea();
                }
            }
        }
    }

    void IniciarDialogo()
    {
        dialogoActivo = true;
        lineaActual = 0;
        panelDialogo.SetActive(true);

        if (escribirCoroutine != null)
            StopCoroutine(escribirCoroutine);

        escribirCoroutine = StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        escribiendo = true;
        textoDialogo.text = "";

        SFXManager.instancia.SonidoNPC();

        foreach (char letra in lineas[lineaActual])
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        SFXManager.instancia.sfxSource.Stop();

        escribiendo = false;
    }

    void MostrarTextoCompleto()
    {
        if (escribirCoroutine != null)
            StopCoroutine(escribirCoroutine);

        textoDialogo.text = lineas[lineaActual];

        SFXManager.instancia.sfxSource.Stop();

        escribiendo = false;
    }

    void SiguienteLinea()
    {
        lineaActual++;

        if (lineaActual < lineas.Length)
        {
            escribirCoroutine = StartCoroutine(EscribirTexto());
        }
        else
        {
            CerrarDialogo();
        }
    }

    void CerrarDialogo()
    {
        dialogoActivo = false;
        escribiendo = false;

        SFXManager.instancia.sfxSource.Stop();

        panelDialogo.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
            CerrarDialogo();
        }
    }
}