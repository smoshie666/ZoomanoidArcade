using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;

public class WallOfDestruction : MonoBehaviour
{
    [Header("Config")]
    public float waitTime;
   // public BallGameEvent newBallInstantiate;
    public GameObject newBallPrefab;
    public GameManagerSO gameState;

    [Header("Broadcasting Event")]
    public BoolGameEvent isDestroyedToggle;

    private void Update()
    {
        Debug.Log(gameState.currentState);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (!ball.isBonusBall && gameState.currentState.stateName != "EndOfLevelFlyer" && GameSession.instance.HasBeenShot == false)
            {
                isDestroyedToggle.Raise(true);
                StartCoroutine(BallDestructionWaitTime());
                                  
            }

            Destroy(other.gameObject);
                        
        }

    }


    public void CallWaitTime(float waitTime)
    { 
    
    
    }

    private IEnumerator BallDestructionWaitTime()
    { 
        yield return new WaitForSeconds(waitTime);
      //  var newball = Instantiate(newBallPrefab.GetComponent<Ball>());
      //  Debug.LogFormat("new Ball instatiated at position {0}", newball.transform.position);
       // newBallInstantiate.Raise(newball);
        

    }

}
