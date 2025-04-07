using System;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InteractionSystem interactionSystem;
    public Action<bool> CanWalk;
    private bool _canWalk = true;
    private float horizontalInput;

    private void Awake()
    {

         playerMovement = GetComponent<PlayerMovement>();
        interactionSystem = GetComponent<InteractionSystem>();
    }

    private void Update()
    {
        CanWalk += (bool result) => _canWalk = result;
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.E))
        {
            interactionSystem.TryInteract();
        }
    }

    private void FixedUpdate()
    {
        if(_canWalk)
            playerMovement.Move(horizontalInput);
    }
}