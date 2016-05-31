using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;
    public Camera mainCamera;
    public string nextSceneName;
    public Color fadeInOutColor = Color.white;

    private bool sceneStarting = true;
    private GUITexture screenTexture;
    private bool goToNextLevel = false;

    void Awake()
    {
        screenTexture = GetComponent<GUITexture>();
        screenTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
        screenTexture.color = fadeInOutColor;
    }


    void Update()
    {
        if (sceneStarting)
            StartScene();

        if (goToNextLevel)
            NextScene();
    }


    void FadeToClear()
    {
        screenTexture.color = Color.Lerp(screenTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToColor()
    {
        screenTexture.color = Color.Lerp(screenTexture.color, fadeInOutColor, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        FadeToClear();

        if (screenTexture.color.a <= 0.05f)
        {
            screenTexture.color = Color.clear;
            screenTexture.enabled = false;
            sceneStarting = false;
        }
    }


    void NextScene()
    {
        screenTexture.enabled = true;
        FadeToColor();

        if (screenTexture.color.a >= 0.95f)
        {
            SceneManager.LoadScene(nextSceneName);
        }
            
    }

    public void GoToNextScene()
    {
        goToNextLevel = true;
    }
}
