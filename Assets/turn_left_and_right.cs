using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turn_left_and_right : MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float rotspeed = 90;
    public float Sprintingspeed = 15f;
    public float walkingspeed = 10f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public bool hacksForDevs;
    void Start()
    {
        speed = walkingspeed;
        controller = GetComponent<CharacterController>();

        // let the gameObject fall down
        //gameObject.transform.position = new Vector3(0, 5, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            speed = Sprintingspeed;
        }
        if (Input.GetKeyDown(KeyCode.RightShift)) 
        {
            speed = Sprintingspeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = walkingspeed;
        }
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            speed = walkingspeed;
        }

        if (hacksForDevs == true)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                jumpSpeed = 100.0f;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                jumpSpeed = 25.0f;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                gravity = 9999.0f;
            }
            else
            {
                gravity = 50.0f;
            }
        }
        float oldy = moveDirection.y;
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection = moveDirection * speed;
        if (Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }
        if (hacksForDevs == false)
        {
            if (controller.isGrounded)
            {
               //We are grounded, so recalculate
               //move direction directly from axes



              if (Input.GetButton("Jump"))
              {
                moveDirection.y = jumpSpeed;
              }
            }
            else
            {
                moveDirection.y = oldy;
            }
        }
        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the co
        float h = Input.GetAxis("Mouse X");
        Vector3 rot = new Vector3(0, h*rotspeed, 0);
        transform.Rotate(rot * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);



    }
}
