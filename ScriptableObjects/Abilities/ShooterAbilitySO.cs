using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ShooterAbilitySO", menuName = "Scriptable Objects/ShooterAbilitySO")]
public class ShooterAbilitySO : AbilitySO
{

    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        abilityType = AbilityType.Shooter;
        abilityMode = AbilityMode.Timed;

        batty.ShooterObj1.SetActive(true);
        batty.ShooterObj2.SetActive(true);

        float time = duration > 0 ? duration : batty.BonusTime;

        // Grab shooters from Batty
        batty.Shooter1.enabled = true;
        batty.Shooter2.enabled = true;

        yield return new WaitForSeconds(time);

        if (batty != null)
        {
            batty.ShooterObj1.SetActive(false);
            batty.ShooterObj2.SetActive(false);

            batty.Shooter1.enabled = false;
            batty.Shooter2.enabled = false;
        }
        manager.ClearActiveAbility();

    }

    public override void OnShoot(BattyController batty)
    {
        // Fire from both shooters
        Debug.Log("ShooterAbility: Fire!");

        if (batty.Shooter1 != null) batty.Shooter1.Shoot();
        if (batty.Shooter2 != null) batty.Shooter2.Shoot();
    }



}
