using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Sprite[] stunFrames; // Массив спрайтов для анимации оглушения
    private SpriteRenderer effectSpriteRenderer; // SpriteRenderer для эффекта
    private GameObject effectObject;

    public Sprite[] stunFrames2; // Массив спрайтов для анимации оглушения
    private SpriteRenderer effectSpriteRenderer2; // SpriteRenderer для эффекта
    private GameObject effectObject2;

    private Movement movement;
    private float blockeffectduration = 0.5f;
    public float currentHealth;
    private float maxHealth = 100f;
    public bool isStunned = false; 
    public float stunDuration = 2f;
    public float blockDuration = 1.5f;
    public bool isBlocking = false;
    private Animator animator;

    private HandFollowCursor handFollowCursor;

    public float maxStamina = 100f;
    public float currentStamina; // Максимальная выносливость
    public float staminaCostPerSecond = 20f; // Стоимость блока в выносливости
    public float staminaRecoveryRate = 5f; // Восстановление выносливости в секунду
    private bool isRecoveringStamina = false; // Отслеживание состояния восстановления выносливости

    public Image healthBar;

    public Image staminaBar;
    void Start()
    {
        handFollowCursor = GetComponent<HandFollowCursor>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        effectObject = new GameObject("StunEffect");
        effectSpriteRenderer = effectObject.AddComponent<SpriteRenderer>();
        effectSpriteRenderer.sortingOrder = 10000; // Устанавливаем порядок отрисовки выше, чем у других спрайтов
        effectObject.SetActive(false);
        effectObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

        effectObject2 = new GameObject("StunEffect2");
        effectSpriteRenderer2 = effectObject2.AddComponent<SpriteRenderer>();
        effectSpriteRenderer2.sortingOrder = 100000; // Устанавливаем порядок отрисовки выше, чем у других спрайтов
        effectObject2.SetActive(false);
        effectObject2.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        
        movement = GetComponent<Movement>();
        animator = GetComponentInChildren<Animator>();

        
    }
    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        staminaBar.fillAmount = currentStamina / maxStamina;
        if (isBlocking)
        {
            effectObject2.transform.position = transform.position + Vector3.up * 0.9f + Vector3.left * 1.2f;
        }
        if (isStunned)
        {
            effectObject.transform.position = transform.position + Vector3.up * 1.1f + Vector3.left * 0.5f; // Позиция над головой
        }
        // Восстановление выносливости
        if (isRecoveringStamina && currentStamina < 100f)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina >= 100f)
            {
                currentStamina = 100f;
                isRecoveringStamina = false; // Останавливаем восстановление, когда выносливость полна
            }
            
        }
         

        // Проверка нажатия пробела
        if (Input.GetKey(KeyCode.Space) && !isStunned && movement.isRunning  && currentStamina > 0)
        {
            Block();
        }
        else
        {
            EndBlock();
        }
    }
    public void TakeDamage(float damage)
    {
        if (isBlocking)
        {
            
            ShowEffect2(); // Отобразить эффект
            StartCoroutine(PlayAnimation(stunFrames2, blockeffectduration));
        }
        

        
        if (!isBlocking) // Получение урона только если не оглушен и не блокирует
        {
            currentHealth -= (int)damage;
            Debug.Log($"Игрок получил {damage} урона! Осталось здоровья: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        else if (isBlocking)
        {
            Debug.Log("Игрок блокирует урон!");
        }
    }

    public void Stun(float duration)
    {
        if (!isStunned && !isBlocking) // Не позволяем повторное оглушение
        {
            isStunned = true; // Устанавливаем состояние оглушения
            animator.SetBool("IsStunned", true);
            animator.SetBool("Walk", false);
            Debug.Log("Игрок оглушен!");
            ShowEffect(); // Отобразить эффект
            StartCoroutine(PlayAnimation(stunFrames, duration));

            StartCoroutine(HandleStun(duration));
        }
    }
    private void ShowEffect()
    {
        effectObject.SetActive(true); // Показываем эффект
    }
    private void ShowEffect2()
    {
        effectObject2.SetActive(true); // Показываем эффект
    }
    private IEnumerator PlayAnimation(Sprite[] frames, float duration)
    {
        float frameDuration = duration / frames.Length; // Время для каждого кадра
        
        if(isBlocking == true)
        {
            yield return new WaitForSeconds(0.3f);
        }

        for (int i = 0; i < frames.Length; i++)
        {
            if(isBlocking == true)
            {            
                effectSpriteRenderer2.sprite = frames[i];
            }
            else
            {
                effectSpriteRenderer.sprite = frames[i];
            }
            
            yield return new WaitForSeconds(frameDuration);
        }


        effectObject.SetActive(false);
        effectObject2.SetActive(false);


    }
    private void Block()
    {
        if (!isBlocking)
        {
            isBlocking = true; // Устанавливаем состояние блока
            animator.SetBool("IsBlocking", true); // Проигрываем анимацию блока
        }

        // Потребляем выносливость
        currentStamina -= staminaCostPerSecond * Time.deltaTime;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }   
        if (currentStamina == 0)
        {
            EndBlock();
        }
        
    }
    private void EndBlock()
    {
        if (isBlocking)
        {
            isBlocking = false; // Сбрасываем состояние блока
            animator.SetBool("IsBlocking", false); // Останавливаем анимацию блока

            // Начинаем восстановление выносливости
            isRecoveringStamina = true;
        }
    }
    private IEnumerator HandleStun(float duration)
    {
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.enabled = false; // Отключаем скрипт движения
        }

        yield return new WaitForSeconds(duration); // Ждем окончания оглушения
        animator.SetBool("IsStunned", false); // Останавливаем анимацию оглушения
        isStunned = false;
        if (movement != null)
        {
            movement.enabled = true; // Включаем управление снова
        }
        Debug.Log("Игрок больше не оглушен.");
    }

    private void Die()
    {
        Debug.Log("Player died!");  
        Destroy(gameObject);
    }
  
    public void GetHealth(int value)
    {
        if (value + currentHealth >= maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += value;
        Debug.Log("Health regen: " + currentHealth);
    }
}
