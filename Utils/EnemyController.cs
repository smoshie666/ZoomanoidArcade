using MoreMountains.Feedbacks;
using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [Header("Configuration")]
    public EnemySO enemy;
    
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
   
    public UnityEvent onFire;

    [Header("Broadcasting Events")]
    public IntGameEvent scorer;

    [Header("MM Feedbacks")]
    public MMFeedbacks enemyStateFeedbacks;   

    private Movement _mover;
    private Shooter[] _shooters;


    private void Start()
    {
        _mover = GetComponent<Movement>();
        if (_mover != null)
        {
            Debug.Log("Attempting to grab movement component speed variable");
            if (enemy != null)
            {
                _mover.speed = enemy.moveSpeed;
                Debug.Log("mover converted to property of Config");
            }
            else { Debug.Log("Config is NULL"); }

        }
        else { Debug.Log("Mover is NULL"); }

        if (enemy != null)
        {
            if (enemy.sprite != null)
            { _spriteRenderer.sprite = enemy.sprite; }
            enemyStateFeedbacks?.PlayFeedbacks();

        }
    }


    public void Shooter() //call from event listener - block shoot trigger
    {
        if (enemy.isShooter)
        {
            _shooters = GetComponentsInChildren<Shooter>();
            Debug.Log("Shooters Gotten");

            if (_shooters != null && _shooters.Length > 0)
            {
                StartCoroutine(ShootRoutine());
            }
        }
    }

    public void OnDestroyed()
    { 
        
        scorer.Raise(enemy.scoreValue);
    }
    private IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(enemy.initialFireCooldown);
        while (true)
        {
            foreach (var shooter in _shooters)
            {
                shooter.Shoot();
                onFire?.Invoke();
            }
            yield return new WaitForSeconds(enemy.fireRate);
        }

    }

    
}
