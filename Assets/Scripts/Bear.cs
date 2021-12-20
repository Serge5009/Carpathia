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

public class Bear : MonoBehaviour
{
    public GameObject target;
    NPCState states;
    NPCVision vision;
    bool isTransitionRunning = false;

    State currentState = State.WANDER;  //  Wander by default


    void Start()
    {
        states = this.GetComponent<NPCState>();
        vision = this.GetComponent<NPCVision>();
    }

    void Update()
    {
        StateTransitions();
        ApplyState();
    }

    void StateTransitions()
    {
        if (currentState == State.WANDER)   //  Transitions from WANDER
        {
            if (vision.CanSee(target))
            {
                currentState = State.PURSUE;
                Debug.Log("Bear is pursuing");

            }

        }
        if (currentState == State.PURSUE)   //  Transitions from PURSUE
        {
            if (!vision.CanSee(target))
            {
                ChangeStateAfterSec(State.WANDER, 2);
            }

        }
        if (currentState == State.EVADE)   //  Transitions from EVADE
        {


        }
        if (currentState == State.HIDE_TO)   //  Transitions from HIDE_TO
        {


        }
        if (currentState == State.HIDE_BEHIND)   //  Transitions from HIDE_BEHIND
        {


        }
    }

    void ApplyState()   //  This one should be implemented in parrent FSM class
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
            Debug.Log("Bear is " + newState);
        }


    }
}
