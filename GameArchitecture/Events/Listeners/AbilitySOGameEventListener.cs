using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "AbilitySO")]
	public sealed class AbilitySOGameEventListener : BaseGameEventListener<AbilitySO, AbilitySOGameEvent, AbilitySOUnityEvent>
	{
	}
}