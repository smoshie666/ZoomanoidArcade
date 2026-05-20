using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MultiInstatiate : MonoBehaviour
{
    [SerializeField] public List<Instantiator> instantiators;
    [SerializeField] private float _timebetweenInstantiation;
    [SerializeField] protected float _timebeforeInstantiate;

    public int MultiInstantiateCount { get { return instantiators.Count; }  }
    public bool autoStart = false;
    public bool doesLoop = false;

    private void Start()
    {
        if (autoStart)
            Instantiations();
    }


    /* public void InstantiateWithRandomPickUp(int instantiatorIndex, PickUpConfigSO randomPickUp)
     { 
         var instantiator = instantiators[instantiatorIndex];
         var pickup = instantiator.prefab.GetComponent<PickUpController>();
         pickup.pickUp = randomPickUp;

         var renderer = pickup.gameObject.GetComponentInChildren<SpriteRenderer>();
         renderer.sprite = randomPickUp.sprite;

         instantiator.InitiateInstantiate();
         Debug.LogFormat("instantiated instantiator index {0} and added prefab with special {1}", instantiatorIndex, randomPickUp);
     }*/

    public void InstantiateByIndex(int index)
    { 
        var instantiator = instantiators[index];
        instantiator.InitiateInstantiate();
    }

    public void Instantiations()
    { 
        StartCoroutine(InstantiationTiming()); 
    }

    private IEnumerator InstantiationTiming()
    {
        yield return new WaitForSeconds(_timebeforeInstantiate);


        if (doesLoop)
        {
            while (true)
            {
                foreach (var instantiator in instantiators)
                {
                    instantiator.InitiateInstantiate();
                    yield return new WaitForSeconds(_timebetweenInstantiation);
                }  
            }

        }
        else
        {
            foreach (var instantiator in instantiators)
            {
                instantiator.InitiateInstantiate();
                yield return new WaitForSeconds(_timebetweenInstantiation);
            }
        }
    }



}
