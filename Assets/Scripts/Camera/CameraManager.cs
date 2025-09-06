using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cmVirtualCam;
    public Camera mainCamera;

    private bool usingVirtualCam = true;
    
    private void Update()
    {
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

        if (!usingVirtualCam)
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

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
