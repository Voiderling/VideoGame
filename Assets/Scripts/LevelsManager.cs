using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class LevelsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("References")]
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text xpText;
    [SerializeField] Image xpBar;

    [Space(10)]
    [Header("Settings")]
    [SerializeField] int targetXp = 100;
    [SerializeField] int targetXPIncrease = 25;
    [SerializeField]
    private int currentLevel;
    private int currentXP;

    void Start()
    {
        currentLevel = 1;
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreaseXP(int amount)
    {
        currentXP += amount;
        CheckForLevelUp();
    }
    private void CheckForLevelUp()
    {
        while(currentXP >= targetXp)
        {
            currentLevel++;
            currentXP -= targetXp;
            targetXp += targetXPIncrease;
        }
    }
    private void UpdateHUD()
    {
        currentLevelText.text = "Level " + currentLevel;
        xpText.text = currentXP + "/" + targetXp;
        xpBar.fillAmount = (float) currentXP / (float)targetXp;
    }
}
