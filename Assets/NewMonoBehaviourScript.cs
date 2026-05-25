using UnityEngine;

public class NewMohoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector3 _originalScale;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(moveX * _moveSpeed, _rb.linearVelocity.y);
        
        // Поворот
        if (moveX < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
        else if (moveX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
        
        _animator.SetFloat("Speed", Mathf.Abs(moveX));
    }
}