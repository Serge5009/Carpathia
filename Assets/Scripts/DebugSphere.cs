using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSphere : MonoBehaviour
{
    public void TeleportTo(Vector3 destination)
    {
        this.transform.position = destination;
    }


}
