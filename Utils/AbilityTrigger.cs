using ScriptableObjectArchitecture;
using UnityEngine;

public class AbilityTrigger : MonoBehaviour
{
    [Header("Configuration")]
    public AbilitySO ability;

    [Header("Broadcasting Events")]
    public AbilitySOGameEvent abilityTriggered;

    public void TriggerAbility()
    { 
        abilityTriggered.Raise(ability);
        Debug.Log("Ability raised  " + ability);
    }
}
