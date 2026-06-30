using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] public float _scrollSpeed = 2f;
    [SerializeField] private Transform _background1;
    [SerializeField] private Transform _background2;
    
    private float _spriteWidth;
    private float _fixedY; // ← здесь храним правильную Y-координату

    void Start()
    {
        SpriteRenderer sr = _background1.GetComponent<SpriteRenderer>();
        _spriteWidth = sr.bounds.size.x;
        
        // ЗАПОМИНАЕМ Y ИЗ РЕДАКТОРА (ту высоту, где ты их поставил)
        _fixedY = _background1.position.y;
        
        // Принудительно выставляем фоны с правильным Y
        _background1.position = new Vector3(0, _fixedY, 0);
        _background2.position = new Vector3(_spriteWidth, _fixedY, 0);
    }

    void Update()
    {
        // Двигаем оба фона строго по горизонтали (Y не трогаем)
        _background1.Translate(Vector2.left * _scrollSpeed * Time.deltaTime);
        _background2.Translate(Vector2.left * _scrollSpeed * Time.deltaTime);
        
        // Зацикливание: если фон ушёл далеко влево
        if (_background1.position.x <= -_spriteWidth)
        {
            // Переставляем его в конец с сохранением Y
            _background1.position = new Vector3(_background2.position.x + _spriteWidth, _fixedY, 0);
        }
        
        if (_background2.position.x <= -_spriteWidth)
        {
            _background2.position = new Vector3(_background1.position.x + _spriteWidth, _fixedY, 0);
        }
    }
}