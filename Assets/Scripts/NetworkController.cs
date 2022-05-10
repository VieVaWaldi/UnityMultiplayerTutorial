using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine.UI;
using TMPro;

public class NetworkController : NetworkBehaviour
{
    public static NetworkController Instance { get; private set; }

    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    [SerializeField]
    private TMP_InputField hostIPAdressInput;

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    private void Awake()
    {
        Cursor.visible = true;
        Instance = this;
    }

    private void Update()
    {
        playersInGameText.text = $"Players in Game: {PlayersInGame}";
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (true) // was IsServer in Tutorial, i think should be IsHost
            {
                playersInGame.Value++;
                DebugLogger.Instance.Log($"{id} just connected. Players in game {playersInGame.Value}");
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (true) // was IsServer in Tutorial, i think should be IsHost
            {
                playersInGame.Value--;
                DebugLogger.Instance.Log($"{id} just disconnected. Players in game {playersInGame.Value}");
            }
        };

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

        hostIPAdressInput.onEndEdit.AddListener((string ip) =>
        {
            DebugLogger.Instance.Log($"Entered IP Adress:{ip}");

            var unet = NetworkManager.Singleton.GetComponent<UNetTransport>();
            unet.ConnectAddress = ip;
        });
    }

    private void GameStarted()
    {
        // hostIPAdressInput.SetActive();
    }
}
