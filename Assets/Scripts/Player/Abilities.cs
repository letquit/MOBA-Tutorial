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

    public Canvas ability1Canvas;
    public Image ability1Skillshot;
    
    [Header("Ability 2")]
    public Image abilityImage2;
    public TextMeshProUGUI abilityText2;
    public KeyCode ability2Key;
    public float ability2Cooldown = 7;
    
    public Canvas ability2Canvas;
    public Image ability2RangeIndicator;
    public float maxAbility2Distance = 7;
    
    [Header("Ability 3")]
    public Image abilityImage3;
    public TextMeshProUGUI abilityText3;
    public KeyCode ability3Key;
    public float ability3Cooldown = 10;
    
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
    
    private void Start()
    {
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

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Ability1Input();
        Ability2Input();
        Ability3Input();

        AbilityCooldown(ref currentAbility1Cooldown, ability1Cooldown, ref isAbility1Cooldown, abilityImage1, abilityText1);
        AbilityCooldown(ref currentAbility2Cooldown, ability2Cooldown, ref isAbility2Cooldown, abilityImage2, abilityText2);
        AbilityCooldown(ref currentAbility3Cooldown, ability3Cooldown, ref isAbility3Cooldown, abilityImage3, abilityText3);

        Ability1Canvas();
        Ability2Canvas();
        Ability3Canvas();
    }

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

    private void Ability1Input()
    {
        if (Input.GetKeyDown(ability1Key) && !isAbility1Cooldown)
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
            isAbility1Cooldown = true;
            currentAbility1Cooldown = ability1Cooldown;

            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
        }
    }

    private void Ability2Input()
    {
        if (Input.GetKeyDown(ability2Key) && !isAbility2Cooldown)
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
            isAbility2Cooldown = true;
            currentAbility2Cooldown = ability2Cooldown;

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;

            Cursor.visible = true;
        }
    }

    private void Ability3Input()
    {
        if (Input.GetKeyDown(ability3Key) && !isAbility3Cooldown)
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
            isAbility3Cooldown = true;
            currentAbility3Cooldown = ability3Cooldown;

            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;
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
