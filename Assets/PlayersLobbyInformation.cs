using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayersLobbyInformation : NetworkBehaviour
{
    private static  PlayersLobbyInformation instance;
    public static  PlayersLobbyInformation Instance { get { return instance; } }
    public Dictionary<ulong, string> playerNames = new Dictionary<ulong, string>();
    public Transform panels;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    
    }

    [ServerRpc(RequireOwnership =false)]
    public void updateNamesServerRpc(ulong newID, string newName) {
        
        updateEveryoneClientRpc(newID, newName);   
    }


    [ClientRpc]
    public void updateEveryoneClientRpc(ulong newID, string newName)
    {
        if (!playerNames.ContainsKey(newID))
        {
            playerNames.Add(newID, newName);
        }
        else
        {
            playerNames[newID] = newName;
        }
        foreach (Transform playerPanel in panels) {
            playerPanel.GetComponent<LobbyPlayerPanel>().UpdateEveryone(playerNames);
        }
        
    }

    public string GetMyName(ulong id) {
        if (playerNames.ContainsKey(id))
        {
            return playerNames[id];
        }
        else {
            return "N/A";
        }
    }
 

    // Update is called once per frame
    void Update()
    {
        
    }
}
