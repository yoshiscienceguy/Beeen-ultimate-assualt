using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class rigidbodyObject : NetworkBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
