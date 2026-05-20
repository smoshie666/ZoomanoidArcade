using System.Collections;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowBallAbilitySO", menuName = "Scriptable Objects/SlowBallAbilitySO")]
public class SlowBallAbilitySO : AbilitySO
{
    [Header("Slow Ball Settings")]
    public float slowdownFactor = 2f;

    private void OnEnable()
    {
        abilityType = AbilityType.SlowBall;
        abilityMode = AbilityMode.Timed;
    }


    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        // Find the ball
        Ball ball = GameObject.FindGameObjectWithTag("Ball")?.GetComponent<Ball>();
        if (ball == null)
        {
            Debug.LogWarning("SLOW BALL: No ball found!");
            yield break;
        }

        // Save original speed
        float originalSpeed = ball.Speed;

        // Choose slowdown
        float factor = slowdownFactor > 0 ? slowdownFactor : 2f;

        // Apply slowdown
        ball.SlowBall(factor);

        // Wait for duration
        float time = duration > 0 ? duration : batty.BonusTime;
        yield return new WaitForSeconds(time);

        // Restore speed
        ball.Speed = originalSpeed;

        Debug.Log("SLOW BALL: Finished!");

        batty.ClearAbility();

    }
}
