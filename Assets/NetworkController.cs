using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkController : NetworkBehaviour
{
    public Transform Spawn;

    private ulong myid;


    public override void OnNetworkSpawn()
    {
        GameObject safetySpawn = GameObject.Find("spawn A");
        if (safetySpawn == null)
        {
            return;
        }
        Spawn = safetySpawn.transform;
        Vector3 rPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + Spawn.position;
        transform.position = rPos;
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
