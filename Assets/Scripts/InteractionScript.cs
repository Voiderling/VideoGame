using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class InteractableDoor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("UI Elements")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private Image loadingScreen;
    [SerializeField] private float fadeDuration = 1f;

    private bool playerInRange = false;
    private bool isInteracting = false;

    private void Update()
    {
        if (playerInRange && !isInteracting)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                StartCoroutine(LoadNextScene());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdateUI(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UpdateUI(false);
        }
    }

    private void UpdateUI(bool show)
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(show);
            if (show) interactionText.text = $"Press {interactionKey} to enter";
        }
    }

    private IEnumerator LoadNextScene()
    {
        isInteracting = true;

        // Fade out
        if (loadingScreen != null)
        {
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                loadingScreen.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncLoad.isDone)
        {
            // Add loading progress logic here if needed
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}