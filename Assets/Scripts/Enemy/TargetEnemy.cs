using UnityEngine;

/// <summary>
/// 目标敌人控制类，用于控制投射物追踪并攻击目标敌人
/// </summary>
public class TargetEnemy : MonoBehaviour
{
    public Transform target;
    private Transform originalTarget;
    private Rigidbody theRB;

    private Stats playerStats;

    public float projectileSpeed;
    
    /// <summary>
    /// 初始化投射物，获取初始目标、玩家状态组件和刚体组件
    /// </summary>
    private void Start()
    {
        originalTarget = target;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        theRB = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// 每帧更新投射物的运动方向，朝向当前目标或原始目标移动
    /// 如果没有有效目标则销毁投射物
    /// </summary>
    private void Update()
    {
        // 优先追踪当前目标
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            theRB.linearVelocity = direction.normalized * projectileSpeed;
        }
        // 当前目标为空时追踪原始目标
        else if (originalTarget != null)
        {
            Vector3 direction = originalTarget.position - transform.position;
            theRB.linearVelocity = direction.normalized * projectileSpeed;
        }
        // 无任何有效目标时销毁投射物
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 设置新的追踪目标
    /// </summary>
    /// <param name="newTarget">新的目标Transform组件</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    /// <summary>
    /// 处理投射物与目标的碰撞检测，对目标造成伤害并销毁投射物
    /// </summary>
    /// <param name="other">碰撞到的Collider组件</param>
    private void OnTriggerEnter(Collider other)
    {
        // 检测是否碰撞到当前目标
        if (target != null && ReferenceEquals(other.gameObject, target.gameObject))
        {
            Stats targetStats = target.gameObject.GetComponent<Stats>();
            targetStats?.TakeDamage(target.gameObject, playerStats.damage);
            Destroy(gameObject);
        }
        // 检测是否碰撞到原始目标
        else if (originalTarget != null && ReferenceEquals(other.gameObject, originalTarget.gameObject))
        {
            Stats originalTargetStats = originalTarget.gameObject.GetComponent<Stats>();
            originalTargetStats?.TakeDamage(originalTarget.gameObject, playerStats.damage);
            Destroy(gameObject);
        }
    }
}
