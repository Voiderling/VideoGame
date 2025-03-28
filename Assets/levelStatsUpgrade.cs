using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private HeroKnight playerSpeed;
    [SerializeField] private HeroKnight playerCombat;
    [SerializeField] private PlayerRespawn playerRespawn;

    [Header("Upgrade Values")]
    [Header("3% Upgrade Movespeed")]
    [SerializeField] private float movespeedBoost = 1f;
    [Header("50% Upgrade Attack damage")]
    [SerializeField] private float damageBoost = 1f;
    [Header("Respawn boost")]
    [SerializeField] private int respawnBoost = 1;

    private void Awake()
    {
        gameObject.SetActive(false);
        if (!ValidateComponents())
        {
            Debug.LogError("Player component references missing in LevelUpSystem!");
            enabled = false;
            return;
        }
    }

    bool ValidateComponents()
    {
        return playerCombat != null &&
               playerRespawn != null;
    }

    public void ChoseUpgrade()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseUpgrade()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void LevelUpRespawn()
    {
        if (playerRespawn != null)
        {
            playerRespawn.IncreaseMaxDeaths(respawnBoost);
            CloseUpgrade();
        }
    }

    public void LevelUpAttack()
    {
        if (playerCombat != null)
        {
            playerCombat.IncreaseAttackDamage(damageBoost * (damageBoost * 0.5f));
            CloseUpgrade();
        }
    }

    public void LevelUpSPeed()
    {
        if (playerCombat != null)
        {
            playerCombat.IncreaseMoveSpeed(movespeedBoost + (movespeedBoost * 0.03f));
            CloseUpgrade();
        }
    }
}