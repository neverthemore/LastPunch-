using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button _buttonOne;
    [SerializeField] private Image _fadeImage;

    public void StartButton()
    {        
        StartCoroutine(FadeOut(_fadeImage));
    }
    private IEnumerator FadeOut(UnityEngine.UI.Image image)
    {
        image.gameObject.SetActive(true);
        Color targetImage = image.color;

        float halfDuration = 5f;
        float time = 0f;
        float startAlpha;
        float endAlpha;

        startAlpha = 1f;
        endAlpha = 0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, time / halfDuration);
            image.color = new Color(targetImage.r, targetImage.g, targetImage.b, newAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
