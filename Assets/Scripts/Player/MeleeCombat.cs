using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement)), RequireComponent(typeof(Stats))]
public class MeleeCombat : MonoBehaviour
{
    private Movement moveScript;
    private Stats stats;
    private Animator anim;

    [Header("Target")]
    public GameObject targetEnemy;

    [Header("Melee Attack Variables")]
    public bool performMeleeAttack = true;
    private float attackInterval;
    private float nextAttackTime = 0;

    private void Start()
    {
        moveScript = GetComponent<Movement>();
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 计算每次攻击的速度和间隔时间
        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);

        targetEnemy = moveScript.targetEnemy;

        // 如果在范围内，则执行近战攻击
        if (targetEnemy != null && performMeleeAttack && Time.time > nextAttackTime)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= moveScript.stoppingDistance)
            {
                StartCoroutine(MeleeAttackInterval());
            }
        }
    }
    
    private IEnumerator MeleeAttackInterval()
    {
        performMeleeAttack = false;

        // 触发攻击动画
        anim.SetBool("isAttacking", true);

        // 根据攻击速度/间隔值等待
        yield return new WaitForSeconds(attackInterval);

        // 检查敌人是否仍然存活。
        if (targetEnemy == null)
        {
            // 停止动画并允许再次攻击
            anim.SetBool("isAttacking", false);
            performMeleeAttack = true;
        }
    }
    
    // 在动画事件中调用
    private void MeleeAttack()
    {
        stats.TakeDamage(targetEnemy, stats.damage);

        // 设置下一次攻击时间
        nextAttackTime = Time.time + attackInterval;
        performMeleeAttack = true;

        // 停止调用攻击动画
        anim.SetBool("isAttacking", false);
    }
}
