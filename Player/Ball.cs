using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    public MMFeedbacks ballVisuals;
    public MMFeedbacks ballHitNoise;
    public bool isBonusBall = false;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private bool _catchBallOn = false;
    private Coroutine _catchRoutine;

    public Rigidbody2D Rb { get { return _rb; } }

    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Max(0f, value); // no negative or zero speeds
    
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
    }

    private void Update()
    {
        if (_rb.simulated && !_catchBallOn)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * _speed;
        }
 
        ChangeBonusBallColour();
    }

    
    public void Launch(Vector2 direction)
    {
        transform.parent = null;
        _rb.simulated = true;
        _rb.linearVelocity = direction.normalized * _speed;
    
    }

    public void CatchBall(Transform parent)
    {
        _catchBallOn = true;

        transform.parent = parent;
        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;
    }

    public void RbSimulatedOff()
    {
        _rb.simulated = false;
    }

    public void RbSimulatedOn()
    {
        _rb.simulated = true;
    }

    public void ReleaseBallFromCatch(Vector2 direction)
    {
        _catchBallOn = false;
        if (transform.parent != null)
        {
            transform.parent = null;
            _rb.simulated = true;
            Launch(direction);
        }
    
    }

    public void ChangeBonusBallColour()
    {
        _spriteRenderer.color = isBonusBall ? Color.green : Color.white;

    }

    public void SlowBall(float speed)
    {
       _speed /= speed;
        Debug.Log("Speed is currently: " + _speed);
    }

    public void StartCatcher(Transform parent, Vector2 launchDirection, float duration)
    {
        // stop any previous catch coroutine
        if (_catchRoutine != null)
            StopCoroutine(_catchRoutine);

        _catchRoutine = StartCoroutine(CatcherRoutine(parent, launchDirection, duration));

    }

    private IEnumerator CatcherRoutine(Transform parent, Vector2 launchDirection, float duration)
    {
        // Wait until ball is valid & caught
        while (this == null)
            yield return null;

        yield return null; // Wait one frame - ensures batty is positioned

        CatchBall(parent.GetComponent<BattyController>().CatcherSpot);
        Debug.LogFormat("Ball caught — starting catcher mode! Duration is {0}", duration);

        yield return new WaitForSeconds(duration);

        if (this != null)
        {
            Debug.Log("Releasing ball after bonus time!");
            ReleaseBallFromCatch(launchDirection);

            if (parent != null)
            {
                var batty = parent.GetComponent<BattyController>();
                if (batty != null)
                {
                    batty.catcherActive = false;
                    batty.catcherArmed = false;
                }
            }
        }

        _catchBallOn = false;
        _catchRoutine = null;
    }

 

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Wall")
        {
            ballVisuals?.PlayFeedbacks();
            ballHitNoise?.PlayFeedbacks();

        }

    }

    /*  public bool CatchBallOn()
   {
       _catchBallOn = true;
       return _catchBallOn;
   } // this doesn't work!!

   public bool CatchBallOff()
   {
       _catchBallOn = false;
       return _catchBallOn;

   } */

}
