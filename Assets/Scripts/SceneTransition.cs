using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float delay = 4f;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,delay);
    }
}
