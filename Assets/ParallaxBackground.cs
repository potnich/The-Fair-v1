// using UnityEngine;

// public class ParallaxBackground : MonoBehaviour
// {
//     [SerializeField] private float _scrollSpeed = 1f; // Скорость движения
//     [SerializeField] private Vector2 _direction = Vector2.left; // Направление (влево)
    
//     private Vector2 _startPosition;
//     private float _spriteWidth;
    
//     void Start()
//     {
//         _startPosition = transform.position;
        
//         // Получаем ширину спрайта для зацикливания
//         SpriteRenderer sr = GetComponent<SpriteRenderer>();
//         if (sr != null)
//         {
//             _spriteWidth = sr.bounds.size.x;
//         }
//     }
    
//     void Update()
//     {
//         // Двигаем фон
//         transform.Translate(_direction * _scrollSpeed * Time.deltaTime);
        
//         // Зацикливание (если фон ушёл за пределы)
//         if (Mathf.Abs(transform.position.x - _startPosition.x) >= _spriteWidth)
//         {
//             transform.position = _startPosition;
//         }
//     }
// }
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 2f;
    [SerializeField] private Transform _background1;
    [SerializeField] private Transform _background2;
    
    private float _spriteWidth;
    
    void Start()
    {
        SpriteRenderer sr = _background1.GetComponent<SpriteRenderer>();
        _spriteWidth = sr.bounds.size.x;
        
        // Никаких Instantiate, только перемещение существующих
        _background2.position = new Vector3(_spriteWidth, 0, 0);
    }
    
    void Update()
    {
        _background1.Translate(Vector2.left * _scrollSpeed * Time.deltaTime);
        _background2.Translate(Vector2.left * _scrollSpeed * Time.deltaTime);
        
        if (_background1.position.x <= -_spriteWidth)
        {
            _background1.position = new Vector3(_background2.position.x + _spriteWidth, 0, 0);
        }
        
        if (_background2.position.x <= -_spriteWidth)
        {
            _background2.position = new Vector3(_background1.position.x + _spriteWidth, 0, 0);
        }
    }
}