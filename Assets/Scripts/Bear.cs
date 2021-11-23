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
        if(vision.CanSee(target))
        {
            states.Pursue(target);
            Debug.Log("Pursue(target)");
        }
        else
        {
            states.Wander();
            Debug.Log("Wander()");
        }
    }
}
