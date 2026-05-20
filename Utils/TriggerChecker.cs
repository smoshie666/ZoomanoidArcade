using UnityEngine;
using UnityEngine.Events;

public class TriggerChecker : MonoBehaviour
{
    [Header("Extra config")]
    public string[] validTags;
    [SerializeField] private bool _destroyOnUse = false;
    [SerializeField] private bool _destroyOnFinish = false;
    [SerializeField] private bool _destroyOnStay = false;


    [Header("Events")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var tag in validTags)
        {
            if (other.CompareTag(tag))
            {
                if (onTriggerEnter != null)
                {
                    onTriggerEnter.Invoke();
                }
                if (_destroyOnUse)
                {
                    Destroy(this.gameObject);
                }

            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        foreach (var tag in validTags)
        {
            if (other.CompareTag(tag))
            {
                if (onTriggerStay != null)
                {
                    onTriggerStay.Invoke();
                }
                if (_destroyOnStay)
                {
                    Destroy(this.gameObject);
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (var tag in validTags)
        {
            if (other.CompareTag(tag))
            {
                if (onTriggerExit != null)
                { onTriggerExit.Invoke(); }

                if (_destroyOnFinish)
                { Destroy(this.gameObject, 2); }

            }

        }
    }
}
