using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform sword;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private bool isTutorial = false;
    private int currentPosition;

    private void Awake()
    {
        ChangePosition(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
            Interact();
    }

    public void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            MenuSoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        AssignPosition();
    }
    private void AssignPosition()
    {
        sword.position = new Vector3(sword.position.x, buttons[currentPosition].position.y);
    }
    private void Interact()
    {
        MenuSoundManager.instance.PlaySound(interactSound);
        if (isTutorial) currentPosition = 10;

        if (currentPosition == 0) // New Game
        {
            // Only reset if XPManager exists
            if (XPManager.Instance != null)
            {
                XPManager.Instance.ResetProgression();
            }
            else
            {
                // First-time initialization
                PlayerPrefs.DeleteKey("CurrentLevel");
                PlayerPrefs.DeleteKey("CurrentXP");
                PlayerPrefs.DeleteKey("TargetXP");
            }

            Time.timeScale = 1f;
            SceneManager.LoadScene(2); // Load first level directly
        }
        else if (currentPosition == 1)
        {
            MenuSoundManager.instance.ChangeSoundVolume(0.2f);
        }
        else if (currentPosition == 2)
        {
            MenuSoundManager.instance.ChangeMusicVolume(0.2f);
        }
        else if (currentPosition == 3)
        {
            SceneManager.LoadScene(1); // HowToPlay directly
        }
        else if (currentPosition == 4)
        {
            Application.Quit();
        }
        else if (currentPosition == 10) // Return to Main Menu
        {
            if (XPManager.Instance != null)
            {
                XPManager.Instance.ResetProgression();
                Destroy(XPManager.Instance.gameObject);
            }
            SceneManager.LoadScene(0); // Main menu directly
        }
    }
}