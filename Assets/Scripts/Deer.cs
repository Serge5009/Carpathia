using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  NOTE:
//  This script will be modified in future
//  New parent FSM class will be created
//  
//  All NPC scripts will be inherited from that class
//  and will use overrided callback functions
//

enum State  //  Will be moved to parent FSM class later
{
    WANDER,
    PURSUE,
    EVADE,
    HIDE_TO,
    HIDE_BEHIND,

    NUM_STATES
}

public class Deer : MonoBehaviour
{
    public GameObject target;
    NPCState states;
    NPCVision vision;
    State currentState = State.WANDER;  //  Wander by default
    bool isTransitionRunning = false;

    void Start()
    {
        states = this.GetComponent<NPCState>();
        vision = this.GetComponent<NPCVision>();
    }

    void Update()
    {

        StateTransitions(); //  Check all possible transitions
        ApplyState();       //  Act according to currnet state
    }

    void StateTransitions()
    {
        if (currentState == State.WANDER)   //  Transitions from WANDER
        {
            if (vision.CanSee(target))  //  If target is in range
            {
                currentState = State.EVADE; //  Start running
                Debug.Log("Deer is evading");
            }


        }
        else if (currentState == State.PURSUE)   //  Transitions from PURSUE
        {


        }
        else if (currentState == State.EVADE)   //  Transitions from EVADE
        {
            if (Vector3.Distance(this.transform.position, target.transform.position) > 25.0f && !vision.CanSee(target))   //  If enemy far enough and out of sight
            {
                StartCoroutine(ChangeStateAfterSec(State.WANDER, 5));

            }

        }
        else if (currentState == State.HIDE_TO)   //  Transitions from HIDE_TO
        {


        }
        else if (currentState == State.HIDE_BEHIND)   //  Transitions from HIDE_BEHIND
        {


        }
    }

    void ApplyState()   //  This one should be override of parrent FSM class
    {
        switch (currentState)
        {
            case State.WANDER:
                states.Wander();
                break;

            case State.PURSUE:
                states.Pursue(target);
                break;

            case State.EVADE:
                states.Evade(target);
                break;

            case State.HIDE_TO:
                states.HideTo(target);
                break;

            case State.HIDE_BEHIND:
                states.HideBehind(target);
                break;

            default:
                break;
        }
    }

    IEnumerator ChangeStateAfterSec(State newState, float sec)
    {
        if (isTransitionRunning)
        { }
        else
        {
            isTransitionRunning = true; //  To prevent multiple coroutines running at the same time

            yield return new WaitForSeconds(sec);
            currentState = newState;
            isTransitionRunning = false;
            Debug.Log("Deer is " + newState);
        }


    }
}
