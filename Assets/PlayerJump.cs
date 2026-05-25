using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private Transform _groundCheck;  // Точка проверки земли
    [SerializeField] private float _groundCheckRadius = 0.2f;  // Радиус проверки
    [SerializeField] private LayerMask _groundLayer;  // Слой земли
    
    private Rigidbody2D _rb;
    private bool _isGrounded;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Автоматически создаем точку проверки земли, если не назначена
        if (_groundCheck == null)
        {
            _groundCheck = new GameObject("GroundCheck").transform;
            _groundCheck.SetParent(transform);
            _groundCheck.localPosition = new Vector3(0, -3.01f, 0);
        }
    }
    
    void Update()
    {
        // Проверка через OverlapCircle (надежнее луча)
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        
        // Прыжок только если на земле
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            Debug.Log("Прыжок!");  // Временная отладка
        }
    }
    
    // Визуализация для отладки
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}