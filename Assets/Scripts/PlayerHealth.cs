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


    private float blockeffectduration = 0.5f;
    public int health = 100;
    private int maxhealth;
    public bool isStunned = false; 
    public float stunDuration = 2f;
    public float blockDuration = 1.5f;
    public bool isBlocking = false;
    private Animator animator;

    public float stamina = 100f; // Максимальная выносливость
    public float staminaCostPerSecond = 20f; // Стоимость блока в выносливости
    public float staminaRecoveryRate = 5f; // Восстановление выносливости в секунду
    private bool isRecoveringStamina = false; // Отслеживание состояния восстановления выносливости

    public Slider healthSlider;

    public Slider staminaSlider;
    void Start()
    {
        maxhealth = health;
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
        

        animator = GetComponentInChildren<Animator>();
        UpdateStaminaUI();
        healthSlider.maxValue = health; 
        healthSlider.value = health;
    }
    void Update()
    {
        if (isBlocking)
        {
            effectObject2.transform.position = transform.position + Vector3.up * 0.9f + Vector3.left * 1.2f;
        }
        if (isStunned)
        {
            effectObject.transform.position = transform.position + Vector3.up * 1.1f + Vector3.left * 0.5f; // Позиция над головой
        }
        // Восстановление выносливости
        if (isRecoveringStamina && stamina < 100f)
        {
            stamina += staminaRecoveryRate * Time.deltaTime;
            if (stamina >= 100f)
            {
                stamina = 100f;
                isRecoveringStamina = false; // Останавливаем восстановление, когда выносливость полна
            }
            UpdateStaminaUI();
        }
         healthSlider.value = health;

        // Проверка нажатия пробела
        if (Input.GetKey(KeyCode.Space) && !isStunned && stamina > 0)
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
            health -= (int)damage;
            Debug.Log($"Игрок получил {damage} урона! Осталось здоровья: {health}");

            if (health <= 0)
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
        stamina -= staminaCostPerSecond * Time.deltaTime;
        if (stamina < 0)
        {
            stamina = 0;
        }
        if (stamina == 0)
        {
            EndBlock();
        }
        UpdateStaminaUI();
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
    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = stamina; // Обновляем значение слайдера
        }
    }
    public void GetHealth(int value)
    {
        if (value + health >= maxhealth)
            health = maxhealth;
        else
            health += value;
        Debug.Log("Health regen: " + health);
    }
}
