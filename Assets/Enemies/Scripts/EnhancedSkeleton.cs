using UnityEngine;
using System.Collections;

public class EnhancedSkeleton : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool enableChase;
    private Animator anim;
    private Health playerHealth;
    private HeroKnight knight;
    private EnemyPatrool enemyPatrol;
    private EnemyHealth enemy;
    [Header("Chase Parameters")]
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float chaseDistance = 7f;
    [SerializeField] private float stoppingDistance = 1.5f;
    private Transform player;
    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;
    [Header("Hurt Response")]
    [SerializeField] private float returnPatrolDelay = 3f;
    private float previousHealth;
    private Coroutine returnRoutine;
    private bool isPlayerInCombatRange;
    private bool enemyState;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrool>();
        enemy = GetComponent<EnemyHealth>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogError("Player not found!");
        previousHealth = enemy.currentHealth;
        knight = GetComponent<HeroKnight>();
    }

    private void Update()
    {
        HandleHealthCheck();
        UpdateCombatState();
    }

    private void HandleHealthCheck()
    {
        if (enemy.currentHealth < previousHealth) OnHurt();
        previousHealth = enemy.currentHealth;
    }

    private void UpdateCombatState()
    {
        cooldownTimer += Time.deltaTime;
        if (enemy.IsDead()) return;
        if (PlayerInSight())
        {
            HandleAttackState();
        }
        else if (ShouldChasePlayer())
        {
            HandleChaseState();
        }
        else
        {
            HandlePatrolState();
        }
    }

    private void HandleAttackState()
    {
        enemyPatrol.enabled = false;
        if (cooldownTimer >= attackCooldown) StartAttack();
    }

    private void HandleChaseState()
    {
        enemyPatrol.enabled = false;
        ChasePlayer();
    }

    private void HandlePatrolState()
    {
        if (!IsPlayerInChaseRange() && !isPlayerInCombatRange)
            enemyPatrol.enabled = true;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private bool ShouldChasePlayer()
    {
        if (player == null) return false;
        return IsPlayerInChaseRange() && !PlayerInSight();
    }

    private bool IsPlayerInChaseRange()
    {
        return Vector2.Distance(transform.position, player.position) <= chaseDistance;
    }

    private void ChasePlayer()
    {
        if (player == null || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

        // Crucial fix: Face player FIRST before moving
        FacePlayer();

        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            chaseSpeed * Time.deltaTime
        );

        anim.SetBool("isMoving", true);
    }

    private void StartAttack()
    {
        anim.SetTrigger("attack");
        anim.SetBool("isMoving", false);
        cooldownTimer = 0;
    }

    private void OnHurt()
    {
        FacePlayer();
        enemyPatrol.enabled = false;
        isPlayerInCombatRange = true;
        if (returnRoutine != null) StopCoroutine(returnRoutine);
        returnRoutine = StartCoroutine(CombatCooldown());
    }

    private IEnumerator CombatCooldown()
    {
        yield return new WaitForSeconds(returnPatrolDelay);

        if (!IsPlayerInChaseRange())
        {
            enemyPatrol.enabled = true;
            isPlayerInCombatRange = false;
        }
        else
        {
            returnRoutine = StartCoroutine(CombatCooldown());
        }
    }

    private void FacePlayer()
    {
        if (player == null) return;
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(player.position.x - transform.position.x);
        transform.localScale = newScale;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            SoundManager.instance.PlaySound(attackSound);
            playerHealth.TakeDamage(damage, transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }
}