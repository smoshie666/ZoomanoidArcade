using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public BoolGameEvent triggerEndOfLevel; //trigger end of level sequence in GameSession


    public UnityEvent onBlocksCompleted; //sceneloader new level
                                            

    //called by bool listener
    public void EndLevel(bool blocksGone)
    {
        if (blocksGone)
        {
            triggerEndOfLevel.Raise(true);
            //need to wait for end of level UI screen
            onBlocksCompleted.Invoke();
        }
    }

    //coroutine to delay sceneloader
}
