using Cinemachine;
using UnityEngine;

/// <summary>
/// 相机管理器类，用于控制虚拟相机和主相机之间的切换以及相机移动逻辑
/// </summary>
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cmVirtualCam;
    public Camera mainCamera;

    private bool usingVirtualCam = true;
    
    /// <summary>
    /// 每帧更新方法，处理相机切换和相机移动逻辑
    /// </summary>
    private void Update()
    {
        // 检测空格键按下，切换虚拟相机和主相机的使用状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            usingVirtualCam = !usingVirtualCam;
            
            if (usingVirtualCam)
            {
                cmVirtualCam.gameObject.SetActive(true);
                // mainCamera.gameObject.SetActive(false);
            }
            else
            {
                cmVirtualCam.gameObject.SetActive(false);
                mainCamera.gameObject.SetActive(true);
            }
        }

        // 当使用主相机时，根据鼠标位置控制相机移动
        if (!usingVirtualCam)
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

            // 检测鼠标是否接近屏幕边缘，实现边缘滚动效果
            if (x < 10)
            {
                mainCamera.transform.position -= Vector3.left * Time.deltaTime * 10;
            }
            else if (x > Screen.width - 10)
            {
                mainCamera.transform.position -= Vector3.right * Time.deltaTime * 10;
            }

            if (y < 10)
            {
                mainCamera.transform.position -= Vector3.back * Time.deltaTime * 10;
            }
            else if (y > Screen.height - 10)
            {
                mainCamera.transform.position -= Vector3.forward * Time.deltaTime * 10;
            }
        }
    }
}

