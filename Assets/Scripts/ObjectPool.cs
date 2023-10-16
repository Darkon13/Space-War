using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int _enemyCount;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private ObjectTypeHolder _enemyTypeHolder;
    [SerializeField] private GameController _gameController;
    [SerializeField] private CameraBound _bound;

    private List<Transform> _enemies;

    private void OnEnable()
    {
        _gameController.GameStarted += DisableAllObjects;
        _gameController.GameStoped += DisableAllObjects;
    }

    private void OnDisable()
    {
        _gameController.GameStarted -= DisableAllObjects;
        _gameController.GameStoped -= DisableAllObjects;
    }
    private void Awake()
    {
        _enemies = new List<Transform>();

        for (int i = 0; i < _enemyCount; i++)
        {
            GameObject enemy = Instantiate(_prefab, transform);

            if (enemy.TryGetComponent(out ObjectMover enemyMover))
            {
                enemyMover.Init(_bound, _enemyTypeHolder.Scale);
            }

            _enemies.Add(enemy.transform);
            enemy.SetActive(false);
        }
    }

    public bool TryToGetObject(out Transform transform)
    {
        transform = _enemies.FirstOrDefault(enemy => enemy.gameObject.activeInHierarchy == false);

        if(transform != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryToGetObjects(int count, out List<Transform> transform)
    {
        transform = _enemies.Where(enemy => enemy.gameObject.activeInHierarchy == false).Take(count).ToList();

        if (transform.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisableAllObjects()
    {
        _enemies.ForEach(enemy => enemy.gameObject.SetActive(false));
    }
}
