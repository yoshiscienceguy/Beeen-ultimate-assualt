using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghosteree : MonoBehaviour
{
    public float flySpeed = 50;
    public float mouseSensitivity = 300;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movemente = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movemente = Camera.main.transform.TransformDirection(movemente);
        transform.Translate(movemente * flySpeed * Time.deltaTime);

        Vector3 rot = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0)*mouseSensitivity*Time.deltaTime;
        transform.Rotate(rot);

        Vector3 Tem = transform.localEulerAngles;
        Tem.z = 0;
        transform.localEulerAngles = Tem;
            
    }
}
