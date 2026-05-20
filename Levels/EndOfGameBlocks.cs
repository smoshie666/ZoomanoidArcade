using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EndOfGameBlocks : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject blocksParentObjectPrefab;
    public float waitTimeVictory;

    [Header("Broadcasting Events")]
    public BoolGameEvent endGameTrigger; //trigger end of game screen in GameSession

    public UnityEvent onVictory;

    private int blocks;
    private bool blocksGone = false;

    private void Start()
    {
        blocks = blocksParentObjectPrefab.transform.childCount;
        blocksGone = false;

    }


    private void Update()
    {
        blocks = blocksParentObjectPrefab.transform.childCount;

        if (blocks <= 0)
        {
            if (!blocksGone)
            {
                blocksGone = true;
                blockChecker();
            }

        }
    }


    public void TimeRestore()
    {
        Time.timeScale = 1;
    }

    private void blockChecker()
    {

        if (blocksGone)
        {
            endGameTrigger?.Raise(true); // for pre end titles VICTORY UI overlay

            StartCoroutine(StartEndScreen());
            Debug.Log("End Game raise/invoke");
            
            // StartCoroutine(clearedBlocksWaitTime());
            //need to wait for end of level UI screen

        }

    }


    IEnumerator StartEndScreen()
    {
        yield return new WaitForSecondsRealtime(waitTimeVictory);
        onVictory?.Invoke();

    }


}
