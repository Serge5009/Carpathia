using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCState : MonoBehaviour
{
    public GameObject debugSphere;
    NavMeshAgent agent;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
        //(target.transform.position);
        //Flee(target.transform.position);
        //Pursue(target.transform.position);
        //Hide(target.transform.position);
        //Evade(target.transform.position);
        //Wander();
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


    public void Seek(GameObject OtherNPC)   //  Will move towards the destination
    {
        Vector3 destination = OtherNPC.transform.position;

        Debug.DrawRay(transform.position, destination - transform.position, Color.blue);    //  Debug
        agent.SetDestination(destination);
    }
    public void Seek(Vector3 destination)   //  Overloaded
    {
        Debug.DrawRay(transform.position, destination - transform.position, Color.blue);    //  Debug
        agent.SetDestination(destination);
    }


    public void Flee(GameObject OtherNPC)   //  Will move from the destination
    {
        Vector3 destination = OtherNPC.transform.position;

        Vector3 destCoordinates = this.transform.position * 2 - destination;
        Debug.DrawRay(transform.position, destination - transform.position, Color.red);    //  Debug
        Debug.DrawRay(transform.position, destCoordinates - transform.position, Color.blue);    //  Debug
        agent.SetDestination(destCoordinates);
    }
    public void Flee(Vector3 destination)   //  Overloaded
    {
        Vector3 destCoordinates = this.transform.position * 2 - destination;
        Debug.DrawRay(transform.position, destination - transform.position, Color.red);    //  Debug
        Debug.DrawRay(transform.position, destCoordinates - transform.position, Color.blue);    //  Debug
        agent.SetDestination(destCoordinates);
    }

    public void Pursue(GameObject OtherNPC)
    {
        Vector3 destination = OtherNPC.transform.position;


        Vector3 targetDir = destination - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(OtherNPC.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        float targetSpeed;
        if (OtherNPC.tag == "Player")
            targetSpeed = OtherNPC.GetComponent<Movement>().maxSpeed;
        else
            targetSpeed = OtherNPC.GetComponent<NavMeshAgent>().speed;


        if ((toTarget > 90 && relativeHeading < 20) || targetSpeed < 0.01f)    //  <Movement>().maxSpeed has to be changed with actual current speed!!!
        {
            Seek(destination);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + targetSpeed);
        Seek(destination + OtherNPC.transform.forward * lookAhead);
    }

    public void Evade(GameObject OtherNPC)
    {
        Vector3 destination = OtherNPC.transform.position;
        Vector3 targetDir = destination - this.transform.position;

        //float lookAhead = targetDir.magnitude / (agent.speed + OtherNPC.GetComponent<Movement>().maxSpeed);   //  for player
        float lookAhead = targetDir.magnitude / (agent.speed + OtherNPC.GetComponent<NavMeshAgent>().speed);
        Flee(destination + OtherNPC.transform.forward * lookAhead);

    }

    Vector3 wanderTarget = Vector3.zero;
    float wanderTimer = 0.0f;
    float wanderRechargeTime = 1.5f;
    public void Wander()
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

        //Debug.Log(targetGlobal);
        debugSphere.GetComponent<DebugSphere>().TeleportTo(targetGlobal);

        Seek(targetGlobal);
    }

    public void HideTo(GameObject OtherNPC)
    {
        Vector3 destination = OtherNPC.transform.position;

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
        }
        Debug.DrawRay(transform.position, chosenSpot - transform.position, Color.magenta);    //  Debug
        Seek(chosenSpot);

    }

    public void HideBehind(GameObject OtherNPC)
    {
        Vector3 destination = OtherNPC.transform.position;

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

    public bool IsSpotted(GameObject observer)
    {
        Vector3 toAgent = transform.position - observer.transform.position;
        float lookingAngle = Vector3.Angle(observer.transform.forward, toAgent);

        if (lookingAngle < 60)
            return true;
        return false;
    }

    public bool CanSeeTarget(GameObject destination)
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
