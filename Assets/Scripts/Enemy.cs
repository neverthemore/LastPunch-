using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public int health = 30;
    public Camera mainCamera;
    public float attackDamage = 5;
    public float attackCooldown = 2f;

    public float pushForce = 5f;
    public float pushDuration = 2f;
    public float stunDuration = 2f;

    private Transform target;
    private Rigidbody rb;
    private Animator animator;
    private bool isStunned = false;
    private bool canAttack = true;
    private bool door = false;

    public int headDamage = 15;
    public int bodyDamage = 10;
    public int legDamage = 5;

    void Start()
    {
        if (gameObject.CompareTag("Door"))
            door = true;
        else door = false;

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

        animator = GetComponentInChildren<Animator>();

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

        if (!door)
        {
            if (!isStunned) // Движение только если не в состоянии "стан"
            {
                MoveTowardsPlayer();
                animator.SetBool("walkEnemy", true);
            }

            FaceCamera();
        }
    }
    public void PushBack(Vector3 direction)
    {
        rb.AddForce(direction.normalized * pushForce, ForceMode.Impulse);
        StartCoroutine(StopAfterPush(pushDuration));
    }
    private IEnumerator StopAfterPush(float pushDuration)
    {
        yield return new WaitForSeconds(pushDuration); // Wait for the specified duration
        rb.velocity = Vector3.zero; // Stop the enemy's movement
    }

    private void MoveTowardsPlayer()
    {
        if (target != null && !door)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= detectionRange)
            {
                PunchRadius punchRadius = target.GetComponent<PunchRadius>();
                if (punchRadius != null)
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    Vector3 destination = target.position - direction * (punchRadius.radius - 0.1f); // Оставляем небольшой зазор

                    float distanceToDestination = Vector3.Distance(transform.position, destination);
                    if (distanceToDestination > 0.1f)
                    {
                        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
                    }
                    else
                    {
                        rb.velocity = Vector3.zero;
                    }

                    // Проверка, находится ли противник в радиусе удара
                    if (punchRadius.IsEnemyInRange(transform.position) && canAttack)
                    {
                        StartCoroutine(AttackPlayer());
                    }
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
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
        if (!door) StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        animator.SetBool("walkEnemy", false);
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
    private IEnumerator AttackPlayer()
    {
        canAttack = false;

        // Запуск анимации атаки
        animator.SetBool("Attack", true);

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            playerHealth.Stun(stunDuration); // Оглушаем игрока
            Debug.Log($"Атаковал игрока на {attackDamage} урона и оглушил его!");
        }

        yield return new WaitForSeconds(attackCooldown);

        animator.SetBool("Attack", false);
        canAttack = true;
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