using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public bool isStunned = false; 
    public float stunDuration = 2f;
    private bool isBlocking = false;
    private Animator animator;
    private CharacterController cc;

    public float stamina = 100f; // Максимальная выносливость
    public float staminaCostPerBlock = 20f; // Стоимость блока в выносливости
    public float staminaRecoveryRate = 5f; // Восстановление выносливости в секунду
    private bool isRecoveringStamina = false; // Отслеживание состояния восстановления выносливости

    public Slider healthSlider;

    public Slider staminaSlider;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        UpdateStaminaUI();
        healthSlider.maxValue = health; 
        healthSlider.value = health;
    }
    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Space) && !isBlocking)
        {
            Block();
        }
    }
    public void TakeDamage(float damage)
    {
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
        if (!isStunned) // Не позволяем повторное оглушение
        {
            isStunned = true; // Устанавливаем состояние оглушения
            animator.SetBool("IsStunned", true); // Запускаем анимацию оглушения
            Debug.Log("Игрок оглушен!");

            // Отключение управления игроком
            cc.enabled = false;
            StartCoroutine(HandleStun(duration));
        }
    }
    private void Block()
    {
        if (stamina >= staminaCostPerBlock) // Проверка, достаточно ли выносливости для блока
        {
            isBlocking = true; // Устанавливаем состояние блока
            animator.SetBool("IsBlocking", true); // Проигрываем анимацию блока
            stamina -= staminaCostPerBlock; // Снижаем выносливость
            UpdateStaminaUI();
            Debug.Log("Игрок заблокировал удар!");

            // Время блока, например, 1 секунда
            StartCoroutine(EndBlock(1f));
        }
        else
        {
            Debug.Log("Недостаточно выносливости для блока!");
        }
    }
    private IEnumerator EndBlock(float duration)
    {
        yield return new WaitForSeconds(duration); // Ждем окончания блока
        isBlocking = false; // Сбрасываем состояние блока
        animator.SetBool("IsBlocking", false); // Останавливаем анимацию блока
        Debug.Log("Игрок больше не блокирует.");

        // Начинаем восстановление выносливости
        isRecoveringStamina = true;
    }
    private IEnumerator HandleStun(float duration)
    {
        yield return new WaitForSeconds(duration); // Ждем окончания оглушения
        animator.SetBool("IsStunned", false); // Останавливаем анимацию оглушения
        isStunned = false;
        cc.enabled = true;
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
}
