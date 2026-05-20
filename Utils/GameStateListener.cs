using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

public class GameStateListener : MonoBehaviour
{
    [Header("Listening to Events")]
    public GameStateSOGameEvent gameStateChanged;

    [Header("Enabled/Disabled Shortcuts")]
    public MonoBehaviour[] components;
    public List<GameStateSO> enabledStates;
    public List<GameStateSO> disabledStates;

    [Header("Actions")]
    public UnityEvent OnMainMenuState;
    public UnityEvent OnLoadingState;
    public UnityEvent OnPausedState;
    public UnityEvent OnPlayingState;
    public UnityEvent OnLifeLostState;    
    public UnityEvent OnAllLivesLostState;
    public UnityEvent OnEndOfLevelFlyerState;
    public UnityEvent OnEndOfLevelState;
    public UnityEvent OnHiScoreTableState;
    public UnityEvent OnVictoryState;
    public UnityEvent OnEndOfGameState;
    public UnityEvent OnInsertCoinState;

    private void OnEnable()
    {
        gameStateChanged.AddListener(GameStateChanged);
    }

    private void OnDisable()
    {
        gameStateChanged.RemoveListener(GameStateChanged);
    }

    private void GameStateChanged(GameStateSO newGameState)
    { 
        InvokeShortcuts(newGameState);
        InvokeActions(newGameState);
    }
    private void InvokeShortcuts(GameStateSO newGameState)
    {
        foreach (var component in components)
        {
            if (enabledStates.Contains(newGameState))
            {
                component.enabled = true;
            }

            if (disabledStates.Contains(newGameState))
            {
                component.enabled = false;
            }
        }
    
    }

    private void InvokeActions(GameStateSO newGameState)
    {
        if (newGameState.stateName == "MainMenu" && OnMainMenuState != null)
        {
            OnMainMenuState.Invoke();
        }

        if (newGameState.stateName == "Loading" && OnLoadingState != null)
        {
            OnLoadingState.Invoke();        
        }

        if (newGameState.stateName == "Playing" && OnPlayingState != null)
        { 
            OnPlayingState.Invoke(); 
        
        }

        if (newGameState.stateName == "Paused" && OnPausedState != null)
        { 
            OnPausedState.Invoke();
        }

        if (newGameState.stateName == "LifeLost" && OnLifeLostState != null)
        {
            OnLifeLostState.Invoke();
        }

        if (newGameState.stateName == "AllLivesLost" && OnAllLivesLostState != null)
        {
            OnAllLivesLostState.Invoke();
        }

        if (newGameState.stateName == "EndOfLevelFlyer" && OnEndOfLevelFlyerState != null)
        {
            OnEndOfLevelFlyerState.Invoke();
        }

        if (newGameState.stateName == "EndOfLevel" && OnEndOfLevelState != null)
        {
            OnEndOfLevelState.Invoke();
        }


        if (newGameState.stateName == "HiScoreTable" && OnHiScoreTableState != null)
        {
            OnHiScoreTableState.Invoke();

        }

        if (newGameState.stateName == "Victory" && OnVictoryState != null)
        {
            OnVictoryState.Invoke();

        }

        if (newGameState.stateName == "EndOfGame" && OnEndOfGameState != null)
        {
            OnEndOfGameState.Invoke();

        }

        if (newGameState.stateName == "InsertCoin" && OnInsertCoinState != null)
        {
            OnInsertCoinState.Invoke();

        }
    }

}
