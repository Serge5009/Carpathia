using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCState : MonoBehaviour
{

    public GameObject target;
    public GameObject debugSphere;
    NavMeshAgent agent;


    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //(target.transform.position);
        //Flee(target.transform.position);
        //Pursue(target.transform.position);
        //Hide(target.transform.position);
        //Evade(target.transform.position);
        Wander();
        //HideTo(target.transform.position);
        //HideBehind(target.transform.position);



        /*
        if(CanSeeTarget(target))
        {
            Debug.Log("I see you");
        }
        else
        {
            Debug.Log("nope");
        }*/

        /*
        if(!IsSpotted(target))
        {
            Wander();
            Debug.Log("I'm ok...");
        }
        else
        {
            Evade(target.transform.position);
            Debug.Log("Run!");
        }
        Wander();*/


    }


    void Seek(Vector3 destination)   //  Will move towards the destination
    {
        //Vector3 destCoordinates = destination;
        Debug.DrawRay(transform.position, destination, Color.blue);    //  Debug
        agent.SetDestination(destination);
    }

    void Flee(Vector3 destination)   //  Will move from the destination
    {
        Vector3 destCoordinates = this.transform.position * 2 - destination;
        Debug.DrawRay(transform.position, destination, Color.blue);    //  Debug
        agent.SetDestination(destCoordinates);
    }

    void Pursue(Vector3 destination)
    {
        Vector3 targetDir = destination - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20) || target.GetComponent<Movement>().maxSpeed < 0.01f)    //  <Movement>().maxSpeed has to be changed with actual current speed!!!
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Movement>().maxSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade(Vector3 destination)
    {
        Vector3 targetDir = destination - this.transform.position;

        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Movement>().maxSpeed);
        Flee(destination + target.transform.forward * lookAhead);

    }

    Vector3 wanderTarget = Vector3.zero;
    float wanderTimer = 0.0f;
    float wanderRechargeTime = 0.5f;
    void Wander()
    {
        wanderTimer += Time.deltaTime;
        if(wanderTimer < wanderRechargeTime)
        {
            return;
        }

        wanderTimer = 0;
        float wanderRadius = 10.0f;
        float wanderJitter = 10.0f;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,   //x
                                    0,                                          //y
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);  //z
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        Vector3 targetGlobal = this.gameObject.transform.position + wanderTarget;

        Debug.Log(targetGlobal);
        debugSphere.GetComponent<DebugSphere>().TeleportTo(targetGlobal);

        Seek(targetGlobal);
    }

    void HideTo(Vector3 destination)
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++) 
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - destination;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 5;

            if (hideDir.magnitude <= 5.0f)
                continue;   //  Ignore spots that are too close to enemy


            if ( Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
            }
            Seek(chosenSpot);       
        }
    }

    void HideBehind(Vector3 destination)
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - destination;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 5;

            if (hideDir.magnitude <= 5.0f)
                continue;   //  Ignore spots that are too close to enemy

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 100.0f;
        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized * 5);
    }

    bool IsSpotted(GameObject observer)
    {
        Vector3 toAgent = transform.position - observer.transform.position;
        float lookingAngle = Vector3.Angle(observer.transform.forward, toAgent);

        if (lookingAngle < 60)
            return true;
        return false;
    }

    bool CanSeeTarget(GameObject destination)
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = destination.transform.position - transform.position;

        Debug.DrawRay(transform.position, rayToTarget, Color.green);    //  Debug

        if (Physics.Raycast(transform.position, rayToTarget, out raycastInfo))
        {
            if(raycastInfo.transform.gameObject.tag == "Player")
                return true;
        }

        return false;
    }



}
