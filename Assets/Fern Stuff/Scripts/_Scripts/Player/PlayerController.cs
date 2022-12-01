using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public Transform A;


    public override void OnNetworkSpawn()
    {

        if (!IsOwner) { tag = "Other Player";gameObject.layer = 0;name = ""; enabled = false; }

        GameObject B = GameObject.Find("spawn A");
        if (B == null) { return; }
        A = B.transform;
        Vector3 rPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + A.position;
        transform.position = rPos;
    }
}