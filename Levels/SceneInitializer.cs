using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    [Header("Dependencies")]
    public SceneSO[] sceneDependencies;

    [Header("On Scene Ready")]
    public UnityEvent onDependenciesLoaded;

    private void Start()
    {
        StartCoroutine(LoadSceneDependencies());
    }

    private void Update()
    {
        foreach (var scene in sceneDependencies)
        {
            Debug.Log("Scene Dependencies number and name:  " + sceneDependencies.Length + scene.name);
        } 
    
    }
    private IEnumerator LoadSceneDependencies()
    {
        for (int i = 0; i <= sceneDependencies.Length - 1; i++)
        {
            Debug.Log("Initiating Load Scene dependencies" + sceneDependencies[i].name + sceneDependencies[i].sceneName);

            var sceneToLoad = sceneDependencies[i];
            if (SceneManager.GetSceneByName(sceneToLoad.name).isLoaded == false)
            {
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneToLoad.name, LoadSceneMode.Additive);

                while (loadScene.isDone == false)
                {
                    yield return null;
                }
            }
            Debug.Log("Initiated Load Scene dependencies");
        }

        if (onDependenciesLoaded != null)
        {
            Debug.Log("Checking On Dependencies Loaded");
            onDependenciesLoaded.Invoke();
        }

    }

}
