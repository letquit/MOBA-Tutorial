using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 控制角色移动和动画的类，使用NavMeshAgent进行路径寻路，并处理朝向旋转和动画参数设置。
/// </summary>
public class Movement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float rotateSpeedMovement = 0.05f;
    private float rotateVelocity;

    public Animator anim;
    private float motionSmoothTime = 0.1f;

    [Header("Enemy Targeting")]
    public GameObject targetEnemy;
    public float stoppingDistance;
    private HighlightManager hmScript;

    /// <summary>
    /// 初始化组件引用。
    /// </summary>
    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        hmScript = GetComponent<HighlightManager>();
    }
    
    /// <summary>
    /// 每帧更新动画和移动逻辑。
    /// </summary>
    private void Update()
    {
        Animation();
        Move();
    }
    
    /// <summary>
    /// 根据角色当前速度更新动画控制器中的Speed参数，用于控制动画混合树。
    /// </summary>
    public void Animation()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
    }

    /// <summary>
    /// 处理角色移动逻辑，包括鼠标右键点击地面或敌人时的移动与目标追踪。
    /// </summary>
    private void Move()
    {
        // 鼠标右键点击检测
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            // 发射射线检测点击位置
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                // 判断点击对象标签并执行相应操作
                if (hit.collider.CompareTag("Ground"))
                {
                    MoveToPosition(hit.point);
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    MoveTowardsEnemy(hit.collider.gameObject);
                }
            }
        }

        // 如果存在目标敌人，则持续追踪直到进入停止距离
        if (targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDistance)
            {
                agent.SetDestination(targetEnemy.transform.position);
            }
        }
    }

    /// <summary>
    /// 移动到指定的世界坐标点。
    /// </summary>
    /// <param name="position">要移动到的目标位置。</param>
    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
        agent.stoppingDistance = 0;
        
        Rotation(position);

        // 清除目标敌人状态
        if (targetEnemy != null)
        {
            hmScript.DeselectHighlight();
            targetEnemy = null;
        }
        else if (position.y >= 0.1f)
        {
            targetEnemy = null;
        }
    }

    /// <summary>
    /// 设置角色朝向并移动至指定敌人。
    /// </summary>
    /// <param name="enemy">要追踪的敌人游戏对象。</param>
    public void MoveTowardsEnemy(GameObject enemy)
    {
        targetEnemy = enemy;
        agent.SetDestination(targetEnemy.transform.position);
        agent.stoppingDistance = stoppingDistance;
        
        Rotation(targetEnemy.transform.position);
        hmScript.SelectedHighlight();
    }

    /// <summary>
    /// 平滑旋转角色使其面向指定位置。
    /// </summary>
    /// <param name="lookAtPosition">需要朝向的目标位置。</param>
    public void Rotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y,
            ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

}
