using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public bool isStunned = false; 
    public float stunDuration = 2f; 
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void TakeDamage(float damage)
    {
        health -= (int)damage;
        Debug.Log($"Player took {damage} damage! Remaining health: {health}");
        if (health <= 0)
        {
            Die();
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
            StartCoroutine(HandleStun(duration));
        }
    }
    private IEnumerator HandleStun(float duration)
    {
        yield return new WaitForSeconds(duration); // Ждем окончания оглушения
        isStunned = false; // Сбрасываем состояние оглушения
        animator.SetBool("IsStunned", false); // Останавливаем анимацию оглушения
        Debug.Log("Игрок больше не оглушен.");
    }

    private void Die()
    {
        Debug.Log("Player died!");  
        Destroy(gameObject);
    }
}