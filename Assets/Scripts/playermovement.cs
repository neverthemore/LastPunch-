using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 5f;
    public Terrain terrain; // Ссылка на Terrain.
    public Transform spriteTransform; // Ссылка на объект со спрайтом.

    private Vector3 moveDirection;

    void Update()
    {
        // Получаем ввод от пользователя.
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Определяем направления вперёд и вправо относительно камеры.
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Проецируем векторы на плоскость XZ.
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Рассчитываем направление движения.
        moveDirection = forward * inputZ + right * inputX;

        // Перемещаем персонажа.
        //transform.position += moveDirection * speed * Time.deltaTime;
        GetComponent<CharacterController>().Move(moveDirection * speed * Time.deltaTime);

        // Обновляем высоту персонажа в зависимости от рельефа.
        UpdatePlayerHeight();

        // Если движение есть, обновляем направление персонажа.
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDirection; // Поворачиваем объект персонажа.
        }

        // Фиксируем спрайт, чтобы он не поворачивался.
        FixSpriteRotation();
    }

    void UpdatePlayerHeight()
    {
        // Получаем текущую позицию.
        Vector3 playerPosition = transform.position;

        // Рассчитываем высоту персонажа на основе рельефа.
        float terrainHeight = terrain.SampleHeight(playerPosition);

        // Устанавливаем позицию персонажа чуть выше рельефа.
        playerPosition.y = terrainHeight + 0.5f;
        transform.position = playerPosition;
    }

    void FixSpriteRotation()
    {
        // Устанавливаем ориентацию спрайта так, чтобы он всегда смотрел на камеру.
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Убираем наклон по оси Y.
        spriteTransform.forward = -cameraForward.normalized;
    }
}
