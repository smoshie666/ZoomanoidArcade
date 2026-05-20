using UnityEngine;
using System.Collections;

public class IntroEnemyController : MonoBehaviour
{
    public float waitTime;
    public float moveSpeed;
    public Transform newPosition;
    
    private bool canMoveCruiser = false;


    private void Start()
    {
        StartCoroutine(WaitForTextScroll());
    }
    private void Update()
    {
        if (canMoveCruiser)
           transform.position = Vector3.MoveTowards(transform.position, newPosition.position, 3.5f);

        if (transform.position == newPosition.position)
            canMoveCruiser = false;
    }

    private IEnumerator WaitForTextScroll()
    {
        yield return new WaitForSeconds(waitTime);
        canMoveCruiser = true;
    }
}
