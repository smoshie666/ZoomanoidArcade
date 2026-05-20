using System.Diagnostics.Contracts;
using UnityEngine;
using ScriptableObjectArchitecture;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuration")]
    public SceneSO sceneToLoad;
    public LevelEntranceSO levelEntrance;
    public bool loadingScreen;

    [Header("Player Path")]
    public PlayerPathSO playerPath;

    [Header("Broadcasting Events")]
    public LoadSceneRequestGameEvent loadNewSceneRequest;

    public void LoadScene()
    {
        if (playerPath != null && levelEntrance != null)
        {
            playerPath.levelEntrance = levelEntrance;
        }

        var request = new LoadSceneRequest(sceneToLoad, loadingScreen);

        loadNewSceneRequest.Raise(request);
    }
}
