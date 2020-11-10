using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
public class NetworkManagerLobby : NetworkManager
{
    [Header("Lobby")]
    [SerializeField] private int minPlayers = 2;
    [SerializeField, Scene] private string menuScene = string.Empty;
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();
    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) return false;
        foreach (var player in roomPlayers) if (!player.isReady) return false;
        return true;
    }
    #region overrides
    #region server
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("").ToList();
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            roomPlayers.Remove(player);
            NotifyPlayersOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = roomPlayers.Count == 0;
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }
    public override void OnStopServer() => roomPlayers.Clear();
    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Map_Scene"))
        {
            for (int i = roomPlayers.Count -1; i >= 0; i--)
            {
                var conn = roomPlayers[i].connectionToClient;
                var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(roomPlayers[i].displayName);
                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
            }
        }
        base.ServerChangeScene(newSceneName);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Map_Scene"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }
    #endregion
    #region client
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs) ClientScene.RegisterPrefab(prefab);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }
    #endregion
    #endregion
    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in roomPlayers) player.HandleReadyToStart(IsReadyToStart());
    }
    public void StartGame(int serverMapInt)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart()) return;
            ServerChangeScene("Map_Scene_" + serverMapInt);
        }
    }
}
