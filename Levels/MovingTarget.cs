using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Configuration")]
    public Vector3 direction;
    public float speed;

    [Header("Dependencies")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private bool _hasHit;

    private void Start()
    {
        _hasHit = true;
    }

    private void FixedUpdate()
    {
        if (_hasHit) { 
        
            MoveToPointB();
        } else {

            MoveToPointA();
        }
    }

    private void MoveToPointA()
    {
        if (pointA != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
        }

        if (transform.position == pointA.transform.position)
        {
            Debug.Log("Has hit platform point thingy A");
            _hasHit = true;
        }

    }

    private void MoveToPointB()
    {
        if (pointB != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
        }

        if (transform.position == pointB.transform.position)
        {
            Debug.Log("Has hit platform point thingy B");
            _hasHit = false;
        }

    }


}
