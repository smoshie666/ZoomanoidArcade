using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "ScoreBonusAbility", menuName = "Scriptable Objects/Abilities/Score Bonus")]
public class ScoreBonusAbilitySO : AbilitySO
{
    public int scoreAmount = 1000;
    public AudioClip bonusSound; //replace with MMFeedback

    private void OnEnable()
    {
        abilityType = AbilityType.ScoreBonus;
        abilityMode = AbilityMode.Triggered;
    }

    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        Debug.Log($"Score Bonus activated: +{scoreAmount} points!");

        // Add score 
        GameSession.instance.AddScore(scoreAmount);

        // Optional: play sound or feedbacks
        if (bonusSound)
            AudioSource.PlayClipAtPoint(bonusSound, batty.transform.position);

        // Optional: small visual effect (e.g., floating text)
        // manager.SpawnEffect(batty.transform.position, "Score+");

        yield return null; // still a coroutine for consistency
    }

}
