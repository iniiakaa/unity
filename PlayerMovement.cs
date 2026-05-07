using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;

        Debug.Log("PlayerMovement: Inisialisasi selesai. Gravity Scale diset ke 0.");
    }

    void Update()
    {
        // Mendapatkan input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        
        if (movement != Vector2.zero)
        {
            Debug.Log($"Input Terdeteksi - X: {movement.x}, Y: {movement.y}");
        }

        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("isRunning", isMoving);

      
        if (movement.x < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            Debug.Log("Player menghadap ke Kiri");
        }
        else if (movement.x > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            Debug.Log("Player menghadap ke Kanan");
        }
    }

    void FixedUpdate()
    {
       
        Vector2 targetPosition = rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;

        
        rb.MovePosition(targetPosition);

        
    }
}
