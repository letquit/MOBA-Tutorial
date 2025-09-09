using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 健康值UI控制器类，用于管理3D和2D血条的显示
/// </summary>
public class HealthUI : MonoBehaviour
{
    public Slider healthSlider3D;
    public Slider healthSlider2D;

    /// <summary>
    /// 初始化3D血条滑动条的最大值并设置当前值
    /// </summary>
    /// <param name="maxValue">血条的最大值</param>
    public void Start3DSlider(float maxValue)
    {
        healthSlider3D.maxValue = maxValue;
        healthSlider3D.value = maxValue;
    }

    /// <summary>
    /// 更新3D血条滑动条的当前值
    /// </summary>
    /// <param name="value">血条的当前值</param>
    public void Update3DSlider(float value)
    {
        healthSlider3D.value = value;
    }

    /// <summary>
    /// 更新2D血条滑动条的最大值和当前值，仅对玩家对象生效
    /// </summary>
    /// <param name="maxValue">血条的最大值</param>
    /// <param name="value">血条的当前值</param>
    public void Update2DSlider(float maxValue, float value)
    {
        // 2D = 仅限玩家
        if (gameObject.CompareTag("Player"))
        {
            healthSlider2D.maxValue = maxValue;
            healthSlider2D.value = value;
        }
    }
}

