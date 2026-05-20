using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class LoadingScreenUI : MonoBehaviour
{
    [Header("Broadcasting on Channels")]
    public BoolGameEvent loadingScreenToggled;

    [Header("Private Dependency")]
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    //function to be called in SceneLoaderManager
    public void ToggleScreen(bool toggle)
    {
        if (toggle)
        {

            _animator.SetTrigger("Show");
        }
        else
        {
            _animator.SetTrigger("Hide");
        }

    }
    //events to call at end of show and hide animations
    public void SendLoadingScreenShownEvent()
    {
        loadingScreenToggled.Raise(true);
    
    }

    public void SendLoadingScreenHiddenEvent()
    { 
        loadingScreenToggled.Raise(false); 
    
    }
}
