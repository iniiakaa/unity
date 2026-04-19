using UnityEngine;

public class EnemyBot : MonoBehaviour
{
    [Header("Wander Settings")]
    public float speed = 2f;
    public float wanderRadius = 5f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("References")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float waitCounter;
    private bool isWaiting;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        startPosition = transform.position;
        SetNewTargetPosition();
    }

    void Update()
    {
        if (isWaiting)
        {
            
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                isWaiting = false;
                SetNewTargetPosition();
            }
        }
        else
        {
            
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }

            
            if (spriteRenderer != null)
            {
                if (targetPosition.x > transform.position.x)
                {
                    spriteRenderer.flipX = false; // Ke Kanan
                }
                else if (targetPosition.x < transform.position.x)
                {
                    spriteRenderer.flipX = true; // Ke Kiri
                }
            }

            
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWaiting = true;
                waitCounter = Random.Range(minWaitTime, maxWaitTime); // Tunggu sebentar

                
                if (animator != null)
                {
                    animator.SetBool("isRunning", false);
                }
            }
        }
    }

    
    private void SetNewTargetPosition()
    {
        float randomX = Random.Range(-wanderRadius, wanderRadius);
        float randomY = Random.Range(-wanderRadius, wanderRadius);
        targetPosition = startPosition + new Vector2(randomX, randomY);
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        // Lingkaran area wandering
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(startPosition, wanderRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }
        
        
        if (Application.isPlaying && !isWaiting)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(targetPosition, 0.2f);
        }
    }
}
