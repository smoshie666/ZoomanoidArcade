using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "BallGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Ball Event",
	    order = 120)]
	public sealed class BallGameEvent : GameEventBase<Ball>
	{
	}
}