using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public Sprite[] stunFrames; // Массив спрайтов для анимации оглушения
    private SpriteRenderer effectSpriteRenderer; // SpriteRenderer для эффекта
    private GameObject effectObject;

    public Vector3 enemyDirectionLocal;

    public GameObject instructionPrefab; // Префаб анимации для рук
    public float instructionOffset = 0.5f; // Смещение для размещения анимации
    public Sprite[] instructionFrames;

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
    private Vector3 originalScale; // Оригинальный масштаб спрайта
    public int headDamage = 15;
    public int bodyDamage = 10;
    public int legDamage = 5;

    private float attackTimer = 0;
    private bool isAttacking = false;

    [SerializeField] private CutSceneLogic _cutSceneLogic;

    void Start()
    {
        originalScale = transform.localScale;
        effectObject = new GameObject("StunEffect");
        effectSpriteRenderer = effectObject.AddComponent<SpriteRenderer>();
        effectSpriteRenderer.sortingOrder = 10000; // Устанавливаем порядок отрисовки выше, чем у других спрайтов
        effectObject.SetActive(false);

        effectObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

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
        Debug.Log(door);
    }
    void Update()
    {

        // Обновляем позицию эффекта, чтобы он следовал за головой
        if (isStunned)
        {
            effectObject.transform.position = transform.position; // Позиция над головой
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
                
                
            }
           
            FaceCamera();
            
            attackTimer -= Time.deltaTime;
        }
    }
    public void PushBack(Vector3 direction)
    {
        direction.y = 0; // Устанавливаем Y в 0, чтобы избежать подъема
        direction.z = 0;
        direction.Normalize();

        Rigidbody rb = GetComponent<Rigidbody>();
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
                    Vector3 destination = target.position - direction * (punchRadius.radius - 0.1f); // Устанавливаем позицию

                    float distanceToDestination = Vector3.Distance(transform.position, destination);
                    if (distanceToDestination > 0.1f)
                    {
                       
                        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
                        animator.SetBool("walkEnemy", true); // Запускаем анимацию ходьбы
                    }
                    else
                    {
                        rb.velocity = Vector3.zero;
                        animator.SetBool("walkEnemy", false); // Останавливаем анимацию ходьбы
                    }

                    // Проверка на возможность атаки
                    if (punchRadius.IsEnemyInRange(transform.position) && canAttack && !isAttacking)
                    {
                        // Если таймер атаки истек, начинаем атаку
                        if (attackTimer <= 0f)
                        {
                            StartCoroutine(AttackPlayer());
                        }
                    }
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                animator.SetBool("walkEnemy", false); // Останавливаем анимацию, если не в зоне атаки
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
            Vector3 directionToPlayer = target.position - transform.position;

            // Нормализуем его
            directionToPlayer.Normalize();

            // Получаем направление противника в его локальной системе координат
            enemyDirectionLocal = transform.right; // Например, "вправо" может быть положительным X в локальной системе координат противника

            // Вычисляем скалярное произведение для определения направления игрока относительно противника
            float dotProduct = Vector3.Dot(enemyDirectionLocal, directionToPlayer);

            if (dotProduct > 0)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
              }
            else
            {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }

        
    }
    public void Stun(float duration)
    {
        if (!door) StartCoroutine(StunCoroutine(duration));
        ShowEffect(); // Отобразить эффект
        StartCoroutine(PlayAnimation(stunFrames, duration));

    }

    private void ShowEffect()
    {
        effectObject.SetActive(true); // Показываем эффект
    }
    private IEnumerator PlayAnimation(Sprite[] frames, float duration)
    {
        float frameDuration = duration / frames.Length; // Время для каждого кадра

        
        

        for (int i = 0; i < frames.Length; i++)
        {
            effectSpriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(frameDuration);
        }

       

        effectObject.SetActive(false);
    }

    private IEnumerator StunCoroutine(float duration)
    {
        animator.SetBool("walkEnemy", false);
        animator.SetBool("IsStunningEnemy", true);
        isStunned = true; // Freeze the enemy
        rb.velocity = Vector3.zero; // Stop movement
        yield return new WaitForSeconds(duration); // Wait for the stun duration
        isStunned = false; // Unfreeze the enemy
        animator.SetBool("IsStunningEnemy", false);
    }
    public void TakeDamage(int attackDamage, HitType hitType)
    {
        health -= attackDamage;
        if (door)
        {
            StartCoroutine(DoorShake());
        }
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
            Destroy(effectObject);
        }

    }
    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        isAttacking = true; // Устанавливаем состояние атаки

        // Запуск анимации атаки
        animator.SetBool("Attack", true);

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            playerHealth.Stun(stunDuration); // Оглушаем игрока
            Debug.Log($"Атакован игрок на {attackDamage} урона и оглушен!");
        }

        // Устанавливаем таймер атаки
        attackTimer = attackCooldown;

        // Ждем окончания анимации атаки 
        yield return new WaitForSeconds(attackCooldown);

        // Останавливаем анимацию атаки
        animator.SetBool("Attack", false);
        isAttacking = false; // Сбрасываем состояние атаки
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

    public enum HitType
    {
        Head,
        Body,
        Legs
    }

    #region Cutscene Logic
    private IEnumerator DoorShake()
    {
        float shake = 1f;
        float time = 0.07f;

        gameObject.transform.rotation = Quaternion.Euler(-shake, shake, -shake);
        if (health > 0) _cutSceneLogic.SelectEffects(true);
        yield return new WaitForSeconds(time);
        gameObject.transform.rotation = Quaternion.Euler(shake, -shake, shake);
        yield return new WaitForSeconds(time);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(time);
        if (health > 0) _cutSceneLogic.SelectEffects(false);
    }
    public void Shake() => StartCoroutine(DoorShake());
    #endregion
}