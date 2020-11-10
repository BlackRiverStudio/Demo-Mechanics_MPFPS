using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;
    public static string DisplayName { get; private set; }
    private const string playerPrefsNameKey = "Player Name";
    private void Start() => SetUpInputField();
    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(playerPrefsNameKey))
        {
            return;
        }
        string defaultName = PlayerPrefs.GetString(playerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }
    public void SetPlayerName(string _name) => continueButton.interactable = !string.IsNullOrEmpty(_name);
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(playerPrefsNameKey, DisplayName);
    }
}
