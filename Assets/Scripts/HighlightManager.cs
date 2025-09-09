using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 高亮管理器类，用于处理游戏对象的悬停高亮和选中高亮效果
/// </summary>
public class HighlightManager : MonoBehaviour
{
    private Transform highlightedObj;
    private Transform selectedObj;
    public LayerMask selectableLayer;

    private Outline highlightOutline;
    private RaycastHit hit;

    /// <summary>
    /// 每帧更新方法，调用悬停高亮检测
    /// </summary>
    private void Update()
    {
        HoverHighlight();
    }

    /// <summary>
    /// 处理鼠标悬停时的对象高亮效果
    /// 检测鼠标指向的对象，如果是可选择的敌人对象则显示高亮轮廓
    /// </summary>
    private void HoverHighlight()
    {
        // 取消之前的高亮效果
        if (highlightedObj != null)
        {
            highlightOutline.enabled = false;
            highlightedObj = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 检测鼠标是否在UI元素上，如果没有则进行射线检测
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, selectableLayer))
        {
            highlightedObj = hit.transform;

            // 只对标记为"Enemy"且未被选中的对象进行高亮处理
            if (highlightedObj.CompareTag("Enemy") && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>();
                highlightOutline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
    }
    
    /// <summary>
    /// 处理对象被选中时的高亮效果
    /// 将当前高亮的对象设置为选中状态，并更新轮廓显示
    /// </summary>
    public void SelectedHighlight()
    {
        if (highlightedObj.CompareTag("Enemy"))
        {
            // 取消之前选中对象的高亮效果
            if (selectedObj != null)
            {
                selectedObj.GetComponent<Outline>().enabled = false;
            }

            // 设置新的选中对象
            selectedObj = hit.transform;
            selectedObj.GetComponent<Outline>().enabled = true;

            highlightOutline.enabled = true;
            highlightedObj = null;
        }
    }

    /// <summary>
    /// 取消当前选中对象的高亮效果
    /// 关闭选中对象的轮廓显示并清空选中引用
    /// </summary>
    public void DeselectHighlight()
    {
        selectedObj.GetComponent<Outline>().enabled = false;
        selectedObj = null;
    }
}

