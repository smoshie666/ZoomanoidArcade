using ScriptableObjectArchitecture;
using UnityEngine;

public class CruiserController : MonoBehaviour
{
    [SerializeField] private Transform _cruiserPoint;

    [Header("Broadcasting Events")]
    public BoolGameEvent cruiserPointReached;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
          
        if(other.gameObject.CompareTag("CruiserPoint"))
        {
            if (cruiserPointReached != null)
                cruiserPointReached.Raise(true);

        }
       
    }


}
