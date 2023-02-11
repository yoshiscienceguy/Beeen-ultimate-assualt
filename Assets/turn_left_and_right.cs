using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class turn_left_and_right : NetworkBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float rotspeed = 90;
    public float Sprintingspeed = 15f;
    public float walkingspeed = 10f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public float stamina = 100;
    public float maxStamina = 100;
    public float staminaRate;
    public float staminaCooldown;
    public float staminaGain;
    private float cooldownTime;
    public Image StaminaMeter;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            StaminaMeter = GameObject.Find("Foreground_StaminaBar").GetComponent<Image>();
        }

    }
    void Start()
    {
        stamina = maxStamina;
        speed = walkingspeed;
        controller = GetComponent<CharacterController>();

        // let the gameObject fall down
        //gameObject.transform.position = new Vector3(0, 5, 0);
    }

    void Update()
    {

        if (stamina >= 0)
        {

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                speed = Sprintingspeed;
                Camera.main.fieldOfView = 80;
                stamina -= staminaRate * Time.deltaTime;
                if (stamina <= 0) {
                    speed = walkingspeed;
                }
                StaminaMeter.fillAmount = stamina / maxStamina;
                cooldownTime = 0;
            }

            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                speed = walkingspeed;
                Camera.main.fieldOfView = 60;
            }

        }
        if (stamina < maxStamina)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (cooldownTime > staminaCooldown)
                {
                    stamina += staminaGain * Time.deltaTime;
                    if (stamina >= maxStamina)
                    {
                        stamina = maxStamina;
                        cooldownTime = 0;
                    }
                    StaminaMeter.fillAmount = stamina / maxStamina;


                }
                else
                {
                    cooldownTime += Time.deltaTime;
                }
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

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the co
        float h = Input.GetAxis("Mouse X");
        Vector3 rot = new Vector3(0, h * rotspeed, 0);
        transform.Rotate(rot * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);



    }
}
