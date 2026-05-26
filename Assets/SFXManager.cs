using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instancia;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bossSource;

    [Header("Clips")]
    public AudioClip sonidoMoneda;
    public AudioClip sonidoCura;
    public AudioClip sonidoNPC;
    public AudioClip sonidoSalto;
    public AudioClip sonidoDisparo;
    public AudioClip sonidoDañoJugador;
    public AudioClip sonidoDañoEnemigo;
    public AudioClip sonidoDash;
    public AudioClip musicaBoss;

    [Header("Volúmenes")]
    [Range(0f, 1f)] public float volumenMoneda = 0.3f;
    [Range(0f, 1f)] public float volumenCura = 0.7f;
    [Range(0f, 1f)] public float volumenNPC = 0.5f;
    [Range(0f, 1f)] public float volumenSalto = 0.4f;
    [Range(0f, 1f)] public float volumenDisparo = 0.7f;
    [Range(0f, 1f)] public float volumenDañoJugador = 0.6f;
    [Range(0f, 1f)] public float volumenDañoEnemigo = 0.5f;
    [Range(0f, 1f)] public float volumenDash = 0.5f;
    [Range(0f, 1f)] public float volumenBoss = 0.7f;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SonidoMoneda()
    {
        sfxSource.PlayOneShot(sonidoMoneda, volumenMoneda);
    }

    public void SonidoCura()
    {
        sfxSource.PlayOneShot(sonidoCura, volumenCura);
    }

    public void SonidoNPC()
    {
        sfxSource.PlayOneShot(sonidoNPC, volumenNPC);
    }

    public void SonidoSalto()
    {
        sfxSource.PlayOneShot(sonidoSalto, volumenSalto);
    }

    public void SonidoDisparo()
    {
        sfxSource.PlayOneShot(sonidoDisparo, volumenDisparo);
    }

    public void SonidoDash()
    {
        sfxSource.PlayOneShot(sonidoDash, volumenDash);
    }

    public void SonidoDañoJugador()
    {
        sfxSource.PlayOneShot(sonidoDañoJugador, volumenDañoJugador);
    }

    public void SonidoDañoEnemigo()
    {
        sfxSource.clip = sonidoDañoEnemigo;
        sfxSource.volume = volumenDañoEnemigo;
        sfxSource.Play();

        CancelInvoke(nameof(DetenerSonidoEnemigo));
        Invoke(nameof(DetenerSonidoEnemigo), 1f);
    }

    void DetenerSonidoEnemigo()
    {
        sfxSource.Stop();
    }

    public void IniciarBossMusic()
    {
        if (bossSource.isPlaying)
            return;

        bossSource.clip = musicaBoss;
        bossSource.loop = true;
        bossSource.volume = volumenBoss;
        bossSource.Play();
    }

    public void DetenerBossMusic()
    {
        bossSource.Stop();
    }
}