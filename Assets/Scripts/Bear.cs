using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    public GameObject target;
    NPCState states;
    NPCVision vision;

    void Start()
    {
        states = this.GetComponent<NPCState>();
        vision = this.GetComponent<NPCVision>();
    }

    void Update()
    {
        states.Wander();
    }
}
