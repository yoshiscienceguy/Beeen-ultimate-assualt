using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class NetworkController : NetworkBehaviour
{
    public Transform Spawn;
    public GameObject Camera;
    private ulong myid;
    public TMP_Text playerName;



    public override void OnNetworkSpawn()
    {
        if (IsOwner) {
            Camera.gameObject.SetActive(true);

        }
        GameObject safetySpawn = GameObject.Find("spawn A");
        if (safetySpawn == null)
        {
            return;
        }
        Spawn = safetySpawn.transform;
        Vector3 rPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + Spawn.position;
        transform.position = rPos;
       
        name = PlayersLobbyInformation.Instance.GetMyName(GetComponent<NetworkObject>().OwnerClientId);
        playerName.text = name;

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
