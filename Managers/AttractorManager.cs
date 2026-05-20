using UnityEngine;
using UnityEngine.EventSystems;


public class AttractorManager : MonoBehaviour
{
    [SerializeField] private GameObject insertCoinButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(insertCoinButton);
    }


}
