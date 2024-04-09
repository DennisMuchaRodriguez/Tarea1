using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float directionY, directionX;
    [SerializeField] private Vector2 movementPlayer;
    [SerializeField] private GameObject bulletPrefab;
    private float bulletSpeed = 10f;
    public HealthBarController healthBarController;
    private void Start()
    {
       healthBarController = GetComponent<HealthBarController>();
    }
    private void Update()
    {
    
        movementPlayer = new Vector2(directionX, directionY);
        myRBD2.velocity = movementPlayer * velocityModifier;

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        Vector2 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckFlip(mouseInput.x);
    
        Debug.DrawRay(transform.position, mouseInput.normalized * rayDistance, Color.red);

        if(Input.GetMouseButtonDown(0)){
            Shoot(mouseInput);
            Debug.Log("Right Click");
        }else if(Input.GetMouseButtonDown(1)){
            Debug.Log("Left Click");
            Shoot(mouseInput);
        }
    }

    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
    private void FixedUpdate()
    {
        myRBD2.velocity = new Vector2(movementPlayer.x * velocityModifier, movementPlayer.y * velocityModifier);
        myRBD2.position = new Vector2(Mathf.Clamp(myRBD2.position.x,-18.25f,18.25f), Mathf.Clamp(myRBD2.position.y, -18.45f, 18.45f));

    }
    public void OnMovementX(InputAction.CallbackContext context)
    {
        directionX = context.ReadValue<float>();

    }
    public void OnMovementY(InputAction.CallbackContext context)
    {
        directionY = context.ReadValue<float>();

    }
    private void Shoot(Vector2 targetPosition)
    {
        Vector2 shootingDirection = (targetPosition - (Vector2)transform.position);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * bulletSpeed; 
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Bullet"))
        {
            healthBarController.UpdateHealth(10);

        }
        else if(collision.gameObject.tag == ("Enemy"))
        {
            healthBarController.UpdateHealth(10);

        }
    }
}
