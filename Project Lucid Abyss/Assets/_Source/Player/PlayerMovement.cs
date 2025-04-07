using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform PlayerSprite;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private float moveSpeed = 5f;

    private bool facingRight = true;

    public void Move(float direction)
    {
        rigidbody2D.velocity = new Vector2(direction * moveSpeed, rigidbody2D.velocity.y);

        if (direction > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = PlayerSprite.localScale;
        scale.x *= -1;
        PlayerSprite.localScale = scale;
    }
}