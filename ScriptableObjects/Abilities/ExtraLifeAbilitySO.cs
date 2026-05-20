using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraLifeAbility", menuName = "Scriptable Objects/Abilities/Extra Life Ability")]
public class ExtraLifeAbilitySO : AbilitySO
{
    public int livesToAdd = 1;
    public AudioClip extraLifeSound; //could be MMfeedback


    private void OnEnable()
    {
        abilityType = AbilityType.ExtraLife;
        abilityMode = AbilityMode.Triggered;
    }


    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        Debug.Log($"Extra Life granted: +{livesToAdd} lives!");

        // Add lives
        //GameSession.instance.AddLives(livesToAdd);
        batty.GainExtraLife(livesToAdd);
        

        // Optional audio/visual feedback
        if (extraLifeSound)
            AudioSource.PlayClipAtPoint(extraLifeSound, batty.transform.position);

        yield return null;
    }
}