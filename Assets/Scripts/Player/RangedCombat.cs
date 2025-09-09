using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 远程攻击控制类，用于处理角色的远程攻击逻辑，包括攻击间隔、动画播放和投射物生成。
/// </summary>
public class RangedCombat : MonoBehaviour
{
    private Movement moveScript;
    private Stats stats;
    private Animator anim;

    [Header("Target")]
    public GameObject targetEnemy;
    
    [Header("Ranged Attack Variables")]
    public bool performRangedAttack = true;
    private float attackInterval;
    private float nextAttackTime = 0;

    [Header("Ranged Projectile Variables")] 
    public GameObject attackProjectile;
    public Transform attackSpawnPoint;
    private GameObject spawnedProjectile;

    /// <summary>
    /// 初始化组件引用，获取角色移动、属性和动画控制器组件。
    /// </summary>
    private void Start()
    {
        moveScript = GetComponent<Movement>();
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 每帧更新远程攻击逻辑，包括攻击间隔计算、目标检测和攻击执行。
    /// </summary>
    private void Update()
    {
        // 计算每次攻击的速度和间隔时间
        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);

        targetEnemy = moveScript.targetEnemy;

        // 如果在范围内，则执行远程攻击
        if (targetEnemy != null && performRangedAttack && Time.time > nextAttackTime)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= moveScript.stoppingDistance)
            {
                StartCoroutine(RangedAttackInterval());
            }
        }
    }
    
    /// <summary>
    /// 控制远程攻击的协程，处理攻击动画播放和攻击间隔等待。
    /// </summary>
    /// <returns>IEnumerator 用于协程执行</returns>
    private IEnumerator RangedAttackInterval()
    {
        performRangedAttack = false;

        // 触发攻击动画
        anim.SetBool("isAttacking", true);

        // 根据攻击速度/间隔值等待
        yield return new WaitForSeconds(attackInterval);

        // 检查敌人是否仍然存活。
        if (targetEnemy == null)
        {
            // 停止动画并允许再次攻击
            anim.SetBool("isAttacking", false);
            performRangedAttack = true;
        }
    }
    
    /// <summary>
    /// 在动画事件中调用，生成远程攻击投射物并设置目标，同时更新攻击状态和下一次攻击时间。
    /// </summary>
    private void RangedAttack()
    {
        spawnedProjectile = Instantiate(attackProjectile, attackSpawnPoint.transform.position, attackSpawnPoint.transform.rotation);
        
        TargetEnemy targetEnemyScript = spawnedProjectile.GetComponent<TargetEnemy>();

        if (targetEnemyScript != null)
        {
            targetEnemyScript.SetTarget(targetEnemy.transform);
        }

        // 设置下一次攻击时间
        nextAttackTime = Time.time + attackInterval;
        performRangedAttack = true;

        // 停止调用攻击动画
        anim.SetBool("isAttacking", false);
    }
}
