using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class multiplayerIntro : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { gameObject.SetActive(false); return; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
