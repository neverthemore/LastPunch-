using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SpriteRenderer handsSpriteRenderer;
    public SpriteRenderer bodySpriteRenderer;
    public SpriteRenderer headSpriteRenderer;
    public SpriteRenderer righthandSpriteRenderer;
    public Sprite idleHandsSprite; // Спрайт рук в состоянии ожидания
    public Sprite punchHandsSprite; // Спрайт рук при ударе
    public Sprite BodySpriteIdle;
    public Sprite BodySpritePunch;
    public Sprite headSpriteIdle;
    public Sprite headSpritePunch;
    public Sprite righthandSpriteIdle;
    public Sprite righthandSpritePunch;
    public Transform punchPoint; // Точка, откуда совершается удар
    public float punchRange = 1.5f; // Радиус удара
    public LayerMask enemyLayers; // Слой врагов
    public float punchDamage = 10f; // Урон от удара

    void Update()
    {
        // Если нажата левая кнопка мыши, запускаем атаку
        if (Input.GetMouseButtonDown(0))
        {
            Punch();
        }
    }

    void Punch()
    {
        // Меняем спрайт рук на спрайт удара
        handsSpriteRenderer.sprite = punchHandsSprite;
        bodySpriteRenderer.sprite = BodySpritePunch;
        headSpriteRenderer.sprite = headSpritePunch;
        righthandSpriteRenderer.sprite = righthandSpritePunch;

        // Проверяем на наличие врагов в зоне удара
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, enemyLayers);

        // Наносим урон всем врагам
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(punchDamage);
        }

        // Запускаем корутину для возврата спрайта рук в исходное состояние
        StartCoroutine(ResetHandsSprite());
    }

    private IEnumerator ResetHandsSprite()
    {
        // Ждем короткое время, чтобы спрайт удара был виден
        yield return new WaitForSeconds(0.2f);

        // Возвращаем спрайт рук в состояние ожидания
        handsSpriteRenderer.sprite = idleHandsSprite;
        bodySpriteRenderer.sprite = BodySpriteIdle;
        headSpriteRenderer.sprite = headSpriteIdle;
        righthandSpriteRenderer.sprite = righthandSpriteIdle;
    }

    void OnDrawGizmosSelected()
    {
        // Отображение радиуса удара в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRange);
    }
}
