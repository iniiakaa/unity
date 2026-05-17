using UnityEngine;

[RequireComponent(typeof(HealthBarBehavior))]
public class SimpleSmartEnemy : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 2f;
    public float detectionRange = 5f;
    public float wanderRadius = 3f;
    
    [Header("References")]
    public Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isChasing;

    private HealthBarBehavior healthBehavior;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        startPosition = transform.position;
        SetNewTarget();

       
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;

        
        healthBehavior = GetComponent<HealthBarBehavior>();
        if (healthBehavior != null)
        {
            
            healthBehavior.onDie.AddListener(OnEnemyDeath);
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        
        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;
            targetPosition = player.position;
        }
        else
        {
            isChasing = false;
            
            if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
            {
                SetNewTarget();
            }
        }

        Move();
        HandleGraphics();
    }

    void Move()
    {
        
        if (isChasing && Vector2.Distance(transform.position, targetPosition) < 0.6f)
        {
            return;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.7f);

        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            
            targetPosition += new Vector2(direction.y, -direction.x) * 2f; 
        }
    }

    void SetNewTarget()
    {
        targetPosition = startPosition + new Vector2(Random.Range(-wanderRadius, wanderRadius), Random.Range(-wanderRadius, wanderRadius));
    }

    void HandleGraphics()
    {
        
        if (animator != null) animator.SetBool("isRunning", true);

       
        float distanceX = targetPosition.x - transform.position.x;
        if (distanceX > 0.1f) spriteRenderer.flipX = false;
        else if (distanceX < -0.1f) spriteRenderer.flipX = true;
    }

    void OnEnemyDeath()
    {
    
        Destroy(gameObject);
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}