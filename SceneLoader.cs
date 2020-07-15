using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCg;
    [Range(0.5f, 6.0f)]
    public float fadeDuration = 1.0f;
    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();
    void InitySceneInfo()
    {
        //loadScenes.Add("start", LoadSceneMode.Additive);
        loadScenes.Add("SampleScene", LoadSceneMode.Single);
    }
    IEnumerator Start()
    {
        InitySceneInfo();
        fadeCg.alpha = 1.0f;
        foreach (var _loadScene in loadScenes)
        {
            yield return StartCoroutine(LoadScene(_loadScene.Key, _loadScene.Value));
        }
        StartCoroutine(Fade(0.0f));
    }

    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);
        Scene loadScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(loadScene);
    }
    IEnumerator Fade(float finalAlpha)
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("start"));
        fadeCg.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(fadeCg.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCg.alpha, finalAlpha))
        {
            fadeCg.alpha = Mathf.MoveTowards(fadeCg.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        fadeCg.blocksRaycasts = false;
        SceneManager.UnloadSceneAsync("SceneLoader");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
