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
    public void gunAppearServerRpc() {
        Debug.Log("hello again");
        if (IsServer)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            //GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<BoxCollider>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            tag = "Untagged";

        }
        gunAppearClientRpc();
    }
    [ClientRpc]
    void gunAppearClientRpc()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        tag = "Any gun";
    }

    [ServerRpc(RequireOwnership = false)]
    public void gunDisappearServerRpc() {

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        tag = "Untagged";
        gunDisappearClientRpc();
    }

    [ClientRpc]
    void gunDisappearClientRpc() {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        tag = "Untagged";
    }


}
