using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 法力值管理系统，用于管理角色的法力值，包括法力值的消耗、恢复和UI显示
/// </summary>
public class ManaSystem : MonoBehaviour
{
    public float maxMana = 100;
    public float startingMana = 100;
    public float manaRegenRate = 5;

    public Slider manaBar2d;
    public Slider manaBar3d;
    public TextMeshProUGUI manaText2d;

    private float currentMana;

    /// <summary>
    /// 初始化法力值系统，设置初始法力值并更新UI显示
    /// </summary>
    private void Start()
    {
        currentMana = startingMana;
        UpdateManaUI();
    }

    /// <summary>
    /// 每帧更新法力值恢复
    /// </summary>
    private void Update()
    {
        RegenerateMana();
    }
    
    /// <summary>
    /// 持续恢复法力值，每帧根据恢复速率增加法力值，直到达到最大值
    /// </summary>
    private void RegenerateMana()
    {
        // 当前法力值未满时进行恢复
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
            UpdateManaUI();
        }
    }
    
    /// <summary>
    /// 更新所有法力值相关的UI元素，包括2D和3D进度条以及数值文本显示
    /// </summary>
    public void UpdateManaUI()
    {
        // 更新2D法力条进度
        if (manaBar2d != null)
        {
            manaBar2d.value = currentMana / maxMana;
        }

        // 更新3D法力条进度
        if (manaBar3d != null)
        {
            manaBar3d.value = currentMana / maxMana;
        }

        // 更新法力值文本显示
        if (manaText2d != null)
        {
            manaText2d.text = Mathf.RoundToInt(currentMana).ToString() + " / " + maxMana;
        }
    }
    
    /// <summary>
    /// 检查是否拥有足够的法力值来使用指定技能
    /// </summary>
    /// <param name="abilityCost">技能消耗的法力值</param>
    /// <returns>如果法力值足够返回true，否则返回false</returns>
    public bool CanAffordAbility(float abilityCost)
    {
        return currentMana >= abilityCost;
    }

    /// <summary>
    /// 使用技能并消耗相应的法力值
    /// </summary>
    /// <param name="abilityCost">技能消耗的法力值</param>
    public void UseAbility(float abilityCost)
    {
        currentMana -= abilityCost;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        UpdateManaUI();
    }
}

