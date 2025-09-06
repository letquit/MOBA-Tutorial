using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Ability 1")] 
    public Image abilityImage1;
    public TextMeshProUGUI abilityText1;
    public KeyCode ability1Key;
    public float ability1Cooldown = 5;
    
    [Header("Ability 2")]
    public Image abilityImage2;
    public TextMeshProUGUI abilityText2;
    public KeyCode ability2Key;
    public float ability2Cooldown = 7;
    
    [Header("Ability 3")]
    public Image abilityImage3;
    public TextMeshProUGUI abilityText3;
    public KeyCode ability3Key;
    public float ability3Cooldown = 10;
    
    private bool isAbility1Cooldown = false;
    private bool isAbility2Cooldown = false;
    private bool isAbility3Cooldown = false;
    
    private float currentAbility1Cooldown;
    private float currentAbility2Cooldown;
    private float currentAbility3Cooldown;
    
    private void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;

        abilityText1.text = "";
        abilityText2.text = "";
        abilityText3.text = "";
    }

    private void Update()
    {
        Ability1Input();
        Ability2Input();
        Ability3Input();

        AbilityCooldown(ref currentAbility1Cooldown, ability1Cooldown, ref isAbility1Cooldown, abilityImage1, abilityText1);
        AbilityCooldown(ref currentAbility2Cooldown, ability2Cooldown, ref isAbility2Cooldown, abilityImage2, abilityText2);
        AbilityCooldown(ref currentAbility3Cooldown, ability3Cooldown, ref isAbility3Cooldown, abilityImage3, abilityText3);
    }
    
    private void Ability1Input()
    {
        // 如果按下技能1的键并且技能1不在冷却中。
        if (Input.GetKeyDown(ability1Key) && !isAbility1Cooldown)
        {
            isAbility1Cooldown = true;
            currentAbility1Cooldown = ability1Cooldown;
        }
    }

    private void Ability2Input()
    {
        // 如果按下技能2的键并且技能2不在冷却中。
        if (Input.GetKeyDown(ability2Key) && !isAbility2Cooldown)
        {
            isAbility2Cooldown = true;
            currentAbility2Cooldown = ability2Cooldown;
        }
    }

    private void Ability3Input()
    {
        // 如果按下技能3的键并且技能3不在冷却中。
        if (Input.GetKeyDown(ability3Key) && !isAbility3Cooldown)
        {
            isAbility3Cooldown = true;
            currentAbility3Cooldown = ability3Cooldown;
        }
    }
    
    private void AbilityCooldown(ref float currentCooldown, float maxCooldown, ref bool isCooldown, Image skillImage, TextMeshProUGUI skillText)
    {
        // 如果技能处于冷却状态。
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            // 如果当前冷却时间小于等于0。
            if (currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0f;

                // 如果技能图像不为空，重置填充量。
                if (skillImage != null)
                {
                    skillImage.fillAmount = 0f;
                }
                // 如果技能文本不为空，清空文本。
                if (skillText != null)
                {
                    skillText.text = "";
                }
            }
            else
            {
                // 如果技能图像不为空，更新填充量为当前冷却时间与最大冷却时间的比值。
                if (skillImage != null)
                {
                    skillImage.fillAmount = currentCooldown / maxCooldown;
                }
                // 如果技能文本不为空，设置文本为当前冷却时间。
                if (skillText != null)
                {
                    skillText.text = Mathf.Ceil(currentCooldown).ToString();
                }
            }
        }
    }
}
