using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 经验系统类，用于管理角色等级、经验值和相关UI显示
/// </summary>
public class ExperienceSystem : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToLevelUp = 100;
    public int expIncreaseFactor = 2;

    public Slider expBarSlider;
    public TextMeshProUGUI levelText2D;
    public TextMeshProUGUI levelText3D;
    
    /// <summary>
    /// 每帧更新UI显示
    /// </summary>
    private void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// 从敌人获得经验值
    /// </summary>
    /// <param name="amount">获得的经验值数量</param>
    private void GainExperienceFromEnemy(int amount)
    {
        GainExperience(amount);
    }

    /// <summary>
    /// 获得经验值并处理升级逻辑
    /// </summary>
    /// <param name="amount">获得的经验值数量</param>
    private void GainExperience(int amount)
    {
        currentExp += amount;

        // 检查是否满足升级条件，如果满足则升级
        while (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 角色升级处理
    /// </summary>
    private void LevelUp()
    {
        currentLevel++;
        currentExp -= expToLevelUp;
        expToLevelUp *= expIncreaseFactor;
    }

    /// <summary>
    /// 更新经验条和等级显示UI
    /// </summary>
    private void UpdateUI()
    {
        // 更新经验条显示
        if (expBarSlider != null)
        {
            expBarSlider.maxValue = expToLevelUp;
            expBarSlider.value = currentExp;
        }
        
        // 更新2D等级文本显示
        if (levelText2D != null)
        {
            levelText2D.text = currentLevel.ToString();
        }
        
        // 更新3D等级文本显示
        if (levelText3D != null)
        {
            levelText3D.text = currentLevel.ToString();
        }
    }
}

