using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movenent : MonoBehaviour
{
    public CharacterController controller;      //  Player controller reference
    public float speed = 12.0f;                 //  Speed settings
    public float worldGravity = 9.8f;          //  Gravity settings variable
    public float jumpHeight = 3f;

    public Transform legs;
    public float groundDistance = 0.4f;     //  Distance to the surface to be considered as grounded
    public LayerMask terrainMask;       //  Falling velocity resets only if touched the ground

    Vector3 velocity;
    bool b_onGround = true;

    void Start()
    {
        
    }

    void Update()
    {
                ////    Ground/Air state check
        if(!b_onGround && Physics.CheckSphere(legs.position, groundDistance, terrainMask)) 
            HitGround();    //  Called every time you land

        if (b_onGround && !Physics.CheckSphere(legs.position, groundDistance, terrainMask))
            LeaveGround();    //  Called every time you loose the ground

                ////    Fall processing
        if (!b_onGround)    //  If player is in the air 
        {
            velocity.y -= worldGravity * Time.deltaTime;    //  inreasing falling velocity
            controller.Move(velocity * Time.deltaTime);     //  Applying falling velocity

            return; //  Stoping here to avoid mid air movement
        }

                ////    Movement processing
        float x = Input.GetAxis("Horizontal");  //  Getting input
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //  Calculating horizontal movement

        //  JUMP    //
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * worldGravity);
        }

        controller.Move(move * speed * Time.deltaTime); //  Applying horizontal movement
        controller.Move(velocity * Time.deltaTime);     //  Applying vertical movement


    }

    void HitGround()
    {
        b_onGround = true;  
        velocity.y = -1f;   //  Reseting the velocity to a small value (not a zero, othervise we never really reach the floor)

        //  Some fall damage & sound here:
        //
        //
    }

    void LeaveGround()
    {
        b_onGround = false;

    }
}
