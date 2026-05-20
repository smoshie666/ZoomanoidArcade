using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class AbilitySOEvent : UnityEvent<AbilitySO> { }

	[CreateAssetMenu(
	    fileName = "AbilitySOVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Ability Event",
	    order = 120)]
	public class AbilitySOVariable : BaseVariable<AbilitySO, AbilitySOEvent>
	{
	}
}