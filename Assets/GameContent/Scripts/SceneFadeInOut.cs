using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
    public Camera mainCamera;
    public string nextSceneName;

    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private GUITexture screenTexture;

    void Awake()
    {
        screenTexture = GetComponent<GUITexture>();
        // Set the texture so that it is the the size of the screen and covers it.
        screenTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }


    void Update()
    {
        // If the scene is starting...
        if (sceneStarting)
            // ... call the StartScene function.
            StartScene();
    }


    void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        screenTexture.color = Color.Lerp(screenTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        screenTexture.color = Color.Lerp(screenTexture.color, Color.black, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (screenTexture.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the GUITexture.
            screenTexture.color = Color.clear;
            screenTexture.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }


    public void EndScene()
    {
        // Make sure the texture is enabled.
        screenTexture.enabled = true;

        // Start fading towards black.
        FadeToBlack();

        // If the screen is almost black...
        if (screenTexture.color.a >= 0.95f)
            // ... reload the level.
            SceneManager.LoadScene(nextSceneName);
    }
}
