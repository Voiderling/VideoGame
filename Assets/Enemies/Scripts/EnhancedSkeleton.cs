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
    [SerializeField] private bool enableChase = true;
    private Animator anim;
    private Health playerHealth;
    private HeroKnight knight;
    private EnemyPatrool enemyPatrol;
    private EnemyHealth enemy;
    [Header("Chase Parameters")]
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float chaseDistance = 7f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float maxVerticalDistance = 1f; // Maximum Y distance to detect player
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
    private void Start()
    {
        enableChase = true;
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

        // Debug statement to help diagnose the issue
        if (player != null)
        {
            Debug.Log($"Player distance: {Vector2.Distance(transform.position, player.position)}, " +
                      $"On same level: {IsPlayerOnSameLevel()}, " +
                      $"Chase enabled: {enableChase}");
        }

        // Check if player is nearby (in front OR behind)
        if (PlayerInSight() || PlayerBehind())
        {
            Debug.Log("Player in attack range!");
            HandleAttackState();
        }
        // Then check for chase, but only on same vertical level
        else if (ShouldChasePlayer())
        {
            Debug.Log("Should chase player!");
            HandleChaseState();
        }
        else
        {
            HandlePatrolState();
        }
    }

    private bool IsPlayerOnSameLevel()
    {
        if (player == null) return false;
        float yDifference = Mathf.Abs(transform.position.y - player.position.y);
        return yDifference <= maxVerticalDistance;
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
        // Only resume patrol if player is outside horizontal range or not on attack state
        if (!isPlayerInCombatRange && !PlayerInSight() && !PlayerBehind())
            enemyPatrol.enabled = true;
    }

    // Check for player in front with boxcast
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

    // NEW: Check for player behind the enemy
    private bool PlayerBehind()
    {
        if (player == null) return false;

        // Check if player is behind based on enemy's facing
        bool playerIsBehind = (transform.localScale.x > 0 && player.position.x < transform.position.x) ||
                             (transform.localScale.x < 0 && player.position.x > transform.position.x);

        // Only detect if close enough
        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        float yDifference = Mathf.Abs(transform.position.y - player.position.y);

        // Player must be close behind AND on same level
        if (playerIsBehind && xDistance <= range && yDifference <= maxVerticalDistance)
        {
            // Get player's health component if we detect them
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerHealth = playerObj.GetComponent<Health>();

            return true;
        }

        return false;
    }

    private bool ShouldChasePlayer()
    {
        if (!enableChase || player == null) return false;

        // Simplify chase detection - just use direct distance
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= chaseDistance && distance > stoppingDistance;
    }

    private bool IsPlayerInHorizontalChaseRange()
    {
        if (player == null) return false;
        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        return xDistance <= chaseDistance;
    }

    private void ChasePlayer()
    {
        if (player == null || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

        // Face player before moving
        FacePlayer();

        // Only move on X axis, maintain current Y position
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

        // Stop at appropriate distance
        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        if (xDistance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                chaseSpeed * Time.deltaTime
            );
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
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

        if (!IsPlayerInHorizontalChaseRange() || !IsPlayerOnSameLevel())
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
        // Apply damage if player is either in front or behind
        if (PlayerInSight() || PlayerBehind())
        {
            SoundManager.instance.PlaySound(attackSound);
            playerHealth.TakeDamage(damage, transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Front attack range
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );

        // Rear detection
        Gizmos.color = Color.magenta;
        Vector3 backDirection = transform.localScale.x > 0 ? Vector3.left : Vector3.right;
        Gizmos.DrawWireSphere(
            transform.position + backDirection * range,
            0.5f
        );

        // Horizontal chase range
        Gizmos.color = Color.yellow;
        float yPos = transform.position.y;
        Gizmos.DrawLine(
            new Vector3(transform.position.x - chaseDistance, yPos, 0),
            new Vector3(transform.position.x + chaseDistance, yPos, 0)
        );

        // Vertical tolerance range
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            new Vector3(transform.position.x, yPos - maxVerticalDistance, 0),
            new Vector3(transform.position.x, yPos + maxVerticalDistance, 0)
        );
    }
}