using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private int nextLevel;
    [SerializeField] private string[] speaker;
    [TextArea]
    [SerializeField] private string[] dialogueWords;
    [SerializeField] private Sprite[] portrait;
    private bool dialogueActivated;
    [SerializeField] private int step;
    [SerializeField] private GameObject speakPrompt;
    private bool inConversation;
    private void Start()
    {
        speakPrompt.SetActive(false); // Initialize speak prompt as hidden
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
                speakPrompt.SetActive(false);
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
            StartCoroutine(changeScene());
            SceneManager.LoadScene(PlayerPrefs.GetInt("level3", nextLevel));
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
            speakPrompt.SetActive(true); // Show prompt when entering
            step = 0; // Reset dialogue when entering trigger
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueActivated = false;
            inConversation = false;
            if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
            speakPrompt.SetActive(false); // Hide prompt when leaving
            step = 0; // Reset progress when leaving
        }
    }
    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds(2);
    }
}