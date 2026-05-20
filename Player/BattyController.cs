using MoreMountains.Feedbacks;
using Playgama;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class BattyController : MonoBehaviour
{

    [Header("References")]
  
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CapsuleCollider2D _collider;
   // [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private Transform[] _ballSpots;
    [SerializeField] private Image _extraLifeImage; //this should be array of images?


    [Header("Configuration")]
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _bonusTime = 5;
    [SerializeField] private Vector2 _launchDirection = new Vector2(2, 4);
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _textPrefab;
    [SerializeField] private GameObject _bonusParticle;

    [Header("Shooters (optional)")]
    [SerializeField] private GameObject _shooterObj1;
    [SerializeField] private GameObject _shooterObj2;
    [SerializeField] private Transform _firePoint1;
    [SerializeField] private Transform _firePoint2;
    [SerializeField] private Shooter _shooter1;
    [SerializeField] private Shooter _shooter2;


    [Header("MM Feedbacks")]
    public MMFeedbacks ballHit;
    public MMFeedbacks shooterFire;
    public MMFeedbacks bonusStateNoise;    
    public MMFeedbacks battyStateFeedback;

    public Transform CatcherSpot => _ballSpots[0];
    public float BonusTime => _bonusTime;
    public GameObject BallPrefab => _ballPrefab;
    public GameObject ShooterObj1 => _shooterObj1;
    public GameObject ShooterObj2 => _shooterObj2;
    public Shooter Shooter1 => _shooter1;
    public Shooter Shooter2 => _shooter2;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public CapsuleCollider2D Collider => _collider;
    public AbilitySO CurrentAbility => _currentAbility;
    public Transform[] BallSpots => _ballSpots;
    public float Speed => _speed;
    public Vector2 LaunchDirection => _launchDirection;
   // public Transform BallSpawnPoint => _ballSpawnPoint;

    [Header("Events")]
    public IntGameEvent bonusScore;
    public IntGameEvent extraLives;
    public UnityEvent onEndLevelFlyer;

    // Private state
    private Rigidbody2D _rb;
    private float _horizontalInput;
    private Vector3 _ballOffset;
    private AbilitySO _currentAbility;
    private AbilityManager _abilityManager;
    public bool catcherActive = false; // Catcher is currently holding a ball
    public bool catcherArmed = false;   // Ability is active, waiting for next ball hit
   
    //private Movement _mover;

    //private Vector3 ballOffset;
    //private AbilitySO _ability;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shooterObj1.SetActive(false);
        _shooterObj2.SetActive(false);
       // _mover = GetComponent<Movement>();
        
    }

    private void Start()
    {
         // Cache initial ball offset
        Ball ball = GetComponentInChildren<Ball>();
        if (ball != null)
            _ballOffset = ball.transform.position - transform.position;

        // Reset extra life UI
        var c = _extraLifeImage.color;
        c.a = 1f;
        _extraLifeImage.color = c;
        battyStateFeedback?.PlayFeedbacks();
        GameSession.instance.HasBeenShot = false;

    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _rb.linearVelocity.y);
        

    }

    private void Update()
    {
        _bonusTime = GameSession.instance._bonusTime;
        Debug.LogFormat("Bonus time = {0} and Game session bonus time = {1}", _bonusTime, GameSession.instance._bonusTime);
    }


    public void ClearAbility()
    {
        _currentAbility = null;
        Debug.Log("Ability Cleared");

    }

    public void ActivateAbility(AbilitySO ability) //call from AbilitySOEvent listener
    {
        _currentAbility = ability; 
        AbilityManager.instance.TryActivateAbility(this, ability);
        OnPowerUpCollect(ability.abilityName);
    }

    void OnPowerUpCollect(string name)
    {
        // Spawn the text at the player's current position + your reference vector
        GameObject popup = Instantiate(_textPrefab, transform.position + new Vector3(0.5f, 1.25f, 0), Quaternion.identity);
        
        // Set the text content
        popup.GetComponentInChildren<TMP_Text>().text = name + "!";
        Instantiate(_bonusParticle, this.transform.position, Quaternion.identity);
    }


    public void SetBonusTime(float time)
    {
        _bonusTime = time;
        // Optional: clamp, animate, log, etc.
        // e.g. HUDManager.Instance.UpdateBonusBar(_bonusTime);
    }


    public void OnMovement(InputAction.CallbackContext value)
    {
        _horizontalInput = value.ReadValue<Vector2>().x;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Ball"))
        {
            var ball = other.gameObject.GetComponent<Ball>();
            ball?.ballVisuals.PlayFeedbacks();
            ballHit?.PlayFeedbacks();
        }

    }

    public void OnLaunch(InputAction.CallbackContext launch)
    {

        if (!launch.performed) return;

        if (launch.performed)
        {
            if (transform.childCount > 0)
            {
                Ball ball = GetComponentInChildren<Ball>();
                if (ball != null)
                {
                    ball.Launch(_launchDirection);
                }

            }

        }

    }

    public void OnShoot(InputAction.CallbackContext fire)
    {
        if (fire.performed)
        {
            
           AbilityManager.instance.OnPlayerShoot();
        
        }
    }

    public void OnBallReset(InputAction.CallbackContext reset)
    {
        if (reset.performed)
        {
            ResetBall();
        }

    }

    public void ResetBall()
    {
        Ball ball = Instantiate(_ballPrefab).GetComponent<Ball>();
        ball.transform.SetParent(transform);
        ball.transform.position = transform.position + _ballOffset;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Catcher handling: delegate to manager or ability

        //for now only Ball is using trigger
        if (!other.CompareTag("Ball")) return;

        // If ability isn't armed OR catcher already active, do nothing
        if (!catcherArmed || catcherActive) {

            Debug.LogFormat("Either CatcherArmed is false {0}, or catcherActive is on {1}", catcherArmed, catcherActive);
            return; }

        Ball ball = other.GetComponent<Ball>();
        if (ball == null)
        {
            Debug.Log("Ball component is null!!");
            return;
        }
        Debug.Log("CATCHER: Ball hit Batty — catching now!");

        catcherActive = true;
        catcherArmed = false;  // disarm immediately

        float time = _currentAbility.duration > 0 ?
                 _currentAbility.duration :
                 _bonusTime;

        ball.StartCatcher(
       transform,
       _launchDirection,
       time
   );


    }

    public void GainExtraLife(int count)
    {
       // GameSession.instance.AddLives(count);
        extraLives?.Raise(count);
        GameSession.instance.UpdateUIDisplay();
    }



    public void AddScore(int points)
    {
        GameSession.instance.AddScore(points);
        bonusScore?.Raise(points);
    }


    public void EndLevelFlyer()
    {
        if (_currentAbility != null && _currentAbility.abilityType == AbilityType.Flying)
        {
            var sprite = _currentAbility.transformation;
            _spriteRenderer.sprite = sprite;
            onEndLevelFlyer.Invoke(); //turn on fx
            ClearAbility();
        }
    }




/*    public void AddBalls()
    {
        //add balls to controller
        //add 1, 2 or 3

        if (_ability != null && _ability.hasBalls)
        {
            for (int i = 0; i < _ability.ability._balls.Length; i++)
            {
                var ball = Instantiate(_ability.ability._balls[i]).GetComponent<Ball>();
                ball.transform.parent = this.transform;
                ball.transform.position = ballSpots[i].position;
                Debug.Log("Balls checked and added");
                ball.isBonusBall = true;

            }
            _ability = null;

        }
        else return;

    }


    public void AddShooters()
    {
        if (_ability != null && _ability.isShooter)
        {
            StartCoroutine(ShooterTime());
        }
        else return;
    }

    private IEnumerator ShooterTime()
    {
        _shooterObj.SetActive(true);
        _shooterObj2.SetActive(true);
        while (!_shooterObj.activeSelf && !_shooterObj2.activeSelf)
        { 
            yield return null;
        }

        _ability.ability.shooters[0] = _shooter1;
        _ability.ability.shooters[1] = _shooter2;
        _bonusTime = GameSession.instance._bonusTime;
        Debug.Log("Bonus time is now: " + _bonusTime);
        yield return new WaitForSeconds(_bonusTime);
        _shooterObj.SetActive(false);
        _shooterObj2.SetActive(false);
        _ability = null;
    }

    public void ActivateExtender()
    {
        if (_ability != null && _ability.isExtended)
        {
            StartCoroutine(ExtenderTime());
            
        }
        else return;
    }

    private IEnumerator ExtenderTime()
    {
        var originalColliderSize = GetComponentInChildren<CapsuleCollider2D>().size;
        var sprite = _ability.ability.transformation;
        var collider = GetComponentInChildren<CapsuleCollider2D>();
        Debug.Log("Gotten sprite");
        GetComponent<SpriteRenderer>().sprite = sprite;
        Debug.Log("Changed sprite renderer");
        collider.size = new Vector2(collider.size.x + 2.1f, collider.size.y - 0.4f);
        //change local scale of collider
        //
        _bonusTime = GameSession.instance._bonusTime;
        yield return new WaitForSeconds(_bonusTime);
        collider.size = originalColliderSize;
        //change local scale of collider 
        sprite = null;
        GetComponent<SpriteRenderer>().sprite = sprite;
        yield return null;
        _ability = null;
        // GetComponentInChildren<SpriteRenderer>().sprite = _originalModel;
        //need to extend COLLIDER not just sprite!!
    }


    public void AddScoreBonus()
    {
        if (_ability != null && _ability.isScoreBonus)
        {    bonusScore.Raise(_ability.scoreBonus);    //raise int event to add to score
            _ability = null; 
        }
        else return;
    }


    public void AddExtraLife()
    {
        if (_ability != null && _ability.isExtraLife)
        //raise int event to add to lives
        {
            extraLives.Raise(_ability.extraLives);
            GameSession.instance.ExtraLivesIconController(_extraLifeImage);
            _ability = null;
        }

        else return;
    }

    public void SlowBall()
    {
        if (_ability != null && _ability.isSlowBall)
        {
            StartCoroutine(BallSlower());
            
        }
        else return;
    }

    private IEnumerator BallSlower()
    {
        Ball ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        while (!ball)
        {
            yield return null;
        }
        var ogSpeed = ball.Speed;
        Debug.Log("Original Speed =  " + ogSpeed);
        ball.SlowBall(2);
        _bonusTime = GameSession.instance._bonusTime;
        yield return new WaitForSeconds(_bonusTime);
        ball.Speed = ogSpeed;
        Debug.Log("Ball Speed should now be original speed again = " + ball.Speed);
        _ability = null;
    }

    public void CatchBall()
    { 
        //start coroutine that calls ball's catch ball function
        if(_ability != null && _ability.isCatcher)
        StartCoroutine(BallCatcher());
    
    }

    private IEnumerator BallCatcher()
    {
        Ball ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        while (!ball)
        {
            yield return null;
        }
        ball.CatchBall(this.transform);
        _bonusTime = GameSession.instance._bonusTime;
        yield return new WaitForSeconds(_bonusTime);
        ball.ReleaseBallFromCatch(_launchDirection);
        _ability = null;

    }

    private IEnumerator StartCatcher(Ball ball)
    { 
        while(!ball) { yield return null; }
        ball.CatchBall(this.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();
            if (ball == null) return;

            Debug.Log("Batty triggered ball collision!");

            // Notify the AbilityManager instead of handling everything here
            AbilityManager.Instance?.TryActivateCatcher(this, ball);
        }


        /*Debug.Log("Batty Controller OnTriggerEnterered");
        if (_ability != null && _ability.isCatcher)
        {
            var ball = other.gameObject.GetComponent<Ball>();
            StartCoroutine(StartCatcher(ball));
            
            if (other.gameObject.CompareTag("Ball") && ball.CatchBallOn())
            {
                if (ball.CatchBallOn())
                {
                    Debug.Log("CatchBallOn() is on!");
                    ball.RbSimulatedOff();
                    StartCoroutine(BallCatchTime(ball));
                    // is this point we want to switch off ball rb.simulated
                    // turn off catch ball after time us up
                    _ability = null;
                    Debug.Log("Ability should be null  " + _ability);

                }
                else return;

            } 
        }
        
        else return;
    }

    private IEnumerator BallCatchTime(Ball ball)
    {
        _bonusTime = GameSession.instance._bonusTime;
        yield return new WaitForSeconds(_bonusTime);
        ball.CatchBallOff();
        
        if (ball != null)
        ball.ReleaseBallFromCatch(_launchDirection);
    
    }
    public void EndLevelFlyer()
    {
        if (_ability != null && _ability.isFlying == true)
        {
            var sprite = _ability.ability.transformation;
            Debug.Log("Gotten sprite");
            GetComponent<SpriteRenderer>().sprite = sprite;
            Debug.Log("Changed sprite renderer");
            //set animation for rockets on
            // turn player input off
            onEndLevelFlyer.Invoke();
            //make this into coroutine and then wait until it reaches top of screen
            //then it will trigger the scene loader manager
            _ability = null;
        }
        else return;
    }

    //needs listener for abilities
    public void ActivateAbility(AbilitySO newAbility)
    {
        _ability = newAbility;

        AddBalls();
        AddShooters();
        AddScoreBonus();
        EndLevelFlyer(); //replace this with coroutine eventually
        ActivateExtender();
        SlowBall();
        AddExtraLife();
        Debug.Log("Ability added  " + _ability);

    }


    public void DestroyAbility(bool nulled)
    {
        if (nulled)
        {
            if (_ability != null)
            {
                var ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
                
                if (_ability.hasBalls)
                {
                    _ability.ability._balls = null;
                    _ability = null;

                }

                if (_ability.isSlowBall)
                {
                    
                    if (ball != null)
                        ball.Speed = 5;
                        _ability = null;
                }

                if (_ability.isCatcher)
                {
                    if (ball != null)
                    {
                        ball.CatchBallOff();
                        _ability = null;
                    
                    }
                
                }
            
            }

        }
    
    }*/
}
