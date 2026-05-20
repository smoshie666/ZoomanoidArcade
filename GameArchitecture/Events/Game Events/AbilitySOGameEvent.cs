using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "AbilitySOGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Ability Event",
	    order = 120)]
	public sealed class AbilitySOGameEvent : GameEventBase<AbilitySO>
	{
	}
}