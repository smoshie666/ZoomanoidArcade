using System.Collections;
using UnityEngine;
using Playgama;
using Playgama.Modules.Platform;

public class PlaygamaReadySignal : MonoBehaviour
{
    private IEnumerator Start()
    {
        // wait one frame so UI/canvas initializes
        yield return null;

#if UNITY_WEBGL && !UNITY_EDITOR
        Bridge.platform.SendMessage(PlatformMessage.GameReady);
        Debug.Log("Playgama GameReady sent!");
#endif
        
    }
}
