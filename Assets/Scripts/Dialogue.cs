using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;

    [SerializeField] private string[] speaker;
    [TextArea]
    [SerializeField] private string[] dialogueWords;
    [SerializeField] private Sprite[] portrait;

    private bool dialogueActivated;
    [SerializeField] private int step;
    private bool inConversation;

    private void Start()
    {
        dialogueCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueActivated)
        {
            if (!inConversation)
            {
                // Start conversation
                inConversation = true;
                dialogueCanvas.SetActive(true);
                ShowNextLine();
            }
            else
            {
                // Continue conversation
                ShowNextLine();
            }
        }
    }

    private void ShowNextLine()
    {
        if (step >= speaker.Length)
        {
            // End dialogue
            dialogueCanvas.SetActive(false);
            step = 0;
            inConversation = false;
            return;
        }

        // Show current line
        speakerText.text = speaker[step];
        dialogueText.text = dialogueWords[step];
        portraitImage.sprite = portrait[step];
        step++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueActivated = true;
            step = 0; // Reset dialogue when entering trigger
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueActivated = false;
            inConversation = false;
            dialogueCanvas.SetActive(false);
            step = 0; // Reset progress when leaving
        }
    }
}