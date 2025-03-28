using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int attackDamage = 3;
    [SerializeField] private int enragedAttackDamage = 5;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private AudioClip attackSound;
    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            Health health = colInfo.GetComponent<Health>();
            if (health != null)
            {
                SoundManager.instance.PlaySound(attackSound);
                health.TakeDamage(attackDamage, transform.position);
            }
            else
            {
                Debug.LogWarning("Hit object without Health component: " + colInfo.name);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
