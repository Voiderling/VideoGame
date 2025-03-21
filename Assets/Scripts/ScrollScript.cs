using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject scrollCanvas;
    private bool isInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            trigger.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            trigger.SetActive(false);
        }
    }

    private void Update()
    {
        if (isInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ToggleScrollCanvas();
        }
    }

    public void ToggleScrollCanvas()
    {
        bool shouldActivate = !scrollCanvas.activeSelf;
        scrollCanvas.SetActive(shouldActivate);
        Time.timeScale = shouldActivate ? 0f : 1f;
    }
}