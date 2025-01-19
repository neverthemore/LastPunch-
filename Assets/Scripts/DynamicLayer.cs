using UnityEngine;

public class DynamicLayerManager : MonoBehaviour
{
    private struct SpriteInfo
    {
        public SpriteRenderer spriteRenderer;
        public int originalOrder;
    }

    private SpriteInfo[] spriteInfos;
    public Transform playerTransform;
    public int orderOffset = 500; // Смещение порядка отображения

    void Start()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        spriteInfos = new SpriteInfo[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteInfos[i].spriteRenderer = spriteRenderers[i];
            spriteInfos[i].originalOrder = spriteRenderers[i].sortingOrder;
        }
    }

    void Update()
    {
        foreach (var spriteInfo in spriteInfos)
        {
            if (transform.position.z < playerTransform.position.z) 
            {
                spriteInfo.spriteRenderer.sortingOrder = spriteInfo.originalOrder + orderOffset; // Увеличиваем порядок для объектов позади игрока
            }
            else
            {
                spriteInfo.spriteRenderer.sortingOrder = spriteInfo.originalOrder - 500; // Восстанавливаем оригинальный порядок
            }
        }
    }
}
