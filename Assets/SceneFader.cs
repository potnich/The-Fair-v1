using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        if (_fadeImage != null)
        {
            _fadeImage.gameObject.SetActive(false);
        }
    }
    
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndIn(sceneName));
    }
    
    private IEnumerator FadeOutAndIn(string sceneName)
    {
        _fadeImage.gameObject.SetActive(true);
        
        // Затемнение
        float elapsedTime = 0;
        Color color = _fadeImage.color;
        color.a = 0;
        _fadeImage.color = color;
        
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / _fadeDuration);
            _fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
        // Загрузка сцены
        SceneManager.LoadScene(sceneName);
        
        // Осветление (ПОСЛЕ загрузки)
        elapsedTime = 0;
        color = _fadeImage.color;
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