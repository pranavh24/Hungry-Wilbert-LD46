using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
public class WilbertJr : MonoBehaviour
{

    public HealthBar healthBar;
    public float maxHealth;
    float currentHealth;

    int gotHitID;
    int invincibleOverID;

    public float damageAmount = 10f;

    AudioSource dmgSound;

    SpriteRenderer sr;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    AIPath astar;
    // Start is called before the first frame update
    void Start()
    {
        dmgSound = GetComponent<AudioSource>();
        invincible = false;
        gotHitID = Animator.StringToHash("GotHit");
        invincibleOverID = Animator.StringToHash("InvincibleOver");
        // First child should be the one holding the sprite
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        print(sr);
        // Hacky stuff
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        currentHealth = maxHealth;

        astar = GetComponent<AIPath>();
        // Health bar

        healthBar.SetMaxHealth(maxHealth);

        StartCoroutine(GetHungrier());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(3);
        }

        UpdateHealthBar();
        print(astar.desiredVelocity.x);
        sr.flipX = astar.desiredVelocity.x < 0;
    }

    private void UpdateHealthBar()
    {
        healthBar.SetHealth(currentHealth);
    }


    public float hungerSpeed = 3f;
    IEnumerator GetHungrier()
    {
        while(true)
        {
            currentHealth -= 1;
            yield return new WaitForSeconds(1 / hungerSpeed);
        }
    }

    public float healAmount = 5f;
    public GameObject healFX;
    public void GetFed()
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Instantiate(healFX, transform);
    }

    public bool invincibilityAllowed = true;
    bool invincible; // Set to false at Start; here because it's appropriate
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (invincibilityAllowed)
        {
            if (!invincible)
            {
                if (collision.transform.name.StartsWith("Wo") || collision.transform.name.StartsWith("Hu"))
                {
                    dmgSound.Play();
                    invincible = true;
                    currentHealth -= damageAmount;
                    // Start invicibility countdown and invincibility effect
                    StartCoroutine(invincibleTimer());
                }
            }
        } else
        {
            if (collision.transform.name.StartsWith("Wo") || collision.transform.name.StartsWith("Hu"))
            {
                dmgSound.Play();
                currentHealth -= damageAmount;
            }
        }
        
    }

    public float timeInvincible = 2f;
    public float numFlashes = 4;
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
        invincible = false;
    }
}
