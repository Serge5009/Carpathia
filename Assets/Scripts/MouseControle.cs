using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControle : MonoBehaviour
{

    public float viewSensitivity = 100f;    //  Sensitivity settigs
    public bool b_invertX, b_invertY;       //  Inversion settings
    public Transform Body;                  //  Reference to the player object

    float xRotation = 0f;                   //  Current head rotation

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //  Disabling cursor
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * viewSensitivity * Time.deltaTime; //  Getting mouse position for body
        float mouseY = Input.GetAxis("Mouse Y") * viewSensitivity * Time.deltaTime; //  Getting mouse position for head

        if (b_invertY)                                                  //  Head rotation inversion
            xRotation += mouseY;
        else
            xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);                  //  Limiting head rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  //  Rotating head

        Body.Rotate(Vector3.up * mouseX);   //  Rotating body
    }
}
