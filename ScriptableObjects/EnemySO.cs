using UnityEngine;

[CreateAssetMenu(fileName = "newEnemy", menuName = "Scriptable Objects/Enemy")]
public class EnemySO : ScriptableObject
{
    public float moveSpeed;
    public Sprite sprite;
    public bool isShooter;
    public float fireRate;
    public float initialFireCooldown;
    public int scoreValue;
}
