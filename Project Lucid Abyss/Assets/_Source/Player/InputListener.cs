using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InteractionSystem interactionSystem;

    private float horizontalInput;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        interactionSystem = GetComponent<InteractionSystem>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.E))
        {
            interactionSystem.TryInteract();
        }
    }

    private void FixedUpdate()
    {
        playerMovement.Move(horizontalInput);
    }
}