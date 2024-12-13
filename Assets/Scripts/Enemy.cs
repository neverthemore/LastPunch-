using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f; // Скорость движения врага
    public float detectionRange = 10f; // Радиус обнаружения цели
    public int health = 30; // Здоровье врага
    public Camera mainCamera; // Камера, на которую враг должен смотреть

    //public string sortinglayer; // Слои спрайтов для врага 

    private Transform target; // Цель (игрок или другой объект)
    private Rigidbody rb; // Ссылка на Rigidbody врага

    void Start()
    {
        // Поиск игрока по тегу
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }

        // Получаем Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true; // Отключаем физическое вращение врага
        }

        // Если камера не указана, берем основную
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        
    }

    //private void Update()
    //{
    //    Setsortinglayer(sortinglayer);
    //}

    public void Setsortinglayer(string layer)
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.sortingLayerName = layer;
        }
    }
    void FixedUpdate()
    {
        // Проверяем, есть ли цель
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Если цель в радиусе обнаружения, двигаться к ней
            if (distanceToTarget <= detectionRange)
            {
                // Рассчитываем направление к цели
                Vector3 direction = (target.position - transform.position).normalized;

                // Движение
                rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
            }
            else
            {
                rb.velocity = Vector3.zero; // Останавливаем движение, если цель вне зоны
            }
        }

        // Вращаем врага лицевой стороной к камере
        FaceCamera();
    }

    private void FaceCamera()
    {
        if (mainCamera != null)
        {
            Vector3 cameraDirection = mainCamera.transform.position - transform.position;

            // Убираем вертикальную составляющую, чтобы враг не наклонялся
            cameraDirection.y = 0;

            // Поворачиваем врага в сторону камеры
            if (cameraDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-cameraDirection);
            }
        }
    }

    // Метод для получения урона
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage; // Уменьшаем здоровье врага
        Debug.Log($"Enemy took {attackDamage} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die(); // Уничтожаем врага, если здоровье <= 0
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Уничтожаем объект врага
    }
}
