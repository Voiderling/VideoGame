using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayScript : MonoBehaviour 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private RectTransform sword;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
            Interact();
    }
    public void Interact()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("MainMenu"), 0);
    }
}
