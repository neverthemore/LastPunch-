using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public int health = 30;
    public Camera mainCamera;

    private Transform target;
    private Rigidbody rb;
    private Animator animator; // Reference to Animator
    private bool isStunned = false;

    public int headDamage = 15;
    public int bodyDamage = 10;
    public int legDamage = 5;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }

        animator = GetComponent<Animator>(); // Initialize animator

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void FixedUpdate()
    {
        if (health <= 0)
        {
            Die();
        }

        if (!isStunned) // Only move if not stunned
        {
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToTarget <= detectionRange)
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }

        FaceCamera();
    }

    private void FaceCamera()
    {
        if (mainCamera != null)
        {
            Vector3 cameraDirection = mainCamera.transform.position - transform.position;
            cameraDirection.y = 0;

            if (cameraDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-cameraDirection);
            }
        }
    }
    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true; // Freeze the enemy
        rb.velocity = Vector3.zero; // Stop movement
        yield return new WaitForSeconds(duration); // Wait for the stun duration
        isStunned = false; // Unfreeze the enemy
    }
    public void TakeDamage(int attackDamage, HitType hitType)
    {
        health -= attackDamage;
        Debug.Log($"Enemy took {attackDamage} damage from {hitType}! Remaining health: {health}");

        if (hitType == HitType.Head)
        {
            animator.SetInteger("HitType", 1); // 1 for head hit
        }
        else if (hitType == HitType.Body)
        {
            animator.SetInteger("HitType", 2); // 2 for body hit
        }
        else if (hitType == HitType.Legs)
        {
            animator.SetInteger("HitType", 3); // 3 for leg hit
        }

        // Check if health is less than or equal to 0
        if (health <= 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        // Optionally, play a death animation here
        Destroy(gameObject);
    }

   

    internal void Setsortinglayer(string v)
    {
        throw new System.NotImplementedException();
    }
}

public enum HitType
{
    Head,
    Body,
    Legs
}