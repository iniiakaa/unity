using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthBarBehavior : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth { get; private set; }

    [Header("UI Settings (Opsional)")]
    [Tooltip("Slider UI untuk Health Bar. Kosongkan jika ingin mencari Slider secara otomatis di child object.")]
    public Slider healthSlider;

    [Tooltip("Aktifkan agar Health Bar UI (World Space) selalu menghadap ke arah kamera.")]
    public bool faceCamera = false;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDie;

    private Camera mainCamera;

    private void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;

        
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        UpdateHealthUI();
    }

    private void LateUpdate()
    {
       
        if (faceCamera && healthSlider != null && mainCamera != null)
        {
            healthSlider.transform.rotation = mainCamera.transform.rotation;
        }
    }

    // Fungsi untuk menerima damage
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
        onTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    
    public void Heal(float healAmount)
    {
        if (currentHealth <= 0) return; /

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

            
            if (currentHealth < maxHealth && currentHealth > 0)
            {
                healthSlider.gameObject.SetActive(true);
            }
            else
            {
                healthSlider.gameObject.SetActive(false);
            }
        }
    }

    // Dipanggil ketika darah habis
    private void Die()
    {
        onDie?.Invoke();
    }
}
