using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation; // Подключаем пространство имен для Sprite Library

public class WeaponSwitcher : MonoBehaviour
{




    [SerializeField] public bool hands = true;

    [SerializeField] public bool pistol = false;

    [SerializeField] public bool ak = false;





    [SerializeField]
    private SpriteLibrary spriteLibrary = default; // Ссылка на SpriteLibrary

    [SerializeField]
    private SpriteResolver targetResolver = default; // Ссылка на SpriteResolver

    [SerializeField]
    private string targetCategory = default; // Категория для смены оружия

    private SpriteLibraryAsset LibraryAsset => spriteLibrary.spriteLibraryAsset; // Получаем SpriteLibraryAsset

    void Update()
    {
        // Смена оружия по нажатию клавиш 1, 2, 3 и т.д.
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Клавиша 1
        {
            SwitchWeaponByIndex(0); // Переключение на первое оружие
            hands = true;
            pistol = false;
            ak = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Клавиша 2
        {
            SwitchWeaponByIndex(1); // Переключение на второе оружие
            hands = false;
            pistol = true;
            ak = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Клавиша 3
        {
            SwitchWeaponByIndex(2); // Переключение на третье оружие
            hands = false;
            pistol = false;
            ak = true;
        }
    }

    /// <summary>
    /// Переключает оружие по индексу.
    /// </summary>
    /// <param name="index">Индекс оружия в списке.</param>
    public void SwitchWeaponByIndex(int index)
    {
        // Получаем все метки (labels) для целевой категории
        string[] labels = LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();

        // Проверяем, чтобы индекс был в пределах массива
        if (index < 0 || index >= labels.Length)
        {
            Debug.LogWarning("Индекс оружия вне диапазона!");
            return;
        }

        // Устанавливаем выбранное оружие
        string label = labels[index];
        targetResolver.SetCategoryAndLabel(targetCategory, label);

        Debug.Log($"Оружие изменено на: {label}");
    }

    /// <summary>
    /// Выбирает случайное оружие.
    /// </summary>
    public void SelectRandom()
    {
        string[] labels = LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();
        int index = Random.Range(0, labels.Length);
        string label = labels[index];

        targetResolver.SetCategoryAndLabel(targetCategory, label);
    }

    /// <summary>
    /// Внедряет кастомный спрайт.
    /// </summary>
    /// <param name="customSprite">Кастомный спрайт.</param>
    public void InjectCustom(Sprite customSprite)
    {
        // Копируем кости и позы
        string referenceLabel = targetResolver.GetLabel();
        Sprite referenceHead = spriteLibrary.GetSprite(targetCategory, referenceLabel);
        SpriteBone[] bones = referenceHead.GetBones();
        NativeArray<Matrix4x4> poses = referenceHead.GetBindPoses();
        customSprite.SetBones(bones);
        customSprite.SetBindPoses(poses);

        // Внедряем новый спрайт
        const string customLabel = "customHead";
        spriteLibrary.AddOverride(customSprite, targetCategory, customLabel);
        targetResolver.SetCategoryAndLabel(targetCategory, customLabel);
    }
}
