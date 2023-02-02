using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class gunID : NetworkBehaviour
{
    public int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void gunAppearServerRpc(Vector3 newPosition, Vector3 newForceLocation, Vector3 incomingSpeed) {
        Debug.Log("hello again");
        if (IsServer)
        {
            //transform.position = newPosition;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().velocity = incomingSpeed;
            GetComponent<Rigidbody>().AddForce(newForceLocation, ForceMode.Impulse);
            //GetComponent<BoxCollider>().isTrigger = false;
            try
            {
                GetComponent<BoxCollider>().enabled = true;
            }
            catch
            {
                GetComponent<SphereCollider>().enabled = true;
            }
            transform.GetChild(0).gameObject.SetActive(true);
            tag = "Any gun";

        }
        gunAppearClientRpc(newPosition, newForceLocation);
    }
    [ClientRpc]
    void gunAppearClientRpc(Vector3 newPosition,Vector3 newForceLocation)
    {
        transform.position = newPosition;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(newForceLocation, ForceMode.Impulse);
        try
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        catch
        {
            GetComponent<SphereCollider>().enabled = true;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        tag = "Any gun";
    }

    [ServerRpc(RequireOwnership = false)]
    public void gunDisappearServerRpc() {

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        try
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        catch
        {
            GetComponent<SphereCollider>().enabled = false;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        tag = "Untagged";
        gunDisappearClientRpc();
    }

    [ClientRpc]
    void gunDisappearClientRpc() {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        try
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        catch
        {
            GetComponent<SphereCollider>().enabled = false;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        tag = "Untagged";
    }


}
