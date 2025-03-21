using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorExitScirpt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject textTrigger;
    private bool isInRange;
    [SerializeField] private bool isExitingLevel;
    [SerializeField] private Transform knightPos;
    [SerializeField] private Transform checkpointPos;
    [SerializeField] private int levelIndex;
    void Start()
    {
        isInRange = false;
        textTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && isExitingLevel)
        {
            ExitLevel();
        }
        else if (isInRange && Input.GetKeyDown(KeyCode.E) && !isExitingLevel) {
            MoveToPoint();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            textTrigger.SetActive(true);
            isInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textTrigger.SetActive(false);
            isInRange = false;
        }
    }
    private void ExitLevel()
    {
        SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("Level2", levelIndex));
    }
    private void MoveToPoint()
    {
        knightPos.position = checkpointPos.position;
    }
}
