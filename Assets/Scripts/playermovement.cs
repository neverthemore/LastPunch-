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
        GetComponent<CharacterController>().Move(moveDirection * speed * Time.deltaTime);

        // Обновляем высоту персонажа в зависимости от рельефа.
        UpdatePlayerHeight();

        // Обновляем поворот спрайта в зависимости от положения курсора.
        UpdateSpriteDirection();
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

    void UpdateSpriteDirection()
    {
        // Получаем позицию курсора в мировых координатах.
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Проверяем пересечение с плоскостью (на уровне персонажа).
        Plane plane = new Plane(Vector3.up, transform.position);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldMousePosition = ray.GetPoint(distance);

            // Проверяем, находится ли курсор слева или справа от персонажа.
            if (worldMousePosition.x < transform.position.x)
            {
                // Курсор слева — поворачиваем спрайт влево.
                spriteTransform.localEulerAngles = new Vector3(spriteTransform.localEulerAngles.x, 180, 0);
            }
            else
            {
                // Курсор справа — поворачиваем спрайт вправо.
                spriteTransform.localEulerAngles = new Vector3(spriteTransform.localEulerAngles.x, 0, 0);
            }
        }
    }

    void LateUpdate()
    {
        // Фиксируем поворот спрайта по другим осям, кроме X.
        Vector3 fixedRotation = spriteTransform.localEulerAngles;
        fixedRotation.y = Mathf.Clamp(fixedRotation.y, 0, 180); // Ограничиваем поворот по Y.
        fixedRotation.z = 0; // Сбрасываем поворот по Z.
        spriteTransform.localEulerAngles = fixedRotation;
    }
}
