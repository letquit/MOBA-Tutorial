using System;
using UnityEngine;

/// <summary>
/// 在启动时禁用Outline组件的 MonoBehaviour 类
/// 该类用于在游戏对象启动后短暂延迟禁用Outline组件
/// </summary>
public class DisableOutlineOnStart : MonoBehaviour
{
    private float delay = 0.01f;
    private Outline outline;

    /// <summary>
    /// Unity生命周期函数，在对象启用时调用
    /// 获取当前对象的Outline组件，并设置延迟调用DisableOutline方法
    /// </summary>
    private void Start()
    {
        outline = GetComponent<Outline>();
        Invoke(nameof(DisableOutline), delay);
    }
    
    /// <summary>
    /// 禁用Outline组件
    /// 将outline组件的enabled属性设置为false
    /// </summary>
    private void DisableOutline()
    {
        outline.enabled = false;
    }
}

