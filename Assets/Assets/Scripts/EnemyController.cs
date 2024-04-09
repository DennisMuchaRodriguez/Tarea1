using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player; 
    public GameObject bulletPrefab; 
    public Transform projectileSpawnPoint;
    public float moveSpeed = 5f; 
    public float returnSpeed = 2f;
    public Transform initialPosition;
    public float detectionRadius = 5f; 
    public float fireRate = 1f; 
    public float bulletSpeed = 10f;

    private bool playerDetected = false; 
    private Vector2 initialPos; 
    private CircleCollider2D circleCollider; 
    private float fireTimer = 0f;
    public HealthBarController healthBarController;
    private void Start()
    {
        initialPos = transform.position;
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = detectionRadius;
    }

    private void Update()
    {
        if (playerDetected)
        {
           
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot(player.position);
                fireTimer = 0f;
            }
        }
        else
        {
           
            transform.position = Vector2.MoveTowards(transform.position, initialPos, returnSpeed * Time.deltaTime);
        }
    }

    private void Shoot(Vector2 targetPosition)
    {
        Vector2 shootingDirection = (targetPosition - (Vector2)transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            playerDetected = false;
        }
        else if (other.gameObject.tag == ("Bullet"))
        {
            healthBarController.UpdateHealth(10);
        }


    }       
        

}   
