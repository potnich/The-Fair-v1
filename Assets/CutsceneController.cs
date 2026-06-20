using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private GameObject _cutscenePanel;
    [SerializeField] private MonoBehaviour _playerMovement;
    [SerializeField] private float _runDistance = 3f;
    [SerializeField] private float _runSpeed = 2f;
    [SerializeField] private bool _runRight = true; // true = бежит и смотрит направо
    
    private Rigidbody2D _playerRb;
    private Animator _animator;
    private Vector3 _originalScale;
    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _playerRb = _playerMovement.GetComponent<Rigidbody2D>();
        _animator = _playerMovement.GetComponent<Animator>();
        _spriteRenderer = _playerMovement.GetComponent<SpriteRenderer>();
        _originalScale = _playerMovement.transform.localScale; // запоминаем размер (чтобы не увеличивался)
        
        // Отключаем управление игроком
        _playerMovement.enabled = false;
        _cutscenePanel.SetActive(false);
        
        StartCoroutine(PlayCutscene());
    }
    
    IEnumerator PlayCutscene()
    {
        // ==========================================
        // 1. ПРИНУДИТЕЛЬНЫЙ ПОВОРОТ (смотрим направо)
        // ==========================================
        if (_runRight)
        {
            // Бежим направо → Отражаем спрайт, чтобы смотрел направо
            if (_spriteRenderer != null)
                _spriteRenderer.flipX = true;
            
            _playerMovement.transform.localScale = new Vector3(Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
            _playerMovement.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            // Бежим налево → Не отражаем (смотрит налево)
            if (_spriteRenderer != null)
                _spriteRenderer.flipX = false;
            
            _playerMovement.transform.localScale = new Vector3(-Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
            _playerMovement.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        
        // 2. Включаем анимацию бега
        _animator.SetFloat("Speed", 1f);
        
        // 3. Бежим
        Vector2 direction = _runRight ? Vector2.right : Vector2.left;
        float distanceTraveled = 0;
        
        while (distanceTraveled < _runDistance)
        {
            float step = _runSpeed * Time.deltaTime;
            _playerRb.transform.Translate(direction * step);
            distanceTraveled += step;
            yield return null;
        }
        
        // 4. Останавливаемся
        _animator.SetFloat("Speed", 0f);
        _playerRb.linearVelocity = Vector2.zero;
        
        // 5. Показываем диалог
        _cutscenePanel.SetActive(true);
        
        float timer = 0;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        _cutscenePanel.SetActive(false);
        
        // 6. Включаем управление обратно (теперь игрок сам повернет, куда захочет)
        _playerMovement.enabled = true;
        
        // 7. Удаляем скрипт катсцены, чтобы он не мешал
        Destroy(gameObject);
    }
}