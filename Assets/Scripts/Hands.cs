using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static Enemy;

public class Hands : MonoBehaviour
{
    public Sprite[] stunFrames; // Массив спрайтов для анимации оглушения
    private SpriteRenderer effectSpriteRenderer; // SpriteRenderer для эффекта
    private GameObject effectObject;
    public float effectDuration = 0.4f;
    public Collider leftHandCollider; // Collider for the left hand
    public Collider rightHandCollider; // Collider for the right hand
    public float freezeDuration;




    void Start()
    {
        effectObject = new GameObject("StunEffect");
        effectSpriteRenderer = effectObject.AddComponent<SpriteRenderer>();
        effectSpriteRenderer.sortingOrder = 10000; // Устанавливаем порядок отрисовки выше, чем у других спрайтов
        effectObject.SetActive(false);
        effectObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

    }
        private void Update()
    {
            effectObject.transform.position = transform.position;
        }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that was hit is a child of the enemy
        if (other.CompareTag("Enemy") || other.transform.parent.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>(); // Get the Enemy component from the parent
            if (enemy != null)
            {
                int attackDamage = 0;
                HitType hitType = HitType.Body; // Default to body hit
                Vector3 pushDirection = (other.transform.position - transform.position).normalized; // Calculate the push direction


                // Check if the left hand collider is enabled
                if (leftHandCollider.enabled)
                {
                    if (other.gameObject.name.Contains("Head"))
                    {
                        attackDamage = enemy.headDamage;
                        hitType = HitType.Head;
                        Debug.Log("Hit Head!");
                        enemy.Stun(freezeDuration);
                        ShowEffect(); // Отобразить эффект
                        StartCoroutine(PlayAnimation(stunFrames, effectDuration));

                    }
                    else if (other.gameObject.name.Contains("Body"))
                    {
                        attackDamage = enemy.bodyDamage;
                        hitType = HitType.Body;
                        Debug.Log("Hit Body!");
                        enemy.Stun(freezeDuration);
                        ShowEffect(); // Отобразить эффект
                        StartCoroutine(PlayAnimation(stunFrames, effectDuration));

                    }
                    else if (other.gameObject.name.Contains("Legs"))
                    {
                        attackDamage = enemy.legDamage;
                        hitType = HitType.Legs;
                        Debug.Log("Hit Legs!");
                        enemy.Stun(freezeDuration);
                    }
                }

                // Check if the right hand collider is enabled
                if (rightHandCollider.enabled)
                {
                    if (other.gameObject.name.Contains("Head"))
                    {
                        attackDamage = enemy.headDamage;
                        hitType = HitType.Head;
                        Debug.Log("Hit Head with Right Hand!");
                        enemy.Stun(freezeDuration);
                        ShowEffect(); // Отобразить эффект
                        StartCoroutine(PlayAnimation(stunFrames, effectDuration));

                    }
                    else if (other.gameObject.name.Contains("Body"))
                    {
                        attackDamage = enemy.bodyDamage;
                        hitType = HitType.Body;
                        Debug.Log("Hit Body with Right Hand!");
                        enemy.Stun(freezeDuration);
                        ShowEffect(); // Отобразить эффект
                        StartCoroutine(PlayAnimation(stunFrames, effectDuration));

                    }
                    else if (other.gameObject.name.Contains("Legs"))
                    {
                        attackDamage = enemy.legDamage;
                        hitType = HitType.Legs;
                        Debug.Log("Hit Legs with Right Hand!");
                        enemy.Stun(freezeDuration);
                    }
                }

                // If damage is greater than zero, apply it to the enemy
                if (attackDamage > 0)
                {
                    enemy.TakeDamage(attackDamage, hitType);
                    enemy.PushBack(pushDirection);
                    Debug.Log($"Dealt {attackDamage} damage to {hitType}!");                  
                }
                else
                {
                    Debug.Log("No damage dealt.");
                }
            }
            else
            {
                Debug.Log("No Enemy component found on the collided object.");
            }
        }
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
}