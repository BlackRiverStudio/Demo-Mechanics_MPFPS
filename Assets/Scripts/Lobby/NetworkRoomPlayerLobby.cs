using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private Text[] playerNameText = new Text[4];
    [SerializeField] private Text[] playerReadyText = new Text[4];
    [SerializeField] private Button startGameButton;
    [SyncVar(hook = nameof(HandleDisplayNameChanged))] public string displayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))] public bool isReady = false;
    private bool isLeader;
    public bool IsLeader
    {
        get { return isLeader; }
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }
    private NetworkManagerLobby room;
    public NetworkManagerLobby Room
    {
        get
        {
            if (room != null) return room;
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }
    #region overrides
    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }
    public override void OnStartClient()
    {
        Room.roomPlayers.Add(this);
        UpdateDisplay();
    }
    public override void OnStopClient()
    {
        Room.roomPlayers.Remove(this);
        UpdateDisplay();
    }
    #endregion
    #region custom methods
    public void HandleDisplayNameChanged(string _oldValue, string _newValue) => UpdateDisplay();
    public void HandleReadyStatusChanged(bool _oldValue, bool _newValue) => UpdateDisplay();
    public void HandleReadyToStart(bool _readyToStart)
    {
        if (!IsLeader) return;
        startGameButton.interactable = _readyToStart;
    }
    private void UpdateDisplay()
    {
        if (!isLocalPlayer)
        {
            foreach (var player in Room.roomPlayers)
            {
                if (player.isLocalPlayer)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
        }
        for (int i = 0; i < playerNameText.Length; i++)
        {
            playerNameText[i].text = "Waiting for player...";
            playerReadyText[i].text = string.Empty;
        }
        for (int i = 0; i < Room.roomPlayers.Count; i++)
        {
            playerNameText[i].text = Room.roomPlayers[i].displayName;
            playerReadyText[i].text = Room.roomPlayers[i].isReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
        }
    }
    #endregion
    #region commands
    [Command] private void CmdSetDisplayName(string _displayName) => this.displayName = _displayName;
    [Command] public void CmdReadyUp()
    {
        isReady = !isReady;
        Room.NotifyPlayersOfReadyState();
    }
    [Command] public void CmdStartGame(int serverMapInt)
    {
        if (Room.roomPlayers[0].connectionToClient != connectionToClient) return;
        Room.StartGame(serverMapInt);
    }
    #endregion
}
