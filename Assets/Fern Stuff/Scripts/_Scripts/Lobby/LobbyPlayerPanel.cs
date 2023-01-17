using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyPlayerPanel : MonoBehaviour {
    [SerializeField] private TMP_Text  _statusText;
    [SerializeField] private TMP_Text  _NameText;
    [SerializeField] private TMP_InputField _Name;

    string _name;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId,ulong ownerID) {
        PlayerId = playerId;
        _name = "Player " + playerId.ToString();
        _Name.text = _name;
        FindObjectOfType<PlayersLobbyInformation>().updateNamesServerRpc(playerId, _name);
        if (ownerID == playerId)
        {
            _NameText.gameObject.SetActive(false);
        }
        else {
            _Name.gameObject.SetActive(false);
        }
        //_nameText.text = $"Player {playerId}";
    }
    public void UpdateMyName() {
        _name = _Name.text;
        FindObjectOfType<PlayersLobbyInformation>().updateNamesServerRpc(PlayerId, _name);
    }
    public void UpdateEveryone(Dictionary<ulong,string> names) {
        if (names.ContainsKey(PlayerId)) {
         
            
            _NameText.text = names[PlayerId];
            _name = names[PlayerId];
        }
     
    }

    public void SetReady() {
        _statusText.text = "Ready";
        _statusText.color = Color.green;
    }
}