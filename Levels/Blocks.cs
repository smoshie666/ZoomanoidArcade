using MoreMountains.Feedbacks;
using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Blocks : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject blocksParentObjectPrefab;
    public GameObject ghostBlock;
    

    private TimeManager timeManager;

    [Header("Broadcasting Events")]
    public BoolGameEvent allBlocksGone; //trigger end of level sequence in GameSession

    public UnityEvent onBlocksCompleted; //sceneloader new level

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

    private void blockChecker()
    {
        
        if (blocksGone) {
            allBlocksGone.Raise(true);
            
            Debug.Log("All blocks gone raise");
        
           // StartCoroutine(clearedBlocksWaitTime());
            //need to wait for end of level UI screen
            
        }
    
    }

    public void MoveToNextLevel() //this needs to be a button press!
    {
        onBlocksCompleted.Invoke(); 
        Debug.Log("All blocks completed invoked");
        blocksGone = false;
        Debug.Log(blocksGone);
        Instantiate(ghostBlock, blocksParentObjectPrefab.transform);

    }

    IEnumerator clearedBlocksWaitTime()
    {
        yield return new WaitForSeconds(3.5f);
    }

    
    private void OnEnable()
    {
        GameSession.nextLevel += MoveToNextLevel;
    }

    private void OnDisable()
    {
        GameSession.nextLevel -= MoveToNextLevel;
    }

}
