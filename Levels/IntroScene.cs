using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private GameObject _cruiser;
    [SerializeField] private GameObject _zooma;
    [SerializeField] private GameObject[] _enemies;
    public int cameraMoveWaitTime = 45;
    public int waitTime = 5;
    public int battyWaitTime = 5;
    public int explodeWaitTime = 15;
    public int explodeWaitTime2 = 5;

    [SerializeField] private Transform _cruiserFinish;
    [SerializeField] private Transform _zoomaFinish;
    [SerializeField] private Vector3 _newCameraPosition;
    [SerializeField] private Button _skipIntroButton;
    
    public UnityEvent onCruiserPointReacher; //enemies will instantiate and use bezier paths to fly over cruiser
    public UnityEvent onBattyReleaseFinish; //explode ship and destroy
    public UnityEvent onFirstExplosion; //instantiate first explosion and FX
    public UnityEvent onSecondExplosion; //instantiate second explosions, then destroy with delay

    [SerializeField]private bool canMoveCruiser = false;
    [SerializeField] private bool releaseBattyReady = false;
    [SerializeField] private bool cruiserPointReached = false;
    [SerializeField] private bool canCameraMove = false;
    private Camera _mainCam;

    //need UI Button to exit intro and proceed to Level One



    //enemies flyover and instantiate explosions
    //then zooma
    //zooma moves out to -800 on Z axis

    private void Awake()
    {
        _mainCam = Camera.main;
        _skipIntroButton.gameObject.SetActive(false);
    }
    private void Start()
    {
        StartCoroutine(WaitForTextScroll());
        StartCoroutine(WaitForTextScrollPlanetCameraMove());
        StartCoroutine(WaitForSkipButton());
    }

    private void Update()
    {

        if (_cruiser != null)
        {
            if (canMoveCruiser)
                _cruiser.transform.position = Vector3.MoveTowards(_cruiser.transform.position, _cruiserFinish.position, 2.5f);

            if (_cruiser.transform.position == _cruiserFinish.position)
            {
                cruiserPointReached = true;
                canMoveCruiser = false;
            }
        }
        if (canCameraMove)
        _mainCam.transform.position = Vector3.MoveTowards(_mainCam.transform.position, _newCameraPosition, 2.5f);

        if (_mainCam.transform.position == _newCameraPosition)
            canCameraMove = false;
    }
    private IEnumerator WaitForTextScroll()
    { 
        yield return new WaitForSeconds(waitTime);
        canMoveCruiser = true;
    }

    private IEnumerator WaitForTextScrollPlanetCameraMove()
    {
        yield return new WaitForSeconds(cameraMoveWaitTime);
        canCameraMove = true;
    }

    private IEnumerator WaitForSkipButton()
    {
        yield return new WaitForSeconds(5f);
        _skipIntroButton.gameObject.SetActive(true);
    }

    public void CruiserPointReached(bool explosions)      //called from bool listener
    {

        Debug.Log("Cruiser point reached!!");
        if (explosions)
        { 
            onCruiserPointReacher?.Invoke();
            StartCoroutine(WaitForBatty());
        }

        StartCoroutine(CruiserExplode());

    }


    private IEnumerator WaitForBatty()
    {
        yield return new WaitForSeconds(battyWaitTime);
        Debug.Log("Batty wait time");
        releaseBattyReady = true;
        Debug.Log("releaseBattyReady = " + releaseBattyReady);
        onBattyReleaseFinish?.Invoke(); //instantiate batty particles/ball, camera move
    }

    private IEnumerator CruiserExplode()
    {
        yield return new WaitForSeconds(explodeWaitTime);
        //first explosions
        onFirstExplosion?.Invoke();
        yield return new WaitForSeconds(explodeWaitTime2);
        onSecondExplosion?.Invoke();
        Debug.Log("CRUISER EXPLODES");
        Destroy(_cruiser, 3f);
        //instantiate explosions
        //cruiser destroyed
        //destroy explosions
    }
}
