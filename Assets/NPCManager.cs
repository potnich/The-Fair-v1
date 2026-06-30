using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    [Header("Настройки NPC")]
    [SerializeField] private GameObject _npcPrefab; // префаб NPC (с спрайтом и Rigidbody2D)
    [SerializeField] private int _minNPC = 0;
    [SerializeField] private int _maxNPC = 3;
    [SerializeField] private float _spawnDelay = 0.5f; // задержка между появлением NPC
    
    [Header("Границы автобуса")]
    [SerializeField] private Transform _leftLimit;   // левая граница (куда идут налево)
    [SerializeField] private Transform _rightLimit;  // правая граница (куда идут направо)
    [SerializeField] private Transform _spawnPoint;  // точка спавна (двери)
    
    [Header("Настройки движения")]
    [SerializeField] private float _moveSpeed = 1.5f;
    
    private List<GameObject> _activeNPCs = new List<GameObject>();
    private bool _isSpawning = false;
    
    void Start()
    {
        if (_npcPrefab == null)
        {
            Debug.LogError("NPC Prefab не назначен!");
            return;
        }
        
        // Подписываемся на события (если есть)
        // Можно вызывать вручную из BusStopController
    }
    
    /// <summary>
    /// Запускает спавн NPC (вызывается из BusStopController при открытии дверей)
    /// </summary>
    public void SpawnNPCs()
    {
        if (_isSpawning) return;
        StartCoroutine(SpawnSequence());
    }
    
    private IEnumerator SpawnSequence()
    {
        _isSpawning = true;
        
        // Определяем количество NPC (случайное)
        int npcCount = Random.Range(_minNPC, _maxNPC + 1);
        
        // Спавним каждого с задержкой
        for (int i = 0; i < npcCount; i++)
        {
            SpawnSingleNPC();
            yield return new WaitForSeconds(_spawnDelay);
        }
        
        _isSpawning = false;
    }
    
    private void SpawnSingleNPC()
    {
        if (_npcPrefab == null || _spawnPoint == null) return;
        
        // Создаём NPC
        GameObject npc = Instantiate(_npcPrefab, _spawnPoint.position, Quaternion.identity);
        _activeNPCs.Add(npc);
        
        // Определяем направление (случайно)
        bool goRight = Random.Range(0, 2) == 1;
        
        // Настраиваем движение
        NPCWalker walker = npc.GetComponent<NPCWalker>();
        if (walker == null)
        {
            // Если нет скрипта NPCWalker — добавляем
            walker = npc.AddComponent<NPCWalker>();
        }
        
        walker.Initialize(goRight ? _rightLimit.position.x : _leftLimit.position.x, _moveSpeed);
        
        // Поворачиваем NPC в нужную сторону
        Vector3 scale = npc.transform.localScale;
        scale.x = goRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        npc.transform.localScale = scale;
        
        // Подписываемся на событие удаления
        walker.OnReachedDestination += () => RemoveNPC(npc);
    }
    
    private void RemoveNPC(GameObject npc)
    {
        if (_activeNPCs.Contains(npc))
        {
            _activeNPCs.Remove(npc);
            Destroy(npc);
        }
    }
    
    /// <summary>
    /// Удаляет всех NPC (вызывается при закрытии дверей)
    /// </summary>
    public void ClearAllNPCs()
    {
        foreach (GameObject npc in _activeNPCs)
        {
            if (npc != null) Destroy(npc);
        }
        _activeNPCs.Clear();
    }
}