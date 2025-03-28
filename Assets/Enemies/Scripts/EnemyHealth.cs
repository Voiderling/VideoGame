using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;
    private bool hurt;
    [SerializeField] private EnemyHealthBar healthBar;
    EnemyPatrool patrol;
    private  Transform tr;
    [Header("Enemy dying")]
    [SerializeField] private AudioClip dieEnemySound;
    [Header("Enemy Hurt")]
    [SerializeField] private AudioClip enemyHurtSound;
    EnhancedSkeleton enemy;
    [SerializeField] private bool isBoss = false;
    private bool isEnraged;
    private XPManager xp;
    private void Awake()
    {
        patrol = GetComponent<EnemyPatrool>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<EnhancedSkeleton>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        xp = GetComponent<XPManager>();
    }
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded. Death counter reset.");
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void EnemyTakeDamage(float damage)
    {

        if (anim != null)
        {
            healthBar.updateHealthBar(currentHealth, maxHealth);
            if(currentHealth > 0 && isBoss)
            {

                currentHealth -= damage;
            }
            if (currentHealth > 0)
            {
                currentHealth -= damage;
                anim.SetTrigger("hurt");
                Debug.Log(currentHealth);
                SoundManager.instance.PlaySound(enemyHurtSound);
                hurt = true;
            }            
            else if (currentHealth <= 0)
            {
                Die();
            }
            if ((currentHealth < maxHealth * 0.5f) && isBoss) {
                anim.SetTrigger("IsEnraged");
                isEnraged = true;
            }
        }
    }


    public void Die()
    {
        if(!dead && isBoss)
        {
            dead = true;
            anim.SetTrigger("death");
            SoundManager.instance.PlaySound(dieEnemySound);

            Destroy(gameObject,3f);
        }
        else if (!dead)
        {
            XPManager.Instance.AddXP(15);
            anim.SetTrigger("death");
            SoundManager.instance.PlaySound(dieEnemySound);
            Debug.Log("Enemy died");
            Destroy(gameObject, 0.9f);
            dead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float GetEnenemyHealth()
    {
        return currentHealth;
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    public bool IsHurt()
    {
        return hurt;
    }

    public void SetIsHurt(bool hurt)
    {
        this.hurt = hurt;
    }
    public bool IsDead()
    {
        return dead;
    }
    public void DisableHurt()
    {
        hurt = false;
    }
    public bool GetIsEnraged() {
        return isEnraged;
    }
}
