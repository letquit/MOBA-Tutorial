using UnityEngine;

/// <summary>
/// 敌人死亡处理类，负责处理敌人死亡时的经验值奖励和对象销毁逻辑
/// </summary>
public class EnemyDeathHandler : MonoBehaviour
{
    /// <summary>
    /// 敌人死亡时给予玩家的经验值数量
    /// </summary>
    public int experienceValue = 50;

    /// <summary>
    /// 处理敌人死亡逻辑，向玩家发送经验值奖励消息并销毁当前游戏对象
    /// </summary>
    public void Die()
    {
        // 查找场景中的玩家对象
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 如果找到玩家对象，则向其发送获得经验值的消息
        if (player != null)
        {
            player.SendMessage("GainExperienceFromEnemy", experienceValue);
        }
        
        // 销毁当前敌人的游戏对象
        Destroy(gameObject);
    }
}

