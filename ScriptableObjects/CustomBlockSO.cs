using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomBlock", menuName = "Scriptable Objects/Custom Block")]
public class CustomBlockSO : ScriptableObject
{
    public string blockType;
    public int scoreBonus;
    public int health;
    public bool addsAbility;
    public bool isShotTrigger;
}
