using UnityEngine;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour
{
    public UnityEvent playingTrack;

    //could make audioclip event and broadcast the audiclip selected, could make array of audioclips to cycle through

    private void Start()
    {
        PlayTrack();
    }

    public void PlayTrack()
    {
        if (playingTrack != null) 
        playingTrack.Invoke();    
    }
}
