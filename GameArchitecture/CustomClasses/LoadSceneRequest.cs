
[System.Serializable]
public class LoadSceneRequest
{
    public SceneSO scene;
    public bool loadingscreen;

    public LoadSceneRequest(SceneSO scene, bool loadingscreen)
    { 
        this.scene = scene;
        this.loadingscreen = loadingscreen;
    
    }
}
