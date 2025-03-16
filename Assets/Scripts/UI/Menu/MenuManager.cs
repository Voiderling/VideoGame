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
        if (currentPosition == 0)
        {
            //Start game
            SceneManager.LoadScene(PlayerPrefs.GetInt("level", 2));
        }
        else if (currentPosition == 1)
        {
            MenuSoundManager.instance.ChangeSoundVolume(0.2f);
        }
        else if (currentPosition == 2)
        {
            MenuSoundManager.instance.ChangeMusicVolume(0.2f);
            //Open Credits
        }
        else if (currentPosition == 3)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("HowToPlay", 1));
        }
        else if (currentPosition == 4)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        } else if (currentPosition == 10)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("MainMenu", 0));
        }
    }
}