using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawnPointSetter : MonoBehaviour
{
    [SerializeField] private CameraBound _bound;
    [SerializeField] private ObjectTypeHolder _enemyTypeHolder;
    
    private const string SpawnerName = "EnemySpawnPoint";

    public event UnityAction<List<Transform>> Inited;

    private GameObject _prefab;
    private List<Transform> _spawners;

    private void Start()
    {
        _prefab = new GameObject(SpawnerName);
        _spawners = new List<Transform>();

        float height = _bound.MaxY - _bound.MinY;
        int spawnerCount = (int)(height / _enemyTypeHolder.MaxHeight);

        float posX = _bound.MaxX + _enemyTypeHolder.MaxWidth / 2;
        float startPosY = _bound.MinY + _enemyTypeHolder.MaxHeight / 2 + ((height % _enemyTypeHolder.MaxHeight) / spawnerCount) / 2;

        for (int i = 0; i < spawnerCount; i++)
        {
            GameObject spawner = Instantiate(_prefab, new Vector2(posX, startPosY + height / spawnerCount * i), Quaternion.identity, transform);
            _spawners.Add(spawner.transform);
        }

        Inited?.Invoke(_spawners);
    }
}
