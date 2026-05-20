using MoreMountains.Feedbacks;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class BlockHealth : MonoBehaviour
{
    [Header("Dependencies")]
    public CustomBlockSO block;

    [Header("Configuration")]
    public GameObject particles;

    [Header("Broadcasting Events")]
    public IntGameEvent scorer;
    public BoolGameEvent hasTriggeredShoot;

    public UnityEvent onAbilityEvent;
    public UnityEvent onDamageEvent;
    public UnityEvent onDestroyedEvent;
    public UnityEvent onStaticEvent;


    [SerializeField]private int blockhealth;



    private void Start()
    {
        blockhealth = block.health;
    }

    private void Update()
    {
        onStaticEvent.Invoke();
    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            blockhealth--;
            Instantiate(particles, transform.position, Quaternion.identity);
            Debug.Log("Particles Released");
            onDamageEvent.Invoke();
            var ball = other.gameObject.GetComponent<Ball>();
            ball.ballVisuals.PlayFeedbacks();
            if (blockhealth <= 0)
            {
                scorer.Raise(block.scoreBonus);
                onDestroyedEvent.Invoke();
                if(block.addsAbility)
                    onAbilityEvent.Invoke(); //instantiator

                if (block.isShotTrigger)
                    hasTriggeredShoot.Raise(true);

                Destroy(this.gameObject);

            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            blockhealth--;
            Instantiate(particles, transform.position + new Vector3(0.5f, 1.25f, 0), Quaternion.identity);

            if (blockhealth <= 0)
            {
                scorer.Raise(block.scoreBonus);

                if (block.addsAbility)
                    onAbilityEvent.Invoke(); //instantiator

                Destroy(this.gameObject);

            }
        }
    }





    //ability so:
    //name, description, sprite, ABILITY: add
}
