using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour {

    public float fadeSpeed = 1.5f;
    public string nextSceneName;
    public Color fadeOutColor = Color.white;

    private GUITexture screenTexture;
    private bool goToTargetScene;

    void Awake()
    {
        screenTexture = GetComponent<GUITexture>();
        screenTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
        screenTexture.color = fadeOutColor;
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            goToTargetScene = true;
        }
    }

    void Update()
    {
        if (goToTargetScene)
            NextScene();
    }

    void FadeToColor()
    {
        screenTexture.color = Color.Lerp(screenTexture.color, fadeOutColor, fadeSpeed * Time.deltaTime);
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
        goToTargetScene = true;
    }
}
