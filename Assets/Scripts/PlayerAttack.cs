using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SpriteRenderer handsSpriteRenderer;
    public SpriteRenderer bodySpriteRenderer;
    public SpriteRenderer headSpriteRenderer;
    public SpriteRenderer righthandSpriteRenderer;
    public Sprite idleHandsSprite; // ������ ��� � ��������� ��������
    public Sprite punchHandsSprite; // ������ ��� ��� �����
    public Sprite BodySpriteIdle;
    public Sprite BodySpritePunch;
    public Sprite headSpriteIdle;
    public Sprite headSpritePunch;
    public Sprite righthandSpriteIdle;
    public Sprite righthandSpritePunch;
    public Transform punchPoint; // �����, ������ ����������� ����
    public float punchRange = 1.5f; // ������ �����
    public LayerMask enemyLayers; // ���� ������
    public float punchDamage = 10f; // ���� �� �����

    void Update()
    {
        // ���� ������ ����� ������ ����, ��������� �����
        if (Input.GetMouseButtonDown(0))
        {
            Punch();
        }
    }

    void Punch()
    {
        // ������ ������ ��� �� ������ �����
        handsSpriteRenderer.sprite = punchHandsSprite;
        bodySpriteRenderer.sprite = BodySpritePunch;
        headSpriteRenderer.sprite = headSpritePunch;
        righthandSpriteRenderer.sprite = righthandSpritePunch;

        // ��������� �� ������� ������ � ���� �����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, enemyLayers);

        // ������� ���� ���� ������
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(punchDamage);
        }

        // ��������� �������� ��� �������� ������� ��� � �������� ���������
        StartCoroutine(ResetHandsSprite());
    }

    private IEnumerator ResetHandsSprite()
    {
        // ���� �������� �����, ����� ������ ����� ��� �����
        yield return new WaitForSeconds(0.2f);

        // ���������� ������ ��� � ��������� ��������
        handsSpriteRenderer.sprite = idleHandsSprite;
        bodySpriteRenderer.sprite = BodySpriteIdle;
        headSpriteRenderer.sprite = headSpriteIdle;
        righthandSpriteRenderer.sprite = righthandSpriteIdle;
    }

    void OnDrawGizmosSelected()
    {
        // ����������� ������� ����� � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRange);
    }
}
