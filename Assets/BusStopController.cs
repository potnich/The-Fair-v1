using UnityEngine;
using System.Collections;

public class BusStopController : MonoBehaviour
{
    [Header("Настройки времени")]
    [SerializeField] private float _timeBeforeStop = 5f;
    [SerializeField] private float _stopDuration = 3f;
    
    [Header("Компоненты")]
    [SerializeField] private MonoBehaviour _parallax;
    [SerializeField] private SpriteRenderer _backgroundSprite;
    
    [Header("Спрайты фона")]
    [SerializeField] private Sprite _closedDoors;
    [SerializeField] private Sprite _slightlyOpen;
    [SerializeField] private Sprite _halfOpen;
    [SerializeField] private Sprite _fullyOpen;
    
    [Header("UI")]
    [SerializeField] private GameObject _notificationPanel;
    [SerializeField] private TMPro.TextMeshProUGUI _notificationText;
    
    [Header("Ручные настройки фона")]
    [SerializeField] private Vector3 _manualScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] private Vector3 _manualRotation = new Vector3(0f, 0f, 0f); // ← добавили
    
    private float _originalSpeed;
    
    void Start()
    {
        if (_parallax != null)
        {
            _originalSpeed = ((ParallaxBackground)_parallax)._scrollSpeed;
        }
        
        StartCoroutine(StopSequence());
    }
    
    private void ChangeBackgroundSprite(Sprite newSprite)
    {
        if (_backgroundSprite == null || newSprite == null) return;
        
        _backgroundSprite.sprite = newSprite;
        
        // Применяем размер
        _backgroundSprite.transform.localScale = _manualScale;
        
        // Применяем поворот
        _backgroundSprite.transform.eulerAngles = _manualRotation;
    }
    
    IEnumerator StopSequence()
    {
        yield return new WaitForSeconds(_timeBeforeStop);
        
        if (_notificationPanel != null)
        {
            _notificationPanel.SetActive(true);
            _notificationText.text = "🚏 Остановка через 5 секунд...";
        }
        
        yield return new WaitForSeconds(3f);
        
        if (_notificationPanel != null)
        {
            _notificationText.text = "🚌 Автобус останавливается...";
        }
        
        float slowdownDuration = 2f;
        float elapsedTime = 0f;
        
        while (elapsedTime < slowdownDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slowdownDuration;
            float currentSpeed = Mathf.Lerp(_originalSpeed, 0f, t);
            
            if (_parallax != null)
            {
                ((ParallaxBackground)_parallax)._scrollSpeed = currentSpeed;
            }
            
            yield return null;
        }
        
        if (_parallax != null)
        {
            ((ParallaxBackground)_parallax)._scrollSpeed = 0f;
        }
        
        if (_backgroundSprite != null)
        {
            ChangeBackgroundSprite(_slightlyOpen);
            yield return new WaitForSeconds(0.5f);
            
            ChangeBackgroundSprite(_halfOpen);
            yield return new WaitForSeconds(0.5f);
            
            ChangeBackgroundSprite(_fullyOpen);
            yield return new WaitForSeconds(0.5f);
        }
        
        if (_notificationPanel != null)
        {
            _notificationText.text = "🚪 Двери открыты!";
        }
        
        yield return new WaitForSeconds(_stopDuration);
        
        if (_backgroundSprite != null)
        {
            ChangeBackgroundSprite(_halfOpen);
            yield return new WaitForSeconds(0.4f);
            
            ChangeBackgroundSprite(_slightlyOpen);
            yield return new WaitForSeconds(0.4f);
            
            ChangeBackgroundSprite(_closedDoors);
            yield return new WaitForSeconds(0.4f);
        }
        
        if (_parallax != null)
        {
            ((ParallaxBackground)_parallax)._scrollSpeed = _originalSpeed;
        }
        
        if (_notificationPanel != null)
        {
            _notificationPanel.SetActive(false);
        }
        
        Destroy(gameObject);
    }
}