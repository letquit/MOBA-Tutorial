using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理角色的三种技能逻辑，包括冷却、法力消耗、UI显示和输入响应。
/// </summary>
public class Abilities : MonoBehaviour
{
    [Header("Ability 1")] 
    public Image abilityImage1;
    public TextMeshProUGUI abilityText1;
    public KeyCode ability1Key;
    public float ability1Cooldown = 5;

    public float ability1ManaCost = 30;
    public Canvas ability1Canvas;
    public Image ability1Skillshot;
    
    [Header("Ability 2")]
    public Image abilityImage2;
    public TextMeshProUGUI abilityText2;
    public KeyCode ability2Key;
    public float ability2Cooldown = 7;
    
    public float ability2ManaCost = 30;
    public Canvas ability2Canvas;
    public Image ability2RangeIndicator;
    public float maxAbility2Distance = 7;
    
    [Header("Ability 3")]
    public Image abilityImage3;
    public TextMeshProUGUI abilityText3;
    public KeyCode ability3Key;
    public float ability3Cooldown = 10;
    
    public float ability3ManaCost = 30;
    public Canvas ability3Canvas;
    public Image ability3Cone;
    
    private bool isAbility1Cooldown = false;
    private bool isAbility2Cooldown = false;
    private bool isAbility3Cooldown = false;
    
    private float currentAbility1Cooldown;
    private float currentAbility2Cooldown;
    private float currentAbility3Cooldown;

    private Vector3 position;
    private RaycastHit hit;
    private Ray ray;
    
    public ManaSystem manaSystem;
    
    /// <summary>
    /// 初始化组件引用和UI状态。
    /// </summary>
    private void Start()
    {
        manaSystem = GetComponent<ManaSystem>();
        
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;

        abilityText1.text = "";
        abilityText2.text = "";
        abilityText3.text = "";

        ability1Skillshot.enabled = false;
        ability2RangeIndicator.enabled = false;
        ability3Cone.enabled = false;
        
        ability1Canvas.enabled = false;
        ability2Canvas.enabled = false;
        ability3Canvas.enabled = false;
    }

    /// <summary>
    /// 每帧更新技能输入、冷却状态和UI显示。
    /// </summary>
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Ability1Input();
        Ability2Input();
        Ability3Input();

        AbilityCooldown(ability1Cooldown, ability1ManaCost, ref currentAbility1Cooldown, ref isAbility1Cooldown, abilityImage1, abilityText1);
        AbilityCooldown(ability2Cooldown, ability2ManaCost, ref currentAbility2Cooldown, ref isAbility2Cooldown, abilityImage2, abilityText2);
        AbilityCooldown(ability3Cooldown, ability3ManaCost, ref currentAbility3Cooldown, ref isAbility3Cooldown,
            abilityImage3, abilityText3);

        Ability1Canvas();
        Ability2Canvas();
        Ability3Canvas();
    }

    /// <summary>
    /// 更新技能1的画布朝向，使其始终面向鼠标位置。
    /// </summary>
    private void Ability1Canvas()
    {
        if (ability1Skillshot.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab1Canvas = Quaternion.LookRotation(position - transform.position);
            ab1Canvas.eulerAngles = new Vector3(0, ab1Canvas.eulerAngles.y, ab1Canvas.eulerAngles.z);

            ability1Canvas.transform.rotation = Quaternion.Lerp(ab1Canvas, ability1Canvas.transform.rotation, 0);
        }
    }

    /// <summary>
    /// 更新技能2的画布位置，限制最大距离并避开玩家自身。
    /// </summary>
    private void Ability2Canvas()
    {
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                position = hit.point;
            }
        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility2Distance);

        var newHitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = (newHitPos);
    }

    /// <summary>
    /// 更新技能3的画布朝向，使其始终面向鼠标位置。
    /// </summary>
    private void Ability3Canvas()
    {
        if (ability3Cone.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab3Canvas = Quaternion.LookRotation(position - transform.position);
            ab3Canvas.eulerAngles = new Vector3(0, ab3Canvas.eulerAngles.y, ab3Canvas.eulerAngles.z);

            ability3Canvas.transform.rotation = Quaternion.Lerp(ab3Canvas, ability3Canvas.transform.rotation, 0);
        }
    }

    /// <summary>
    /// 处理技能1的输入逻辑，包括激活技能画布和释放技能。
    /// </summary>
    private void Ability1Input()
    {
        if (Input.GetKeyDown(ability1Key) && !isAbility1Cooldown && manaSystem.CanAffordAbility(ability1ManaCost))
        {
            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;
            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;
            
            Cursor.visible = true;
        }
        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
        {
            if (manaSystem.CanAffordAbility(ability1ManaCost))
            {
                manaSystem.UseAbility(ability1ManaCost);
                isAbility1Cooldown = true;
                currentAbility1Cooldown = ability1Cooldown;

                ability1Canvas.enabled = false;
                ability1Skillshot.enabled = false;
            }
        }
    }

    /// <summary>
    /// 处理技能2的输入逻辑，包括激活技能画布和释放技能。
    /// </summary>
    private void Ability2Input()
    {
        if (Input.GetKeyDown(ability2Key) && !isAbility2Cooldown && manaSystem.CanAffordAbility(ability2ManaCost))
        {
            ability2Canvas.enabled = true;
            ability2RangeIndicator.enabled = true;

            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;

            Cursor.visible = false;
        }
        if (ability2Canvas.enabled && Input.GetMouseButtonDown(0))
        {
            if (manaSystem.CanAffordAbility(ability2ManaCost))
            {
                manaSystem.UseAbility(ability2ManaCost);
                
                isAbility2Cooldown = true;
                currentAbility2Cooldown = ability2Cooldown;

                ability2Canvas.enabled = false;
                ability2RangeIndicator.enabled = false;

                Cursor.visible = true;
            }
        }
    }

    /// <summary>
    /// 处理技能3的输入逻辑，包括激活技能画布和释放技能。
    /// </summary>
    private void Ability3Input()
    {
        if (Input.GetKeyDown(ability3Key) && !isAbility3Cooldown && manaSystem.CanAffordAbility(ability3ManaCost))
        {
            ability3Canvas.enabled = true;
            ability3Cone.enabled = true;

            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;

            Cursor.visible = true;
        }
        if (ability3Cone.enabled && Input.GetMouseButtonDown(0))
        {
            if (manaSystem.CanAffordAbility(ability3ManaCost))
            {
                manaSystem.UseAbility(ability3ManaCost);
                
                isAbility3Cooldown = true;
                currentAbility3Cooldown = ability3Cooldown;

                ability3Canvas.enabled = false;
                ability3Cone.enabled = false;
            }
        }
    }
    
    /// <summary>
    /// 更新技能冷却状态和UI显示。
    /// </summary>
    /// <param name="abilityCooldown">技能的总冷却时间。</param>
    /// <param name="abilityManaCost">技能的法力消耗。</param>
    /// <param name="currentCooldown">当前剩余冷却时间的引用。</param>
    /// <param name="isCooldown">是否处于冷却状态的引用。</param>
    /// <param name="skillImage">技能图标图像。</param>
    /// <param name="skillText">技能冷却时间文本。</param>
    private void AbilityCooldown(float abilityCooldown, float abilityManaCost, ref float currentCooldown, ref bool isCooldown, Image skillImage, TextMeshProUGUI skillText)
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0;
            }

            if (skillImage != null)
            {
                skillImage.color = Color.grey;
                skillImage.fillAmount = currentCooldown / abilityCooldown;
            }

            if (skillText != null)
            {
                skillText.text = Mathf.Ceil(currentCooldown).ToString();
            }
        }
        else
        {
            if (manaSystem.CanAffordAbility(abilityManaCost))
            {
                if (skillImage != null)
                {
                    skillImage.color = Color.grey;
                    skillImage.fillAmount = 0;
                }

                if (skillText != null)
                {
                    skillText.text = " ";
                }
            }
            else
            {
                if (skillImage != null)
                {
                    skillImage.color = Color.red;
                    skillImage.fillAmount = 1;
                }

                if (skillText != null)
                {
                    skillText.text = "X";
                }
            }
        }
    }
}
