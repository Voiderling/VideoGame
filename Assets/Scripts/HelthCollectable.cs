using UnityEngine;

public class HelthCollectable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip itemSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            SoundManager.instance.PlaySound(itemSound);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
