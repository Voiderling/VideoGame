using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss_Run : StateMachineBehaviour
{
    [SerializeField] private float speed = 8.5f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float chaseBuffer = 0.5f; // Added buffer zone
    private bool rageAttack;
    Transform player;
    Rigidbody2D rb;
    Boss boss;
    float cooldownTimer;
    float actualAttackRange; // Dynamic attack range calculation

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        cooldownTimer = 0;
        actualAttackRange = attackRange + chaseBuffer;
        rageAttack = animator.GetComponent<EnemyHealth>().GetIsEnraged();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        float currentDistance = Vector2.Distance(
            new Vector2(player.position.x, 0),
            new Vector2(rb.position.x, 0)
        );

        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);

        // Attack logic
        if (currentDistance <= attackRange && cooldownTimer <= 0)
        {
            animator.SetBool("IsMoving", false);
            if (rageAttack)
            {
                int randomAttack = Random.Range(1, 4);
                speed = 10.5f;
                animator.SetTrigger("Attack" + randomAttack);
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            cooldownTimer = attackCooldown;
            actualAttackRange = attackRange + chaseBuffer; // Expand range temporarily
        }
        // Chase logic with buffer zone
        else if (currentDistance > actualAttackRange)
        {
            animator.SetBool("IsMoving", true);

            // Calculate target position at edge of attack range
            Vector2 direction = (player.position - rb.transform.position).normalized;
            Vector2 targetPosition = (Vector2)player.position - direction * attackRange;

            rb.MovePosition(Vector2.MoveTowards(
                rb.position,
                targetPosition,
                speed * Time.fixedDeltaTime
            ));

            // Reset to normal range after initiating chase
            actualAttackRange = attackRange;
        }
        // Maintain idle if within buffer zone
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.SetBool("IsMoving", false);
    }
}