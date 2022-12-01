using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class drive : MonoBehaviour
{
    public bool isdriving;
    public float flyspeed = 10;
    public float zoomyflyspeed = 100;
    public float notzoomyflyspeed = 10;
    public float rotspeed = 90;
    Rigidbody rb;
    public Transform camrasnapper;
    public Transform camrasnapper2;

    // Start is called before the first frame update
    void Start()
    {
        flyspeed = notzoomyflyspeed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      if(isdriving == true)
      {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                flyspeed = zoomyflyspeed;
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                flyspeed = zoomyflyspeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                flyspeed = notzoomyflyspeed;
            }
            if (Input.GetKeyUp(KeyCode.RightShift))
            {
                flyspeed = notzoomyflyspeed;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float w = Input.GetAxis("Fly");

            Vector3 movment = new Vector3(h*flyspeed, w*(flyspeed/2), v*flyspeed);

            transform.Translate(movment * Time.deltaTime);

            float vv = Input.GetAxis("Mouse Y");
            Vector3 rot = new Vector3(-vv* rotspeed, 0, 0);
            transform.Rotate(rot * Time.deltaTime);

            float hh = Input.GetAxis("Mouse X");
            Vector3 rot1 = new Vector3(0, hh * rotspeed, 0);
            transform.Rotate(rot1 * Time.deltaTime);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(isdriving == false && Input.GetKeyDown(KeyCode.E))
            {
                other.transform.SetParent(transform);
                
                other.gameObject.SetActive(false);
                isdriving = true;
                Camera.main.transform.SetParent(camrasnapper);
                Camera.main.transform.localPosition = Vector3.zero;
                Camera.main.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isdriving == false && Input.GetKeyDown(KeyCode.E))
            {
                other.transform.SetParent(transform);
                Camera.main.transform.SetParent(camrasnapper);
                Camera.main.transform.localPosition = Vector3.zero;
                Camera.main.transform.localEulerAngles = Vector3.zero;
                other.gameObject.SetActive(false);
                isdriving = true;
            }
        }
        if (other.CompareTag("Player"))
        {
            if (isdriving == false && Input.GetKeyDown(KeyCode.Q))
            {
                other.transform.SetParent(transform);
                Camera.main.transform.SetParent(camrasnapper2);
                Camera.main.transform.localPosition = Vector3.zero;
                Camera.main.transform.localEulerAngles = Vector3.zero;
                other.gameObject.SetActive(false);
                isdriving = true;
            }
        }
    }

        
    


}
