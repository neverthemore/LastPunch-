using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonOne; // Кнопка для старта
    [SerializeField] private Image _fadeImage; // Изображение для затухания

    private void Start()
    {
        // Подписываемся на событие нажатия кнопки
        if (_buttonOne != null)
        {
            _buttonOne.onClick.AddListener(StartButton);
        }
    }

    public void StartButton()
    {
        StartCoroutine(FadeOut(_fadeImage));
       
    }

    private IEnumerator FadeOut(Image image)
    {
        image.gameObject.SetActive(true); // Активируем изображение
        Color targetImage = image.color;

        float halfDuration = 3f; 
        float time = 0f;

        float startAlpha = 0f;
        float endAlpha = 1f;

        
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, time / halfDuration);
            image.color = new Color(targetImage.r, targetImage.g, targetImage.b, newAlpha);
            yield return null;
        }

         

        SceneManager.LoadScene(1); // Загрузка сцены по индексу
    }
}