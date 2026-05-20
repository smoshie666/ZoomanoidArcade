using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    [Header("Dependencies")]
    public LoadingScreenUI loadingScreenUI;

    private LoadSceneRequest _pendingRequest;



    //Function to be called from a Listener
    public void OnLoadMenuRequest(LoadSceneRequest request)
    {
        if (isSceneAlreadyLoaded(request.scene) == false)
        { 
            SceneManager.LoadScene(request.scene.sceneName);
        }
    
    }


    //Function to be called from a Listener
    public void OnLoadLevelRequest(LoadSceneRequest request)
    {
        if (isSceneAlreadyLoaded(request.scene))
        {
            ActivateLevel(request);
        }
        else {
            if (request.loadingscreen)
            {
                _pendingRequest = request;
                loadingScreenUI.ToggleScreen(true);
            }
            else {

                StartCoroutine(ProcessLevelLoading(request));
            }
        
        }
    
    }

    //Function to be called from a Listener
    public void OnLoadingSceenToggled(bool toggle)
    {
        if (toggle && _pendingRequest != null)
        {
            StartCoroutine(ProcessLevelLoading(_pendingRequest));
        }
    }
    private bool isSceneAlreadyLoaded(SceneSO scene)
    { 
        Scene loadedScene = SceneManager.GetSceneByName(scene.name);

        if (loadedScene != null && loadedScene.isLoaded)
        {
            return true;
        }
        else
            return false;
    
    }

    private IEnumerator ProcessLevelLoading(LoadSceneRequest request)
    {
        if (request.scene != null)
        {
            Scene currentLoadedLevel = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(currentLoadedLevel);

            AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(request.scene.name, LoadSceneMode.Additive);

            while (!loadSceneProcess.isDone)
            {
                yield return null;  
            }
            //once level is ready, activate it
            ActivateLevel(request);
        }
    }

    private void ActivateLevel(LoadSceneRequest request)
    {
        Scene loadedLevel = SceneManager.GetSceneByName(request.scene.name);
        SceneManager.SetActiveScene(loadedLevel);

        //hide loading screen if there is one

        if (request.loadingscreen)
        { 
            loadingScreenUI.ToggleScreen(false);
        
        }
    
        //clean the loading screen request
        _pendingRequest = null;

    }


}
