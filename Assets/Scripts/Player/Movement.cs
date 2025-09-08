using System;
using UnityEngine;
using UnityEngine.AI;

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

    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        hmScript = GetComponent<HighlightManager>();
    }
    
    private void Update()
    {
        Animation();
        Move();
    }
    
    public void Animation()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
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

        if (targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDistance)
            {
                agent.SetDestination(targetEnemy.transform.position);
            }
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
        agent.stoppingDistance = 0;
        
        Rotation(position);

        if (targetEnemy != null)
        {
            hmScript.DeselectHighlight();
            targetEnemy = null;
        }
    }

    public void MoveTowardsEnemy(GameObject enemy)
    {
        targetEnemy = enemy;
        agent.SetDestination(targetEnemy.transform.position);
        agent.stoppingDistance = stoppingDistance;
        
        Rotation(targetEnemy.transform.position);
        hmScript.SelectedHighlight();
    }

    public void Rotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y,
            ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

}
