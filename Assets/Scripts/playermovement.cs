using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 5f;
    public Terrain terrain; // Присвой ссылку на твой Terrain
    private Vector3 moveDirection;

   
    void Update()
    {
        // Получаем ввод от пользователя
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Рассчитываем направление движения
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Перемещаем персонажа по XZ осям
        transform.position += moveDirection * speed * Time.deltaTime;

        // Обновляем позицию по высоте, исходя из рельефа terrain
        UpdatePlayerHeight();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdatePlayerHeight()
    {
        // Получаем текущие координаты X и Z персонажа
        Vector3 playerPosition = transform.position;

        // Получаем высоту terrain в текущей позиции персонажа
        float terrainHeight = terrain.SampleHeight(playerPosition);

        // Обновляем Y координату персонажа в зависимости от высоты terrain
        playerPosition.y = terrainHeight + 0.5f; // 0.5f - высота персонажа над землей

        // Применяем новую позицию
        transform.position = playerPosition;
    }
    void LateUpdate()
    {
        // Персонаж всегда смотрит на камеру
        transform.forward = Camera.main.transform.forward;
    }
}

