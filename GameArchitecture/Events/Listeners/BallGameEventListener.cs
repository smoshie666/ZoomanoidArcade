using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Ball")]
	public sealed class BallGameEventListener : BaseGameEventListener<Ball, BallGameEvent, BallUnityEvent>
	{
	}
}