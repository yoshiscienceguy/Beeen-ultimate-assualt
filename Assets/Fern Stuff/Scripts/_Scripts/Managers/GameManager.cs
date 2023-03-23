using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour {
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private NetworkController _playerPrefab;
    public Transform spawnArea;
    public NetworkVariable<int> playerCount;
    public TMP_Text playercounttext;

    private void Awake()
    {
        playercounttext = GameObject.Find("Remaining players").GetComponent<TMP_Text>();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        spawnArea = GameObject.Find("spawn A").transform;
        playerCount = new NetworkVariable<int>();

    }
    [ClientRpc]
    public void updatePlayerCountClientRpc(int value)
    {
        playercounttext = GameObject.Find("Remaining players").GetComponent<TMP_Text>();
        playerCount.Value = value;
        playercounttext.text = playerCount.Value.ToString();

    }

    [ServerRpc (RequireOwnership =false)]
    public void addPlayerServerRpc()
    {
        playercounttext = GameObject.Find("Remaining players").GetComponent<TMP_Text>();
        playerCount.Value += 1;
        updatePlayerCountClientRpc(playerCount.Value);
    }
    [ServerRpc(RequireOwnership = false)]
    public void oofPlayerServerRpc()
    {
        playerCount.Value -= 1;
    }
    public void oof()
    {
        playercounttext = GameObject.Find("Remaining players").GetComponent<TMP_Text>();
        oofPlayerServerRpc();
        playercounttext.text = playerCount.Value.ToString();
    }

    public override void OnNetworkSpawn() {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        addPlayerServerRpc();

    }   

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId) {
        var spawn = Instantiate(_playerPrefab);
        //Vector3 newSpawn = new Vector3(spawnArea.position.x + Random.Range(-5,5), spawnArea.position.y, spawnArea.position.z + Random.Range(-5, 5));
        //spawn.gameObject.transform.position = newSpawn;
        spawn.NetworkObject.SpawnWithOwnership(playerId);
    }

    public override void OnDestroy() {
        base.OnDestroy();
        MatchmakingService.LeaveLobby();
        if(NetworkManager.Singleton != null )NetworkManager.Singleton.Shutdown();
    }

    
}