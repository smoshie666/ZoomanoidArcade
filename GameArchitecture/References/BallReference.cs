using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class BallReference : BaseReference<Ball, BallVariable>
	{
	    public BallReference() : base() { }
	    public BallReference(Ball value) : base(value) { }
	}
}