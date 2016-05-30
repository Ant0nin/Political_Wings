using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;
    public Camera mainCamera;
    public string nextSceneName;

    private bool sceneStarting = true;
    private GUITexture screenTexture;
    private bool goToNextLevel = false;

    void Awake()
    {
        screenTexture = GetComponent<GUITexture>();
        screenTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }


    void Update()
    {
        if (sceneStarting)
            StartScene();

        // TODO : debug à retirer
        if (Input.GetMouseButtonDown(0))
            goToNextLevel = true;
        if (goToNextLevel)
            EndScene();
        // ----------------------
    }


    void FadeToClear()
    {
        screenTexture.color = Color.Lerp(screenTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToWhite()
    {
        screenTexture.color = Color.Lerp(screenTexture.color, Color.white, fadeSpeed * Time.deltaTime);
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


    public void EndScene()
    {
        screenTexture.enabled = true;
        FadeToWhite();

        if (screenTexture.color.a >= 0.95f)
            SceneManager.LoadScene(nextSceneName);
    }
}
