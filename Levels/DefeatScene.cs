using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DefeatScene : MonoBehaviour
{

    public UnityEvent onPlanetExplode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlanetExplode());
    }

    private IEnumerator PlanetExplode()
    {
        yield return new WaitForSeconds(5);
        onPlanetExplode?.Invoke();

    }
}
