
using UnityEngine;

public class LevelEntrance : MonoBehaviour
{
    public LevelEntranceSO entrance;

    //Level entrances are basically just empty gameobjects and we use their transform to define the particular level entrance
    //Can add parameters to the LevelEntranceSO if we so desire - name etc, doesn't have to be empty but the only important thing for this loading system is their transform
    //This script is attached to the entrance and a the SO will define the entrance transform as the entry point
}
