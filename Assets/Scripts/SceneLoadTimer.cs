using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoadTimer : MonoBehaviour
{
    public string sceneToLoad;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAndMeasure()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        Debug.Log("Iniciando carga");

        float startTime = Time.realtimeSinceStartup;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneToLoad);

        while (operation.progress < 0.9f)
        {
            Debug.Log("Progress: " + operation.progress);

            yield return null;
        }

        float endTime = Time.realtimeSinceStartup;

        float loadTime = endTime - startTime;

        Debug.Log("Tiempo de carga: " +
                  loadTime.ToString("F2") +
                  " segundos");
    }
}