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
        gameObject.SetActive(true);
        gunAppearClientRpc();
    }
    [ClientRpc]
    void gunAppearClientRpc()
    {
        gameObject.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void gunDisappearServerRpc() {

        gameObject.SetActive(false);
        gunDisappearClientRpc();
    }

    [ClientRpc]
    void gunDisappearClientRpc() {
        gameObject.SetActive(false);
    }


}
