using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private Image XpProgressBar;

    [Header("Level Up System")]
    [SerializeField] private LevelUpSystem levelUp;

    [Header("Progression Settings")]
    [SerializeField] private int startingLevel = 1;
    [SerializeField] private float baseXPRequired = 100f;
    [SerializeField] private float xpMultiplier = 1.5f;

    private int currentLevel;
    private float currentXP;
    private float targetXP;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgression();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadProgression()
    {
        // Check if there's a saved progression
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", startingLevel);
            currentXP = PlayerPrefs.GetFloat("CurrentXP", 0f);
            targetXP = PlayerPrefs.GetFloat("TargetXP", baseXPRequired);
        }
        else
        {
            // First-time initialization
            InitializeProgression();
        }
        UpdateUI();
    }

    void InitializeProgression()
    {
        currentLevel = startingLevel;
        currentXP = 0f;
        targetXP = baseXPRequired;
        SaveProgression();
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        SaveProgression();
        UpdateUI();
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        if (currentXP >= targetXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP -= targetXP;
        currentLevel++;
        targetXP *= xpMultiplier;

        if (levelUp != null)
        {
            levelUp.ChoseUpgrade();
        }
        else
        {
            Debug.LogError("LevelUpSystem reference not set!");
        }

        SaveProgression();
        UpdateUI();
    }

    void SaveProgression()
    {
        // Save current progression to PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetFloat("CurrentXP", currentXP);
        PlayerPrefs.SetFloat("TargetXP", targetXP);
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        if (experienceText != null)
            experienceText.text = $"{currentXP:F0}/{targetXP:F0}";

        if (levelText != null)
            levelText.text = $"Level {currentLevel}";

        if (XpProgressBar != null)
            XpProgressBar.fillAmount = currentXP / targetXP;
    }

    // Optional: Method to reset progression if needed
    public void ResetProgression()
    {
        // Clear only relevant keys instead of DeleteAll
        PlayerPrefs.DeleteKey("CurrentLevel");
        PlayerPrefs.DeleteKey("CurrentXP");
        PlayerPrefs.DeleteKey("TargetXP");

        // Reset runtime values
        currentLevel = startingLevel;
        currentXP = 0f;
        targetXP = baseXPRequired;

        // Force save and update
        PlayerPrefs.Save();
        UpdateUI();

        Debug.Log("Progression reset complete");  // Add this for verification
    }
}