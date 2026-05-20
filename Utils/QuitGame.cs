using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quitgame()
    {
        if (Application.isEditor)
        {

            Debug.Log("Cannot Quit Game from Unity!");

        }

        else
        {
            Application.Quit();

        }

    }

}
