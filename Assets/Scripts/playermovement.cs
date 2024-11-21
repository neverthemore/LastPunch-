using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 5f;
    public Terrain terrain; // ������ �� Terrain.
    public Transform spriteTransform; // ������ �� ������ �� ��������.

    private Vector3 moveDirection;

    void Update()
    {
        // �������� ���� �� ������������.
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // ���������� ����������� ����� � ������ ������������ ������.
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // ���������� ������� �� ��������� XZ.
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // ������������ ����������� ��������.
        moveDirection = forward * inputZ + right * inputX;

        // ���������� ���������.
        //transform.position += moveDirection * speed * Time.deltaTime;
        GetComponent<CharacterController>().Move(moveDirection * speed * Time.deltaTime);

        // ��������� ������ ��������� � ����������� �� �������.
        UpdatePlayerHeight();

        // ���� �������� ����, ��������� ����������� ���������.
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDirection; // ������������ ������ ���������.
        }

        // ��������� ������, ����� �� �� �������������.
        FixSpriteRotation();
    }

    void UpdatePlayerHeight()
    {
        // �������� ������� �������.
        Vector3 playerPosition = transform.position;

        // ������������ ������ ��������� �� ������ �������.
        float terrainHeight = terrain.SampleHeight(playerPosition);

        // ������������� ������� ��������� ���� ���� �������.
        playerPosition.y = terrainHeight + 0.5f;
        transform.position = playerPosition;
    }

    void FixSpriteRotation()
    {
        // ������������� ���������� ������� ���, ����� �� ������ ������� �� ������.
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // ������� ������ �� ��� Y.
        spriteTransform.forward = -cameraForward.normalized;
    }
}
