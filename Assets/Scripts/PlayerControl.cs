using UnityEngine;
using Unity.Netcode;

public class PlayerControl : NetworkBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walk,
        ReverseWalk
    }

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private float rotationSpeed = 1.5f;

    [SerializeField]
    private Vector2 deafaultInitialPlanePosition = new Vector2(-4, 4);

    // NetworkVariables that contain all necessary Positions, will be synced with server
    [SerializeField]
    private NetworkVariable<Vector3> networkPlayerPosition = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Vector3> networkPlayerRotation = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    // Client has to cache its own last state to compare it to the server
    private Vector3 oldPlayerPosition;
    private Vector3 oldPlayerRotation;

    // Componenet that makes manipulating the player a lot easier
    private CharacterController characterController;

    private Animator animator;

    private void Awake()
    {
        // Get Component from Player Prefab
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // This spawns the player if we dont attach a custom spwaner
    private void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(deafaultInitialPlanePosition.x, deafaultInitialPlanePosition.y),
                0, Random.Range(deafaultInitialPlanePosition.x, deafaultInitialPlanePosition.y));
        }

    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        ClientMoveAndRotate();
        ClientAnimation();
    }

    /// <summary>
    /// Calculate new Position and Animation.
    /// Then send those to the server.
    /// </summary>
    private void ClientInput()
    {
        // Player Position and Rotation Changes
        Vector3 newRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);

        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 newPosition = direction * forwardInput;

        if (oldPlayerPosition != newPosition || oldPlayerRotation != newRotation)
        {
            oldPlayerPosition = newPosition;
            oldPlayerRotation = newRotation;

            // Send updates to the server
            UpdateClientPositionAndDirectionServerRpc(newPosition * speed, newRotation * rotationSpeed);
        }

        // Player State Changes
        if (forwardInput > 0)
        {
            UpdateClientAnimationStateServerRpc(PlayerState.Walk);
        }
        else if (forwardInput < 0)
        {
            UpdateClientAnimationStateServerRpc(PlayerState.ReverseWalk);
        }
        else
        {
            UpdateClientAnimationStateServerRpc(PlayerState.Idle);
        }


    }

    /// <summary>
    /// After the server got the update, update your local position and rotation.
    /// </summary>
    private void ClientMoveAndRotate()
    {
        if (networkPlayerPosition.Value != Vector3.zero)
        {
            characterController.Move(networkPlayerPosition.Value);
        }
        if (networkPlayerRotation.Value != Vector3.zero)
        {
            transform.Rotate(networkPlayerRotation.Value);
        }
    }

    /// <summary>
    /// After the server got the update, update your local animation state.
    /// </summary>
    private void ClientAnimation()
    {
        if (networkPlayerState.Value != PlayerState.Walk)
        {
            animator.SetFloat("Walk", 1.0f);
        }
        else if (networkPlayerState.Value != PlayerState.ReverseWalk)
        {
            animator.SetFloat("Walk", -1.0f);
        }
        else
        {
            animator.SetFloat("Walk", 0.0f);
        }
    }

    /// <summary>
    /// Most important method that communicates the NetworkVariales with the server.
    /// Name must end on ServerRpc.
    /// </summary>
    [ServerRpc]
    public void UpdateClientPositionAndDirectionServerRpc(Vector3 newPosition, Vector3 newRotation)
    {
        networkPlayerPosition.Value = newPosition;
        networkPlayerRotation.Value = newRotation;
    }

    [ServerRpc]
    public void UpdateClientAnimationStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }
}
