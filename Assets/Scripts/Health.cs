using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private HeroKnight knight;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private Vector3 checkpointPosition;

    private void Awake()
    {
        knight = GetComponent<HeroKnight>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        checkpointPosition = transform.position;
    }

    public void TakeDamage(float _damage, Vector3 attackerPosition)
    {
        if (knight.IsInvulnerable()) return;

        if (knight.IsBlocking())
        {
            bool isAttackerInFront = (knight.GetFacingDirection() == 1 && attackerPosition.x > transform.position.x) ||
                                    (knight.GetFacingDirection() == -1 && attackerPosition.x < transform.position.x);
            if (isAttackerInFront)
            {
                _damage = 0;
                anim.SetTrigger("Block");
                SoundManager.instance.PlaySound(knight.GetShieldSound());
                return;
            }
        }

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                GetComponent<HeroKnight>().disableMovement();
                GetComponent<HeroKnight>().enabled = false;
                anim.SetTrigger("Death");
                SoundManager.instance.PlaySound(deathSound);
                dead = true;

              }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
       for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    }
    private IEnumerator DeathSequence()
    {
        dead = true;
        GetComponent<HeroKnight>().enabled = false;
        GetComponent<HeroKnight>().disableMovement();
        anim.SetTrigger("Death");
        SoundManager.instance.PlaySound(deathSound);

        // Disable components temporarily

        yield return new WaitForSeconds(2f);
        GetComponent<PlayerRespawn>().Respawn();
    }
    public void FullRespawn()
    {
        dead = false;
        currentHealth = startingHealth;
        anim.ResetTrigger("Death");
        anim.Play("Idle");

        // Re-enable components
        foreach (Behaviour component in components)
            component.enabled = true;

        GetComponent<HeroKnight>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        spriteRend.enabled = true;
    }
}