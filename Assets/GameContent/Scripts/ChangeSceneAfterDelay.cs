using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterDelay : MonoBehaviour {

    public float fadeSpeed = 1.5f;
    public string nextSceneName = "main_scene";
    public float delay = 3f;
    public Color fadeOutColor = Color.white;

    private GUITexture screenTexture;
    private bool goToTargetScene;


    void Awake()
    {
        screenTexture = GetComponent<GUITexture>();
        screenTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
        screenTexture.color = fadeOutColor;
    }

    void Update()
    {
        if (goToTargetScene)
            NextScene();
    }

    void Start () {
        StartCoroutine(GoToNextScene());
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

    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(delay);
        goToTargetScene = true;
    }
}
