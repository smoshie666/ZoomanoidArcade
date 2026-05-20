using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "Scriptable Objects/Game Management/Game Manager")]
public class GameManagerSO : ScriptableObject
{
    public GameStateSO currentState;

    [Header("Broadcasting Events")]
    public GameStateSOGameEvent gameStateChanged;

    private GameStateSO _previousState;

    public void SetGameState(GameStateSO gameState)
    {
        if (currentState != null)
        { 
            _previousState = currentState;
        }
    
        currentState = gameState;

        if (gameStateChanged != null)
        {
            gameStateChanged.Raise(gameState);
        }

    }

    public void RestorePreviousState()
    { 
        SetGameState(_previousState);
    }
}
