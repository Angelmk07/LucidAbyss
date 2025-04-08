using System;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InteractionSystem interactionSystem;
    [SerializeField] private Animator animator;
    public static Action<bool> CanDo;

    private bool _canWalk = true;
    private float horizontalInput;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        interactionSystem = GetComponent<InteractionSystem>();
    }

    private void OnEnable()
    {
        CanDo += OnCanDoChanged;
    }

    private void OnDisable()
    {
        CanDo -= OnCanDoChanged;
    }

    private void OnCanDoChanged(bool result)
    {
        _canWalk = result;
        horizontalInput = 0;
    }

    private void Update()
    {
        if (_canWalk)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactionSystem.TryInteract();
            }
        }
        else
        {
            horizontalInput = 0;
        }
    }

    private void FixedUpdate()
    {
        if(animator != null)
        {
            animator.SetFloat("Speed", horizontalInput);
            animator.SetBool("Move", horizontalInput != 0);
        }
        playerMovement.Move(horizontalInput);

    }
}
