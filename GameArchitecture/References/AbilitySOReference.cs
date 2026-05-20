using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class AbilitySOReference : BaseReference<AbilitySO, AbilitySOVariable>
	{
	    public AbilitySOReference() : base() { }
	    public AbilitySOReference(AbilitySO value) : base(value) { }
	}
}