using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private int health = 10;
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private bool isInvulnreable = false;
    [SerializeField] Animator animator;
    public void TakeDamage(int damage)
    {
        if (isInvulnreable)
        {
            return;
        }
        health -= damage;
        if(health <= 5)
        {
            GetComponent<Animator>().SetBool("isEnraged", true);
        }
        if(health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        animator.SetBool("die", true);
        Destroy(gameObject,3);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
