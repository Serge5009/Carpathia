using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    GameObject[] seePoints;
    GameObject eyes;
    public float viewDistance = 20.0f;
    public float hearDistance = 5.0f;
    public float viewAngle = 110.0f;

    public bool CanSee(GameObject otherNPC)
    {
        Vector3 direction = otherNPC.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        if(Vector3.Distance(this.transform.position, otherNPC.transform.position) < hearDistance)
            return true;    //  If too close
        if (direction.magnitude < viewDistance && angle < viewAngle)
            return true;    //  If can see

        return false;
    }

    public bool CanBeSeen(GameObject OtherNPC)
    {
        return false;
    }
}
