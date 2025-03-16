using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private int maxDeaths = 2;
    private int deathCounter = 0;
    private EnemyPatrool patrol;
    private EnhancedSkeleton enemy;
    private Health playerHealth;
    private UiManager uiManager;

    [System.Obsolete]
    private void Start()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UiManager>();
    }

    public void Respawn()
    {
        deathCounter++;

        if (deathCounter > maxDeaths)
        {
            uiManager.GameOver();
            return;
        }
        Debug.Log("Death number: " + deathCounter);
        if (currentCheckpoint != null)
        {
            playerHealth.FullRespawn();
            transform.position = currentCheckpoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}