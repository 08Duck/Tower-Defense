using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Transform Respawn;
    public int maxHealth = 100;
    private float currentHealth;
    private float targetHealth;
    public Text HpPotsText;
    public int MaxHpPots = 3;
    private int HpPots;
    public float lerpSpeed = 10f;
    public Slider healthBar;

    private bool isDead = false;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        targetHealth = maxHealth;

        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        UpdateHpPots();
    }
    private void UpdateHpPots()
    {
        HpPotsText.text = "" + HpPots;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HpPotion") && HpPots != MaxHpPots)
        {
            HpPots++;
            UpdateHpPots();
            Destroy(other.gameObject); // Destroy the potion you collided with
        }
    }
    void Update()
    {
        if (targetHealth <= 0 && !isDead)
        {
            StartCoroutine(Die());
        }

        currentHealth = Mathf.Lerp(currentHealth, targetHealth, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(currentHealth - targetHealth) < 0.01f)
        {
            currentHealth = targetHealth;
        }

        if (healthBar)
        {
            healthBar.value = currentHealth;
        }

        if(Input.GetKeyDown("q") && HpPots>=1)
        {
            HpPots--;
            UpdateHpPots();
            Heal(50);
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        isInvincible = true;

        // Återställ direkt
        transform.position = Respawn.position;
        currentHealth = maxHealth;
        targetHealth = maxHealth;

        // Vänta 1.5 sekunder med odödlighet
        yield return new WaitForSeconds(1.5f);

        isDead = false;
        isInvincible = false;
    }

    public void TakeDamage(int amount)
    {
        if (!isInvincible)
        { 
            targetHealth = Mathf.Clamp(targetHealth - amount, 0, maxHealth);
        Debug.Log("Taking Damage. Target Health: " + targetHealth);
    }
    }

    public void Heal(int amount)
    {
        targetHealth = Mathf.Clamp(targetHealth + amount, 0, maxHealth);
        Debug.Log("Healing. Target Health: " + targetHealth);
    }

}
