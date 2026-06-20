using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 2f;
    
    void Start()
    {
        if (_fadeImage != null)
        {
            _fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeInCoroutine());
        }
        else
        {
            Debug.LogError("FadeImage не назначен!");
        }
    }
    
    IEnumerator FadeInCoroutine()
    {
        Color color = _fadeImage.color;
        color.a = 1f;
        _fadeImage.color = color;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);
            _fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
        _fadeImage.gameObject.SetActive(false);
        
        Debug.Log($"Fade закончился. Длительность была: {_fadeDuration} секунд"); // ← отладка
    }
}