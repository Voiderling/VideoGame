using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    [SerializeField] private SpriteRenderer flagSprite;
    [SerializeField] private Color activeColor;
    private bool isActive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            flagSprite.color = activeColor;
            isActive = true;
            // Add sound effect here if needed
        }
    }
}