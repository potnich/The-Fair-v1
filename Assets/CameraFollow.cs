// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     [SerializeField] private Transform _target; 
//     [SerializeField] private float _smoothSpeed = 5f; 
//     [SerializeField] private Vector3 _offset = new Vector3(0, 0, -10); 

//     void LateUpdate()
//     {
//         if (_target == null) return;
        
//         Vector3 desiredPosition = new Vector3(_target.position.x, transform.position.y, -10);
        
//         Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        
//         transform.position = smoothedPosition;
//     }
// }
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target; 
    [SerializeField] private float _smoothSpeed = 5f; 
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, -10); 

    public Vector2 minBounds; // Минимум X и Y
    public Vector2 maxBounds; // Максимум X и Y

    void LateUpdate()
    {
        Vector3 desiredPosition = _target.position + _offset;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
    }
}