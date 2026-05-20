using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public void SetTime(float scale)
    { 
        Time.timeScale = scale;
    
    }

    public void SetDeltaTimeScale(float scale)
    {
       Time.timeScale = scale;
       Time.timeScale = Time.unscaledDeltaTime;
        //not sure if this will worik!!! :)
    }

    public void PauseUpdateCanvas()
    {
        Canvas.ForceUpdateCanvases();
        Debug.Log("BUTTON CLICKED");
    }
}
