using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Respawner : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float waitTime;
    public LevelEntranceSO levelEntrance;

    [Header("Player Path")]
    public PlayerPathSO playerPath;


    public UnityEvent onLifeLost;

    //bool listener from Game Session tto set off instantiate
    public void RespawnPlayer(bool respawning)
    {
        if (respawning)
        {
            StartCoroutine(WaitToSpawn());
            Debug.Log("Respawnyyying");
        }
    }

    private IEnumerator WaitToSpawn()
    {
        playerPath.levelEntrance = levelEntrance;
        yield return null; 
            //new WaitForSeconds(waitTime);
        onLifeLost?.Invoke();
    }
}
