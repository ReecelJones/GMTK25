using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float curHealth;

    public Slider healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            UpdateHealthBar();
        }
    }

    public void TakeDamage(float amount)
    {
        curHealth -= amount;
        curHealth = Mathf.Max(curHealth, 0f); //Ensure health does not drop below 0

        UpdateHealthBar();
        
        if (curHealth <= 0f)
        {
            Die();
        }
    }

    //Can implement if wanting to
    public void Heal(float amount)
    {
        curHealth += amount;
        curHealth = Mathf.Min(curHealth, maxHealth); //Ensures health does not exceed maxHealth

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            if (healthBar.IsActive())
            {
                print("YES!");
                healthBar.enabled = false;
            }
            else
            {
                healthBar.enabled = true;
                Invoke("UpdateHealthBar", 1.0f);
            }
                healthBar.value = curHealth;
        }
    }

    private void Die()
    {
        print(gameObject.name + "has died!");
        //Implement dealth logic - e.g. play animation
        Destroy(gameObject);
        GameManager.instance.OnEnemyKilled(this.GetComponent<EnemyUnit>());
    }

}
