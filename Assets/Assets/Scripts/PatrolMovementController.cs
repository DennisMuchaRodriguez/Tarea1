using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovementController : MonoBehaviour
{
    [SerializeField] private Transform[] checkpointsPatrol;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float normalSpeed = 5f; 
    [SerializeField] private float chaseSpeed = 10f;
    [SerializeField] private float chaseDistance = 5f; 
    [SerializeField] private LayerMask playerLayer; 

    private int currentPatrolIndex = 0;

    private void Start()
    {
        MoveToNextPatrolPoint();
    }

    private void Update()
    {
        
        if (IsPlayerInRange())
        {
          
            myRBD2.velocity = (checkpointsPatrol[currentPatrolIndex].position - transform.position).normalized * chaseSpeed;
        }
        else
        {
            
            myRBD2.velocity = (checkpointsPatrol[currentPatrolIndex].position - transform.position).normalized * normalSpeed;
        }

  
        if (Vector2.Distance(transform.position, checkpointsPatrol[currentPatrolIndex].position) < 0.1f)
        {
            MoveToNextPatrolPoint();
        }

        
        Vector2 raycastDirection = myRBD2.velocity.normalized; 
        Debug.DrawRay(transform.position, raycastDirection * chaseDistance, Color.red);
    }

    private void MoveToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % checkpointsPatrol.Length;
    }

    private bool IsPlayerInRange()
    {
    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, myRBD2.velocity.normalized, chaseDistance, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
           
            return true;
        }

        return false;
    }
}
