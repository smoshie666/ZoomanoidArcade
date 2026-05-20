using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAbility", menuName = "Scriptable Objects/Ability")]
public abstract class AbilitySO : ScriptableObject
{
    [Header("General")]
    public AbilityType abilityType;
    public AbilityMode abilityMode;
    public string abilityName;

    [Tooltip("How long the ability lasts (seconds). Set to 0 to use Batty’s default BonusTime.")]
    public float duration = 0f;

    [Header("Visuals")]
    public Sprite transformation;

    /// <summary>
    /// Core activation logic for this ability.
    /// </summary>
    public abstract IEnumerator Activate(BattyController batty, AbilityManager manager);

    /// <summary>
    /// Optional: override if your ability needs cleanup.
    /// </summary>
    public virtual void Deactivate(BattyController batty, AbilityManager manager)
    {
        // Default: do nothing
    }

    // Optional event — ShooterAbility overrides this
    public virtual void OnShoot(BattyController batty) { }

    // Optional event — for future abilities (multi-ball triggers, ball effects)
    public virtual void OnBallHit(Ball ball) { }

    /*public void OnShoot()
    {
        if (shooters == null)
            return;
        if (shooters.Count > 0)
        {
            for (int i = 0; i < shooters.Count; i++)
            {
                var shooter = shooters[i];
                shooter.Shoot();
            }
        }
    }*/

}
