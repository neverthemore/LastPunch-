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
        GetComponent<CharacterController>().Move(moveDirection * speed * Time.deltaTime);

        // ��������� ������ ��������� � ����������� �� �������.
        UpdatePlayerHeight();

        // ��������� ������� ������� � ����������� �� ��������� �������.
        UpdateSpriteDirection();
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

    void UpdateSpriteDirection()
    {
        // �������� ������� ������� � ������� �����������.
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // ��������� ����������� � ���������� (�� ������ ���������).
        Plane plane = new Plane(Vector3.up, transform.position);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldMousePosition = ray.GetPoint(distance);

            // ���������, ��������� �� ������ ����� ��� ������ �� ���������.
            if (worldMousePosition.x < transform.position.x)
            {
                // ������ ����� � ������������ ������ �����.
                spriteTransform.localEulerAngles = new Vector3(spriteTransform.localEulerAngles.x, 180, 0);
            }
            else
            {
                // ������ ������ � ������������ ������ ������.
                spriteTransform.localEulerAngles = new Vector3(spriteTransform.localEulerAngles.x, 0, 0);
            }
        }
    }

    void LateUpdate()
    {
        // ��������� ������� ������� �� ������ ����, ����� X.
        Vector3 fixedRotation = spriteTransform.localEulerAngles;
        fixedRotation.y = Mathf.Clamp(fixedRotation.y, 0, 180); // ������������ ������� �� Y.
        fixedRotation.z = 0; // ���������� ������� �� Z.
        spriteTransform.localEulerAngles = fixedRotation;
    }
}
