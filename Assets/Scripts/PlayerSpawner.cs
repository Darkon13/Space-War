using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Sprite _playerSprite;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CameraBound _bound;
    [SerializeField] private GameController _controller;
    [SerializeField] private KeyListener _keyListener;

    public event UnityAction<Player> PlayerInited;
    public event UnityAction PlayerSpawned;

    private const string SpawnerName = "PlayerSpawnPoint";
    private Transform _spawnerTransform;
    private GameObject _player;

    private void Start()
    {
        float posX = _bound.MinX + (_playerSprite.rect.width / _playerSprite.pixelsPerUnit * _playerPrefab.transform.localScale.x) / 2;
        float posY = _bound.MinY + (_bound.MaxY - _bound.MinY) / 2;

        _spawnerTransform = Instantiate(new GameObject(SpawnerName), GetComponent<Transform>()).transform;
        _spawnerTransform.position = new Vector2(posX, posY);

        _player = Instantiate(_playerPrefab);

        if(_player.TryGetComponent(out Player player))
        {
            player.Init(_bound, _keyListener, _controller, _playerSprite, this);

            PlayerInited?.Invoke(player);
        }

        _player.SetActive(false);
    }

    private void OnEnable()
    {
        _controller.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _controller.GameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        float startRotationZ = -90;

        _player.transform.position = _spawnerTransform.position;
        _player.transform.rotation = Quaternion.Euler(0, 0, startRotationZ);
        _player.SetActive(true);

        PlayerSpawned?.Invoke();
    }
}
