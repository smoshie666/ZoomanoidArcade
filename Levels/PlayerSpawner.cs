using System.Diagnostics.Contracts;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerPathSO playerPath;
    public GameObject playerPrefab;
    public CinemachineCamera virtualCamera;
    public GameObject playerParent;

    public void InstatiatePlayerOnLevel()
    {
        Debug.Log("Instatiate player called");
        GameObject player = GetPlayer();
        Transform entrance = GetLevelEntrance(playerPath.levelEntrance);

        player.transform.position = entrance.transform.position;
        if (playerParent != null)
        { player.transform.parent = playerParent.transform; }

        if (virtualCamera.Follow == null)
        this.virtualCamera.Follow = player.transform;

        //once player instatiated, clear the player path for next time
        Debug.Log("Instatiated player");

        playerPath.levelEntrance = null;
        Debug.Log("Reset Path");

    }


    private GameObject GetPlayer()
    { 
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        { 
           playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        return playerObject;
    }

    private Transform GetLevelEntrance(LevelEntranceSO playerEntrance)
    {
        if (playerEntrance == null)
        {
            return this.transform.GetChild(0).transform;
        }

        var levelEntrances = FindObjectsByType<LevelEntrance>(FindObjectsSortMode.None);

        foreach (var levelentrance in levelEntrances)
        {
            if (levelentrance.entrance == playerEntrance)
            { 
                return levelentrance.gameObject.transform;
            }
            //Level entrances are basically just empty gameobjects and we use their transform to define the particular level entrance
            //Can add parameters to the LevelEntranceSO if we so desire - name etc, doesn't have to be empty but the only important thing for this loading system is their transform
        }

        return this.transform.GetChild(0).transform;

    }
}
