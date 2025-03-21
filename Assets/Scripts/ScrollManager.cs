using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject scrollCanvas;
    void Start()
    {
        scrollCanvas.SetActive(false);
    }
    public void StopInteract()
    {
      scrollCanvas.SetActive(false);  
      Time.timeScale = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
