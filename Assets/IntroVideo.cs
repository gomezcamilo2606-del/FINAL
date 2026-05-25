using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int siguienteEscena;

    void Start()
    {
        videoPlayer.loopPointReached += FinVideo;
    }

    void FinVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(siguienteEscena);
    }
}