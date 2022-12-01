using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turn_up_and_down : MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float rotspeed = -90;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Mouse Y");
        Vector3 rot = new Vector3(-v * rotspeed , 0, 0);
        transform.Rotate(rot * Time.deltaTime);
        

    }
}
