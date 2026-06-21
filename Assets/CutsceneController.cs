using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private GameObject _cutscenePanel;   // панель диалога
    [SerializeField] private MonoBehaviour _playerMovement;
    [SerializeField] private float _runDistance = 3f;
    [SerializeField] private float _runSpeed = 2f;
    [SerializeField] private bool _runRight = true;
    
    private Rigidbody2D _playerRb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalScale;
    
    void Start()
    {
        // Получаем компоненты с Player
        _playerRb = _playerMovement.GetComponent<Rigidbody2D>();
        _animator = _playerMovement.GetComponent<Animator>();
        _spriteRenderer = _playerMovement.GetComponent<SpriteRenderer>();
        _originalScale = _playerMovement.transform.localScale;
        
        // Отключаем управление игроком
        _playerMovement.enabled = false;
        
        // ПРОВЕРКА: принудительно включаем диалог, чтобы убедиться, что он работает
        if (_cutscenePanel != null)
        {
            _cutscenePanel.SetActive(true);
            Debug.Log("Диалог принудительно включен в Start()!");
        }
        else
        {
            Debug.LogError("Cutscene Panel не назначен в инспекторе!");
        }
        
        // Запускаем катсцену
        StartCoroutine(PlayCutscene());
    }
    
    IEnumerator PlayCutscene()
    {
        // Поворот персонажа
        if (_runRight)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.flipX = false;
            _playerMovement.transform.localScale = new Vector3(Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
        else
        {
            if (_spriteRenderer != null)
                _spriteRenderer.flipX = true;
            _playerMovement.transform.localScale = new Vector3(-Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
        
        // Бег
        _animator.SetFloat("Speed", 1f);
        
        Vector2 direction = _runRight ? Vector2.right : Vector2.left;
        float distanceTraveled = 0;
        
        while (distanceTraveled < _runDistance)
        {
            float step = _runSpeed * Time.deltaTime;
            _playerRb.transform.Translate(direction * step);
            distanceTraveled += step;
            yield return null;
        }
        
        // Останавливаемся
        _animator.SetFloat("Speed", 0f);
        _playerRb.linearVelocity = Vector2.zero;
        
        // Ждём 1 секунду
        yield return new WaitForSeconds(1f);
        
        // ВКЛЮЧАЕМ ДИАЛОГ
        Debug.Log("Включаем диалог!");
        if (_cutscenePanel != null)
        {
            _cutscenePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Cutscene Panel потерян!");
        }
        
        // Ждём 3 секунды
        yield return new WaitForSeconds(3f);
        
        // ВЫКЛЮЧАЕМ ДИАЛОГ
        Debug.Log("Выключаем диалог!");
        if (_cutscenePanel != null)
        {
            _cutscenePanel.SetActive(false);
        }
        
        // Возвращаем управление
        _playerMovement.enabled = true;
        
        // Удаляем скрипт
        Destroy(gameObject);
    }
}