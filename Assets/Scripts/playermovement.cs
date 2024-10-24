using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 5f;
    public Terrain terrain; // ������� ������ �� ���� Terrain
    private Vector3 moveDirection;

   
    void Update()
    {
        // �������� ���� �� ������������
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // ������������ ����������� ��������
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // ���������� ��������� �� XZ ����
        transform.position += moveDirection * speed * Time.deltaTime;

        // ��������� ������� �� ������, ������ �� ������� terrain
        UpdatePlayerHeight();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdatePlayerHeight()
    {
        // �������� ������� ���������� X � Z ���������
        Vector3 playerPosition = transform.position;

        // �������� ������ terrain � ������� ������� ���������
        float terrainHeight = terrain.SampleHeight(playerPosition);

        // ��������� Y ���������� ��������� � ����������� �� ������ terrain
        playerPosition.y = terrainHeight + 0.5f; // 0.5f - ������ ��������� ��� ������

        // ��������� ����� �������
        transform.position = playerPosition;
    }
    void LateUpdate()
    {
        // �������� ������ ������� �� ������
        transform.forward = Camera.main.transform.forward;
    }
}

