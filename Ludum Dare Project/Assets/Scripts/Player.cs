using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    
    public float moveSpeed = 1.0f;
    public float thrust = 3.0f;
    public GameObject vomit;

    private bool isFacingRight = true;
    
    public float maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;
    
    Rigidbody2D rb;
    public Transform firePoint;

    SpriteRenderer sr;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    AudioSource dmgSound;
    // Start is called before the first frame update
    void Start()
    {
        dmgSound = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();

        // First child should have a sprite
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();


        // Used in collisions and invincibletimer.
        // Hacky stuff
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        invincible = false;
    }

    private void FixedUpdate()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 movementForce = (targetPos - rb.position).normalized;
        if (rb.velocity.sqrMagnitude <= moveSpeed * moveSpeed)
        {
            rb.AddForce(movementForce * moveSpeed);
        }
        else
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, moveSpeed);
        }
    }
    // Update is called once per frame

    public float foodCost = 2f;
    void Update()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float remaining = targetPos.x - transform.position.x;
        
        if(Time.timeScale == 1f) {
            if(remaining > 0 && !isFacingRight){
                Flip();
            }else if(remaining < 0 && isFacingRight){
                Flip();
            }
        }

        if(Input.GetButtonDown("Fire1")) {

            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dirx = targetPos.x - transform.position.x;
            float diry = targetPos.y - transform.position.y;
            float angle = Mathf.Atan2(diry, dirx) * Mathf.Rad2Deg;

            Instantiate(vomit, firePoint.position, Quaternion.AngleAxis(angle, vomit.transform.forward));

            currentHealth -= foodCost;
            healthBar.SetHealth(currentHealth);
        }

        if(currentHealth <= 0) {
            SceneManager.LoadScene(3);
        }
    }
    void Flip() {
		isFacingRight = !isFacingRight;

		// transform.localScale = new Vector3(transform.localScale.x*-1, 1, 1);
        transform.Rotate(0, 180f, 0);
	}

    public void TakeDamage(float damage) {
        dmgSound.Play();
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (invincibilityAllowed)
        {
            // Start invicibility countdown and invincibility effect
            invincible = true;
            StartCoroutine(invincibleTimer());
        }
    }

    public void eatFood(float food) {
        if (currentHealth + food > maxHealth)
        {
            currentHealth = maxHealth;
        }else
        {
            currentHealth += food;
        }
        healthBar.SetHealth(currentHealth);
    }

    public bool invincibilityAllowed = true;
    private bool invincible; // Set to false at Start
    public float damageAmount = 5f;
    void OnCollisionStay2D(Collision2D hitInfo)
    {
        if (!invincible)
        {
            if (hitInfo.gameObject.name == "Hunter(Clone)")
            {
                // Take damage from Hunters. HunterBullets will deal damage in their own thing. 
                TakeDamage(damageAmount);
            }
            else if (hitInfo.gameObject.name.StartsWith("Wo"))
            {
                // Take damage from Wolf
                TakeDamage(damageAmount);
            }
            
        }
    }


    public float timeInvincible = 2f;
    public int numFlashes = 4;
    /**
     * Assumed that you've been hit and invincible is true.
     */
    IEnumerator invincibleTimer()
    {
        //Flash red for a sec; this means there is at least a slight delay before taking damage again
        sr.color = Color.red;
        yield return new WaitForSeconds(timeInvincible / 2 / numFlashes);
        sr.color = Color.white;
        for (int i = 0; i < numFlashes; i++)
        {
            //Flash
            sr.material.shader = shaderGUItext;
            yield return new WaitForSeconds(timeInvincible / 2 / numFlashes);
            //Unflash
            sr.material.shader = shaderSpritesDefault;
            yield return new WaitForSeconds(timeInvincible / 2 / numFlashes);
        }
        sr.material.shader = shaderSpritesDefault;
        invincible = false;
    }
}
