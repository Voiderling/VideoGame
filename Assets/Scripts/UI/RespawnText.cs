using UnityEngine;
using TMPro;

public class RespawnText : MonoBehaviour
{
    [SerializeField] private PlayerRespawn respawnsAvailable;
    [SerializeField] private string textIntro = "Respawns: ";
    [SerializeField] private TMP_Text txt; // Assign in Inspector

    void Start()
    {
        // Initialize TMP_Text if not assigned in Inspector
        if (txt == null)
            txt = GetComponent<TMP_Text>();

        // Find PlayerRespawn if not assigned in Inspector
        if (respawnsAvailable == null)
            respawnsAvailable = FindObjectOfType<PlayerRespawn>();
    }

    void Update()
    {
        UpdateRespawnsAvailable();
    }

    private void UpdateRespawnsAvailable()
    {
        if (respawnsAvailable != null && txt != null)
        {
            int deathCounter = respawnsAvailable.GetRespawnCounter();
            if (respawnsAvailable.GetRespawnCounter() < 0) {
                gameObject.SetActive(false);
            }
            txt.text = textIntro + deathCounter.ToString();
        }
    }
}