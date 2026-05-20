using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class BallEvent : UnityEvent<Ball> { }

	[CreateAssetMenu(
	    fileName = "BallVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Ball Event",
	    order = 120)]
	public class BallVariable : BaseVariable<Ball, BallEvent>
	{
	}
}