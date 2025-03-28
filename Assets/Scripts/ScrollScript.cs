using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject scrollCanvas;
    [SerializeField] private GameObject triggerObject;
    private bool isInRange;
    private int count;
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

        if (isInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ToggleScrollCanvas();
        }
    }

    public void ToggleScrollCanvas()
    {
        bool shouldActivate = !scrollCanvas.activeSelf;
        scrollCanvas.SetActive(shouldActivate);
        Time.timeScale = shouldActivate ? 0f : 1f;
        count++;
        if (count > 1)
        {
            Time.timeScale = 1f;
            triggerObject.SetActive(false);
            return;
        }
    }
}