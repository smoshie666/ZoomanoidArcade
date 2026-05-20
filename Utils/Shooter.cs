
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Configuration")]
    public Transform fireOrigin;

    [Header("Dependencies")]
    public GameObject shotPrefab;


    public void Shoot()
    {
        if (fireOrigin != null) 
        Instantiate(shotPrefab, fireOrigin.position, fireOrigin.rotation, this.transform);
    }
}
