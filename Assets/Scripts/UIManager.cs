using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                DebugLogger.Instance.Log("Server started succesfully.");
            }
            else
            {
                DebugLogger.Instance.Log("Server could not be started.");
            }
        });

        startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                DebugLogger.Instance.Log("Host started succesfully.");
            }
            else
            {
                DebugLogger.Instance.Log("Host could not be started.");
            }
        });

        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                DebugLogger.Instance.Log("Client started succesfully.");
            }
            else
            {
                DebugLogger.Instance.Log("Client could not be started.");
            }
        });
    }

    private void Update()
    {
        playersInGameText.text = $"Players in Game: {PlayersManager.Instance.PlayersInGame}";
    }
}
