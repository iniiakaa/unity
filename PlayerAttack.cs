using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 25f;
    public float attackRange = 0.8f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [Header("Hitbox Settings")]
    [Tooltip("Titik pusat serangan. Jika kosong, script akan membuatnya otomatis.")]
    public Transform attackPoint;
    public float attackPointOffsetX = 1f;
    
    [Tooltip("Layer khusus musuh (biarkan Everything jika belum mengatur Layer)")]
    public LayerMask enemyLayers = ~0; 

    [Header("Animation")]
    [Tooltip("Nama Parameter Bool di Animator untuk memainkan animasi attack")]
    public string attackBoolName = "IsAttacking";
    public float attackAnimDuration = 0.25f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    Vector3 GetAttackPos()
    {
        if (attackPoint != null) return attackPoint.position;

        SpriteRenderer sr = spriteRenderer != null ? spriteRenderer : GetComponent<SpriteRenderer>();
        float offsetX = (sr != null && sr.flipX) ? -attackPointOffsetX : attackPointOffsetX;
        return transform.position + new Vector3(offsetX, 0, 0);
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetBool(attackBoolName, true);
            CancelInvoke("ResetAttackBool");
            Invoke("ResetAttackBool", attackAnimDuration);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(GetAttackPos(), attackRange, enemyLayers);

        
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.gameObject == gameObject) continue;

            HealthBarBehavior health = hit.GetComponent<HealthBarBehavior>();
            
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    void ResetAttackBool()
    {
        if (animator != null)
        {
            animator.SetBool(attackBoolName, false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackPos(), attackRange);
    }
}
