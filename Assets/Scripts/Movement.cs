using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;      //  Player controller reference
    public float maxSpeed = 8.0f;                 //  Speed settings
    public float worldGravity = 9.8f;          //  Gravity settings variable
    public float jumpHeight = 0.5f;

    public Transform legs;
    public float groundDistance = 0.3f;         //  Distance to the surface to be considered as grounded
    public float groundInteractDistance = 0.5f; //  Distance to the surface to be able to interact with it
    public LayerMask terrainMask;       //  Falling velocity resets only if touched the ground

    Vector3 velocity;               //  Vector that stores player speed
    bool b_onGround = true;
    bool b_closeToGround = true;    //  Is player close enough to ground to interact with it
    float speed;

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

        b_closeToGround = Physics.CheckSphere(legs.position, groundInteractDistance, terrainMask);

                ////    Fall processing
        if (!b_onGround)    //  If player is in the air 
        {
            velocity.y -= worldGravity * Time.deltaTime;    //  inreasing falling velocity
        }

                ////    Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = maxSpeed;
            //some endurance checking here later
        }
        else
            speed = 4f;

                ////    Movement processing
        if(b_closeToGround)
        {
            float x = Input.GetAxis("Horizontal") * speed;  //  Getting input
            float z = Input.GetAxis("Vertical") * speed;

            velocity = transform.right * x + transform.up * velocity.y + transform.forward * z; //  Calculating horizontal movement

            ////    Jump
            if (Input.GetButtonDown("Jump"))
                Jump();
        }

            


        //! MOVEMENT CODE ABOVE THIS LINE !//
        controller.Move(velocity * Time.deltaTime);     //  Applying movement
    }



    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * 2f * worldGravity);    //  Calculate velocity required to jump to the given height
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
