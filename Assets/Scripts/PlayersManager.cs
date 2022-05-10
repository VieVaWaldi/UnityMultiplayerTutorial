using UnityEngine;
using Unity.Netcode;

public class PlayersManager : NetworkBehaviour
{
    public static PlayersManager Instance { get; set; }
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

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

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (true)
            {
                playersInGame.Value++;
                DebugLogger.Instance.Log($"{id} just connected. Players in game {playersInGame.Value}");
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (true)
            {
                playersInGame.Value--;
                DebugLogger.Instance.Log($"{id} just disconnected. Players in game {playersInGame.Value}");
            }
        };
    }
}
