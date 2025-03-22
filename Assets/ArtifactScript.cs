using UnityEngine;

public class ArtifactScript : MonoBehaviour
{
    [SerializeField] private GameObject artifactVisual; // Assign the child object
    [SerializeField] private EnemyHealth bossHealth; // Assign in Inspector
    [SerializeField] private GameObject textTrigger;
    private bool isInRange;

    void Start()
    {
        artifactVisual.SetActive(false);
        textTrigger?.SetActive(false);
    }

    void Update()
    {
        // Check if boss exists and is dead
        if (bossHealth.IsDead())
        {
            artifactVisual?.SetActive(true); // Force activate

            // Collect artifact when pressing E
            if (isInRange && Input.GetKeyDown(KeyCode.E))
            {
                artifactVisual.SetActive(false);
                textTrigger?.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && bossHealth.IsDead())
        {
            textTrigger?.SetActive(true);
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textTrigger?.SetActive(false);
            isInRange = false;
        }
    }
}