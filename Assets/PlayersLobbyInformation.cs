using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayersLobbyInformation : NetworkBehaviour
{
    private static PlayersLobbyInformation instance;
    public static PlayersLobbyInformation Instance { get { return instance; } }
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

    [ServerRpc(RequireOwnership = false)]
    public void addUpdateNameServerRpc(ulong newID, string newName)
    {
        if (!playerNames.ContainsKey(newID))
        {
            playerNames.Add(newID, newName);
        }
        else
        {
            playerNames[newID] = newName;
        }

        addUpdateNameClientRpc(newID, newName);
    }


    [ClientRpc]
    public void addUpdateNameClientRpc(ulong newID, string newName)
    {
        if (!playerNames.ContainsKey(newID))
        {
            playerNames.Add(newID, newName);
        }
        else
        {
            playerNames[newID] = newName;
        }
        try
        {
            foreach (Transform playerPanel in panels)
            {
                playerPanel.GetComponent<LobbyPlayerPanel>().UpdateEveryone(playerNames);
            }
        }
        catch { 
        
        }

    }

    [ServerRpc (RequireOwnership =false)]
    public void pullNamesServerRpc()
    {
        foreach (var player in playerNames)
        {
            pullEveryonesNameClientRpc(player.Key, player.Value);
        }

    }

    [ClientRpc]
    public void pullEveryonesNameClientRpc(ulong newID, string newName)
    {
        if (IsServer) { return; }
        if (!playerNames.ContainsKey(newID))
        {
            playerNames.Add(newID, newName);
        }
        else
        {
            playerNames[newID] = newName;
        }
    }
    public string GetMyName(ulong id)
    {


        pullNamesServerRpc();


        if (!IsOwner)
        {
            return "";

        }

        if (playerNames.ContainsKey(id))
        {
            addUpdateNameServerRpc(id, playerNames[id]);
            return playerNames[id];
        }
        else
        {
            addUpdateNameServerRpc(id, "Player " + id.ToString());
            return "Player " + id.ToString();
        }


    }


    // Update is called once per frame
    void Update()
    {

    }
}
