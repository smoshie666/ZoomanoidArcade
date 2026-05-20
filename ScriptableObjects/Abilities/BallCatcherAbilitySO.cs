using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BallCatcherAbilitySO", menuName = "Scriptable Objects/BallCatcherAbilitySO")]
public class BallCatcherAbilitySO : AbilitySO
{
    
    private void OnEnable()
    {
        abilityType = AbilityType.Catcher;
        abilityMode = AbilityMode.Timed;
    }

    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        Debug.Log("BALL CATCHER: Activated!");

        // Arm the ability — wait for NEXT ball collision
        batty.catcherArmed = true;


        // How long does the catcher last?
        float time = duration > 0 ? duration : batty.BonusTime;


        yield return new WaitForSeconds(time);

        // If never triggered, disarm it
        batty.catcherArmed = false;

       
        manager.ClearActiveAbility();

    }




}


