using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCState : MonoBehaviour
{

    public GameObject target;
    NavMeshAgent agent;


    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Seek(target);
        //Flee(target);
        Pursue(target);



    }


    void Seek(GameObject destination)   //  Will move towards the destination
    {
        Vector3 destCoordinates = destination.transform.position;
        agent.SetDestination(destCoordinates);
    }

    void Flee(GameObject destination)   //  Will move from the destination
    {
        Vector3 destCoordinates = this.transform.position * 2 - destination.transform.position;
        agent.SetDestination(destCoordinates);
    }

    void Pursue(GameObject destination)
    {
        Vector3 destCoordinates = destination.transform.position - this.transform.position;
        float speedFactor = destCoordinates.magnitude / (agent.speed + destination.GetComponent<Movement>().GetCurrentSpeed());
        Debug.Log("Speed factor is" + speedFactor);
        agent.SetDestination(destCoordinates + destCoordinates * speedFactor);
    }




}
