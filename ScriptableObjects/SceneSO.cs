using UnityEngine;

[CreateAssetMenu(fileName = "newScene", menuName = "Scriptable Objects/Scene")]
public class SceneSO : ScriptableObject
{
    [Header("Scene Information")]
    public string sceneName;
}
