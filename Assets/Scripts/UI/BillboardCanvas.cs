using System;
using UnityEngine;

/// <summary>
/// BillboardCanvas类用于创建始终面向摄像机的画布效果
/// 该组件会使游戏对象始终朝向主摄像机，常用于UI元素或标签的显示
/// </summary>
public class BillboardCanvas : MonoBehaviour
{
    private Transform cameraTransform;

    /// <summary>
    /// Start方法在组件启用时调用一次
    /// 获取主摄像机的变换组件引用，用于后续的朝向计算
    /// </summary>
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    /// <summary>
    /// LateUpdate方法在每帧的最后调用
    /// 更新游戏对象的朝向，使其始终面向摄像机
    /// 使用LateUpdate确保在所有Update方法执行完毕后进行朝向调整
    /// </summary>
    private void LateUpdate()
    {
        // 计算并设置对象朝向，使其始终面向摄像机
        // 使用摄像机的旋转信息来确定正确的朝向和上方向量
        transform.LookAt(transform.position + cameraTransform.rotation * -Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}

