using UnityEngine;
using System.Collections;


public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;
    
    
    
    private Coroutine _activeCoroutine;

    private AbilitySO _activeAbility;

    private BattyController _batty;

    private void Awake()
    {
        instance = this;
    }


    public void TryActivateAbility(BattyController batty, AbilitySO ability) //call this to implement ability
    {
        if (ability == null)
        {
            Debug.LogWarning("No ability assigned!");
            return;
        }
        
        // If there’s an active coroutine, stop it to prevent overlaps
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }

        // Store the reference
        _activeAbility = ability;
        _batty = batty;
       
        // Start the ability coroutine
        _activeCoroutine = StartCoroutine(ActivateAbility(batty, ability));
    }


    public void SetActiveAbility(AbilitySO newAbility)
    {
        _activeAbility = newAbility;
       
    }

    private IEnumerator ActivateAbility(BattyController batty, AbilitySO ability)
    {
        Debug.Log($"Activating ability: {ability.name}");
        yield return StartCoroutine(ability.Activate(batty, this));

        // When done, clear references
        _activeCoroutine = null;
        _activeAbility = null;

    }


    // Optional helper if want to manually stop abilities (e.g., on death or state change)
    public void CancelActiveAbility()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }
        _activeAbility = null;
    }

    public void OnPlayerShoot()
    {
        if (_activeAbility != null)
            _activeAbility?.OnShoot(_batty);
    }

    public void ClearActiveAbility()
    {
        _activeAbility = null;
        _batty = null;
    }

    /*private IEnumerator ShooterTime(BattyController batty, AbilitySO ability)
    {
        batty.ShooterObj.SetActive(true);
        batty.ShooterObj2.SetActive(true);
        while (!batty.ShooterObj.activeSelf && !batty.ShooterObj2.activeSelf)
        {
            yield return null;
        }

        ability.shooters[0] = batty.Shooter1;
        ability.shooters[1] = batty.Shooter2;
        batty.SetBonusTime(GameSession.instance._bonusTime);
        Debug.LogFormat("Bonus time is now: {0}, and the Game Session bonus time is: {1} ", batty.BonusTime, GameSession.instance._bonusTime);
        yield return new WaitForSeconds(batty.BonusTime);
        batty.ShooterObj.SetActive(false);
        batty.ShooterObj2.SetActive(false);
        ability = null;
    }

    private IEnumerator ExtenderTime(BattyController batty, AbilitySO ability)
    {

        var spriteRenderer = batty.SpriteRenderer;
        var collider = batty.Collider;

        // Store original values
        var originalColliderSize = collider.size;
        var originalSprite = spriteRenderer.sprite;

        // Apply transformation sprite and new collider scale
        var sprite = ability.transformation;
        collider.size = new Vector2(collider.size.x + 2.1f, collider.size.y - 0.4f);

        //  var originalColliderSize = GetComponentInChildren<CapsuleCollider2D>().size;

        Debug.Log($"Extender ability active for {batty.BonusTime} seconds");

        yield return new WaitForSeconds(batty.BonusTime);

        // Restore original collider and sprite
        collider.size = originalColliderSize;
        spriteRenderer.sprite = originalSprite;


        batty.ClearAbility();
        Debug.Log("Extender ability ended");

      /*  var collider = GetComponentInChildren<CapsuleCollider2D>();
        Debug.Log("Gotten sprite");
        GetComponent<SpriteRenderer>().sprite = sprite;
        Debug.Log("Changed sprite renderer");
        
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


    private IEnumerator BallSlower(BattyController batty, AbilitySO ability)
    {

        // Wait until the Ball exists in the scene
        Ball ball = GameObject.FindGameObjectWithTag("Ball")?.GetComponent<Ball>();
        yield return new WaitUntil(() => ball != null);


        // Cache original speed
        var ogSpeed = ball.Speed;
        Debug.Log($"[SlowBall] Original Speed = {ogSpeed}");


        // Apply slowdown — can make this a property of the AbilitySO if want variable slowdown
        float slowdownFactor = ability.slowAmount > 0 ? ability.slowAmount : 2f;
        ball.SlowBall(slowdownFactor);

        yield return new WaitForSeconds(batty.BonusTime);

        ball.Speed = ogSpeed;
        Debug.Log("[AbilityManager] SlowBall ended.");

        Debug.Log("Ball Speed should now be original speed again = " + ball.Speed);

        ability = null; //optional?

    }


    private IEnumerator BallCatcher(BattyController batty, AbilitySO ability)
    {

        Ball ball = GameObject.FindGameObjectWithTag("Ball")?.GetComponent<Ball>();
        if (ball == null)
            yield break;

        // Get duration from ability (fallback to GameSession or Batty?)
        float bonusTime = ability != null ? ability.duration : batty.BonusTime; 
        Vector2 launchDir = batty.LaunchDirection;

        ball.StartCatcher(batty.transform, launchDir, bonusTime);

        yield return new WaitForSeconds(bonusTime);

         
        ability = null;

    }

    public void TryActivateCatcher(BattyController batty, Ball ball)
    {
        // Ensure the current ability is a catcher
        if (_activeAbility == null || !_activeAbility.abilityType == AbilityType.Catcher)
            return;

        // Start the unified BallCatcher coroutine
        StartCoroutine(BallCatcher(batty, _activeAbility));

        // Reset ability reference if needed
        _activeAbility = null;
    }

    public void EndLevelFlyer(BattyController batty, AbilitySO ability)
    {
        if (ability != null && ability.abilityType == AbilityType.Flying)
        {
            var sprite = ability.transformation;
            Debug.Log("Gotten sprite");
            batty.GetComponent<SpriteRenderer>().sprite = sprite;
            Debug.Log("Changed sprite renderer");
            //set animation for rockets on
            // turn player input off
            batty.onEndLevelFlyer?.Invoke();
            //reaches top of screen
            //then it will trigger the scene loader manager
            ability = null;
        }
        else return;
    }

    public IEnumerator BallFlyerRoutine(BattyController batty, AbilitySO ability)
    {
        if (ability == null || ability.abilityType != AbilityType.Flying)
            yield break;

        Debug.Log("Starting End of Level Flyer!");

        // Disable normal input via game state
        GameSession.instance.GameManagerSO.SetGameState(GameSession.instance.endOfLevelFlyerState);

        // Swap Batty sprite to the rocket/flying version
        if (ability.transformation != null)
            batty.GetComponent<SpriteRenderer>().sprite = ability.transformation;

        // Trigger any animation or effects
        batty.onEndLevelFlyer?.Invoke();

        // Wait for the flight duration (defined in AbilitySO)
        yield return new WaitForSeconds(ability.duration);

        // Once flight ends, trigger end of level state
        GameSession.instance.GameManagerSO.SetGameState(GameSession.instance.endOfLevelState);

        // Optional: Reset Batty sprite if needed
        batty.ResetSprite();

        Debug.Log("End of Level Flyer complete.");
    }

    public void AddBalls(BattyController batty, AbilitySO ability)
    {
        //add balls to controller
        //add 1, 2 or 3

        if (ability != null && ability.abilityType == AbilityType.MultiBall)
        {
            for (int i = 0; i < ability.extraBalls.Length; i++)
            {
                var ball = Instantiate(ability.extraBalls[i]).GetComponent<Ball>();
                ball.transform.parent = this.transform;
                ball.transform.position = batty.BallSpots[i].position;
                Debug.Log("Balls checked and added");
                ball.isBonusBall = true;

            }
            ability = null;

        }
        else return;

    }


    */

}
