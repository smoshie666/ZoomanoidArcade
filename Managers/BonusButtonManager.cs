using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BonusButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject bonusTimeExtenderButton;

    private void OnEnable()
    {
        StartCoroutine(SelectDefault());
    }

    private IEnumerator SelectDefault()
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(bonusTimeExtenderButton);
    }

}
