using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyerAbility", menuName = "Scriptable Objects/Abilities/Flyer AbilitySO")]
public class FlyerAbilitySO : AbilitySO
{


    private void OnEnable()
    {
        abilityType = AbilityType.Flying;
        abilityMode = AbilityMode.Triggered;
    }


    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
             
        //set animation for rockets on
        // turn player input off
        batty.onEndLevelFlyer.Invoke();
        // Determine duration
        float time = duration > 0 ? duration : batty.BonusTime;
        yield return new WaitForSeconds(time);

        batty.ClearAbility();

       
    }

   


}
