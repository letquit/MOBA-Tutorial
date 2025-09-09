using System.Collections;
using UnityEngine;

/// <summary>
/// 角色属性管理类，用于处理角色的生命值、伤害和攻击速度等基础属性，
/// 并负责处理受伤逻辑以及更新UI显示。
/// </summary>
public class Stats : MonoBehaviour
{
    [Header("基础属性")]
    public float health;           // 最大生命值
    public float damage;           // 攻击伤害
    public float attackSpeed;      // 攻击速度

    // 生命条滑块变量
    public float damageLerpDuration;   // 受伤时血条平滑过渡的持续时间
    private float currentHealth;       // 当前显示的生命值（用于UI平滑过渡）
    private float targetHealth;        // 目标生命值（实际生命值）
    private Coroutine damageCoroutine; // 血条平滑过渡协程引用

    HealthUI healthUI; // 引用 HealthUI 组件，用于更新2D和3D血条

    /// <summary>
    /// 初始化组件引用和初始生命值设置。
    /// </summary>
    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();

        currentHealth = health;
        targetHealth = health;

        healthUI.Start3DSlider(health);
        healthUI.Update2DSlider(health, currentHealth);
    }

    /// <summary>
    /// 仅用于测试：按下 V 键时对自身造成一次伤害。
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(gameObject, damage);
        }
    }

    /// <summary>
    /// 对目标造成指定数值的伤害，并根据目标标签执行不同的死亡处理逻辑。
    /// </summary>
    /// <param name="target">受到伤害的目标 GameObject。</param>
    /// <param name="damageAmount">造成的伤害数值。</param>
    public void TakeDamage(GameObject target, float damageAmount)
    {
        Stats targetStats = target.GetComponent<Stats>();

        targetStats.targetHealth -= damageAmount;

        // 如果是玩家且生命值小于等于0，则销毁对象并调用玩家死亡处理
        if (target.CompareTag("Player") && targetStats.targetHealth <= 0)
        {
            Destroy(target.gameObject);
            CheckIfPlayerDead();
        }
        // 如果不是玩家但血条协程未运行，则启动血条平滑过渡
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }

        // 如果是敌人且生命值小于等于0，则尝试调用敌人死亡处理方法
        if (target.CompareTag("Enemy") && targetStats.targetHealth <= 0)
        {
            EnemyDeathHandler enemyDeathHandler = target.GetComponent<EnemyDeathHandler>();

            if (enemyDeathHandler != null)
            {
                enemyDeathHandler.Die();
            }
        }
        // 如果不是敌人但血条协程未运行，则启动血条平滑过渡
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    /// <summary>
    /// 处理玩家死亡时的UI更新逻辑。
    /// </summary>
    private void CheckIfPlayerDead()
    {
        healthUI.Update2DSlider(health, 0);
    }
    
    /// <summary>
    /// 启动血条平滑过渡协程。
    /// </summary>
    private void StartLerpHealth()
    {
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    /// <summary>
    /// 使用插值方式平滑更新当前显示的生命值，并在过程中持续更新UI。
    /// </summary>
    /// <returns>IEnumerator，支持协程操作。</returns>
    private IEnumerator LerpHealth()
    {
        float elapsedTime = 0;
        float initialHealth = currentHealth;
        float target = targetHealth;

        while (elapsedTime < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(initialHealth, target, elapsedTime / damageLerpDuration);
            UpdateHealthUI();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentHealth = target;
        UpdateHealthUI();

        damageCoroutine = null;
    }

    /// <summary>
    /// 更新2D和3D血条UI显示。
    /// </summary>
    private void UpdateHealthUI()
    {
        healthUI.Update2DSlider(health, currentHealth);
        healthUI.Update3DSlider(currentHealth);
    }
}
