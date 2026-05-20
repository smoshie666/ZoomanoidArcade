using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiBallAbilitySO", menuName = "Scriptable Objects/Multi Ball Ability")]
public class MultiBallAbilitySO : AbilitySO
{
    [Header("MultiBall Settings")]
    public Ball[] extraBalls;

    private void OnEnable()
    {
        abilityType = AbilityType.MultiBall;
        abilityMode = AbilityMode.Triggered; // one-shot ability
    }

    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        Debug.Log("MULTIBALL: Activated!");

        if (extraBalls == null || extraBalls.Length == 0)
        {
            Debug.LogWarning("MULTIBALL: No extra balls assigned!");
            yield break;
        }

        Transform[] spots = batty.BallSpots;
        if (spots == null || spots.Length == 0)
        {
            Debug.LogWarning("MULTIBALL: Batty has no ball spawn points!");
            yield break;
        }

        // Spawn 1 ball per element in extraBalls
        for (int i = 0; i < extraBalls.Length; i++)
        {
            Ball ball = Instantiate(extraBalls[i]).GetComponent<Ball>();

            // Attach and position
            ball.transform.SetParent(batty.transform);
            ball.transform.position = spots[Mathf.Min(i, spots.Length - 1)].position;

            // Mark as bonus ball
            ball.isBonusBall = true;

            Debug.Log($"MULTIBALL: Spawned bonus ball #{i + 1}");
        }

        // Multiball ends immediately—no duration
        batty.ClearAbility();
        yield break;
    }

    
}
