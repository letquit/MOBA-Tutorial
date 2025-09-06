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

    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
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
                if (hit.collider.tag == "Ground")
                {
                    // movement
                    agent.SetDestination(hit.point);
                    agent.stoppingDistance = 0;
                    
                    // rotation
                    Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y,
                        ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    
                    transform.rotation = Quaternion.Euler(0, rotationY, 0);
                }
            }
        }
    }

}
