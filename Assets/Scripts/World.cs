using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World : MonoBehaviour
{
    private static readonly World _instance = new World();

    private static GameObject[] hidingSpots;

    static World()  //  constructor
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    private World() { }


    public static World Instance
    {
        get { return _instance; }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
