using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerStats : MonoBehaviour
{

    public enum DestructionState { 
    
        Alive,
        Destructing,
        Destroyed,
        WaitingContinue    
    }

    [Header("Config")]
    [SerializeField] private int _score;
    [SerializeField] private int _lives;
    [SerializeField] private int _currentLives;
    [SerializeField] private int _invulnerabilityTime = 3;
    [SerializeField] private BoxCollider2D _triggerCollider;

    [Header("Broadcasting Events")]
    /* 
     public IntGameEvent bonusLives;
     public FloatEvent timeScaler;
     public BoolGameEvent hiScoreBonusEvent;*/
    public BoolGameEvent isShot;
    public BoolGameEvent allLivesLost;

    public UnityEvent onLifeLost;
    public UnityEvent onNoLives;
    
    public DestructionState state = DestructionState.Alive;


    private AbilitySO _ability;
    private bool allLivesGone = false;
    [SerializeField] private bool _isInvulnerable;
    [SerializeField] private bool _isDestroyed = false;



    private void Awake()
    {
       // _score = GameSession.score;
        _lives = GameSession.totalLives;

        _isInvulnerable = true;
    }

    private void Start()
    {
        _currentLives = _lives;
        StartCoroutine(TimeInvulnerable());

        if (state != DestructionState.Alive)
        {
            state = DestructionState.Alive;
        }
    }

    private void Update()
    {
        LivesChecker();
        Debug.Log("All Lives Gone Bool is: " + allLivesGone);
        //UpdateScore();
    }


    private IEnumerator TimeInvulnerable()
    {
        StartCoroutine(Invulnerablility());
        yield return new WaitForSeconds (_invulnerabilityTime);
        _isInvulnerable = false;
    }


    private IEnumerator Invulnerablility()
    {
        while (_isInvulnerable)
        {
            _triggerCollider.enabled = false;
            yield return null;
        }

        //sprite should flash
        
        _triggerCollider.enabled = true;

    }


    private void LivesChecker()
    {
        _lives = GameSession.totalLives;
        
        
        if (_lives < _currentLives)  //this ensures no balls reset after 0 lives
        {
            _currentLives = _lives;

            onLifeLost?.Invoke();//this will call Destroy and instatiate methods
            //should turn off ability
            Debug.Log("On Life Lost Event");                    //so obj is destroyed and explosion instantiated
            Debug.Log("LIVES =  " + _currentLives);
        }

        if (_lives > _currentLives)
        { 
            _currentLives = _lives;
        }

        if (_lives <= 0)
        {
            _lives = 0;
            onNoLives?.Invoke(); //destroy etc
        }

        if (_lives <= 0 && !allLivesGone)
        {    
           StartCoroutine(AllLivesGoneWait());
           allLivesGone = true;
        }   
            //so obj is destroyed and explosion instantiated
    }

    private IEnumerator AllLivesGoneWait()
    {
        yield return null;
        allLivesLost.Raise(true); //activate game over screen

    }

    public void ChooseAbility()
    { 
    
    
    }

    public void AbilitySetter(AbilitySO ability)
    {
        if (_ability != null)
        { 
            _ability = ability;
        }
    }

    private void UpdateScore()
    {
      //  _score = GameSession.score;
        Debug.Log("SCORE =  " + _score);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            if (_isDestroyed) return;
            
            //if (state != DestructionState.Alive) return

            _isDestroyed = true;
            state = DestructionState.Destructing;
            //turn off collider
            _triggerCollider.enabled = false;
            isShot.Raise(true);
                        
            onLifeLost?.Invoke();
            state = DestructionState.Destroyed;
        }

    }


    public bool HasLostAllLives()
    { 
        if (_lives <= 0)
            return true;

        else return false;
    }
}
