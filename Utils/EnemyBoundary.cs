using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBoundary : MonoBehaviour
{
    [SerializeField] private Boundary _boundary;

    public UnityEvent onDestinationReached;
    // Update is called once per frame
    void Update()
    {
        
        float y = Mathf.Clamp(transform.position.y, _boundary.yMin,_boundary.yMax);

        this.transform.position = new Vector3 (transform.position.x, y);
        DestinationCheck();
        
    }

    public void DestinationCheck()
    { 
        if (transform.position.y == _boundary.yMin)
        {
            if (onDestinationReached != null)
                onDestinationReached.Invoke ();

        }
    }

}
