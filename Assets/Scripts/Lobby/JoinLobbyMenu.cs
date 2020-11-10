using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager;
    [SerializeField] private GameObject landingPagePanel;
    [SerializeField] private InputField adressInputField;
    [SerializeField] private Button joinButton;
    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConn;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconn;
    }
    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConn;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconn;
    }
    public void JoinLobby()
    {
        string ipAdress = adressInputField.text;
        networkManager.networkAddress = ipAdress;
        networkManager.StartClient();
        joinButton.interactable = false;
    }
    private void HandleClientConn()
    {
        joinButton.interactable = true;
        landingPagePanel.SetActive(false);
        gameObject.SetActive(false);
    }
    private void HandleClientDisconn() => joinButton.interactable = true;
}
