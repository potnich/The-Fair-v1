using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    
    void Start()
    {
        if (_fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }
    
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        Color color = _fadeImage.color;
        color.a = 1;
        _fadeImage.color = color;
        
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / _fadeDuration);
            _fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
        _fadeImage.gameObject.SetActive(false);
    }
}