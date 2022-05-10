using TMPro;
using Unity.Collections;
using Unity.Netcode;

public class PlayerHUD : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    private bool overlaySet = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playerName.Value = $"Player {OwnerClientId}";
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = $"{playerName.Value}";
    }

    void Update()
    {
        if (!overlaySet && !string.IsNullOrEmpty(playerName.Value.ToString()))
        {
            // Für Player String Name ISerializable implementieren. Folg 2, ca Min 20
            // SetOverlay();
            overlaySet = true;
        }
    }

}

