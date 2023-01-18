using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour {
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private NetworkController _playerPrefab;
    public Transform spawnArea;

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

        spawnArea = GameObject.Find("spawn A").transform;
    }

    public override void OnNetworkSpawn() {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);

    }   

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId) {
        var spawn = Instantiate(_playerPrefab);
        Vector3 newSpawn = new Vector3(spawnArea.position.x + Random.Range(-5,5), spawnArea.position.y, spawnArea.position.z + Random.Range(-5, 5));
        spawn.gameObject.transform.position = newSpawn;
        spawn.NetworkObject.SpawnWithOwnership(playerId);
    }

    public override void OnDestroy() {
        base.OnDestroy();
        MatchmakingService.LeaveLobby();
        if(NetworkManager.Singleton != null )NetworkManager.Singleton.Shutdown();
    }

    
}