using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class IntroBattyController : MonoBehaviour
{
    //unparent from cruiser
    //move out and up to new vector3
    //any particle or other FX
    //camera move and start first level

    public Transform cruiserParent;
    [SerializeField] private Vector3 _battyDestination;
    [SerializeField] private float _waitTime;
    public ParticleSystem[] particleSystems;
    [SerializeField] private Vector3 _newCameraPosition;
    [SerializeField] private float _smoothingCameraSpeed = 5f;

    public UnityEvent onStartLevel;

    private Camera _camera;
    private bool _canMove = false;
    private bool _cameraCanMove = false;
   
    private void Awake()
    {
        _camera = Camera.main;
        
    }


    private void Start()
    {
        

    }

    private void Update()
    {
        Debug.LogFormat("Bools are as follows: canMove = {0}, and cameraCanMove = {1}.", _canMove, _cameraCanMove);
        if (_canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _battyDestination, 5f);
            
            if (transform.position == _battyDestination)
            {
                StartCoroutine(StartNextLevel());
                _canMove = false;
            }
        }

        if (_cameraCanMove)
        {
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _newCameraPosition, _smoothingCameraSpeed * Time.deltaTime);
        }

    }

    public void InitiateBattyMovement(bool begin) //call from listener
    {
        if(begin)
        StartCoroutine(StartBattyMovement());

        if (cruiserParent != null)
            transform.parent = null;
    }

    private IEnumerator StartBattyMovement()
    {
        yield return new WaitForSeconds(_waitTime);
        _canMove = true;
    }

    private IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("Start Next Level coroutine begins");
        onStartLevel?.Invoke();
    }

    public void MoveCameraToBatty() //call from listener
    {
        _cameraCanMove = true;

    }

}
