using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] ObjectSpawnPointSetter _spawnPointSetter;
    [SerializeField] ObjectTypeHolder _objectTypeHolder;
    [SerializeField] ObjectPool _pool;
    [SerializeField] Timer _spawnTimer;

    private List<Transform> _spawnPoints;

    private void OnEnable()
    {
        _spawnPointSetter.Inited += Init;
        _spawnTimer.TimerEnded += Spawn;
    }

    private void OnDisable()
    {
        _spawnPointSetter.Inited -= Init;
        _spawnTimer.TimerEnded -= Spawn;
    }

    private void Init(List<Transform> spawnPoints) => _spawnPoints = spawnPoints;

    private void Spawn()
    {
        List<Transform> spawnPoints = ChoosePoints(_spawnPoints.Count);

        if (_pool.TryToGetObjects(spawnPoints.Count, out List<Transform> objects))
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].position = spawnPoints[i].position;
                objects[i].gameObject.SetActive(true);

                if (objects[i].TryGetComponent(out ObjectMover objectMover))
                {
                    objectMover.Spawn(_objectTypeHolder.ObjectTypes[Random.Range(0, _objectTypeHolder.ObjectTypes.Count)]);
                }
            }
        }
    }

    private List<Transform> ChoosePoints(int maxPoints, int minPoints = 1)
    {
        int pointsCount = Random.Range(minPoints, maxPoints);
        List<Transform> result = new List<Transform>();

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (pointsCount == (_spawnPoints.Count - i))
            {
                result.Add(_spawnPoints[i]);

                pointsCount--;
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    result.Add(_spawnPoints[i]);

                    pointsCount--;
                }
            }

            if (pointsCount == 0)
                break;
        }

        return result;
    }
}
