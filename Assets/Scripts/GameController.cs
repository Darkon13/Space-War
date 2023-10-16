using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    [SerializeField] private UISwitcher _switcher;

    public event UnityAction GameStarted;
    public event UnityAction GameStoped;

    public event UnityAction PlayerDied;
    public event UnityAction PlayerAwaked;

    private Player _player;
    private bool _isPlayerInited;

    private void Awake()
    {
        Time.timeScale = 0;
        _isPlayerInited = false;
    }

    private void OnEnable()
    {
        _spawner.PlayerInited += OnPlayerInited;
        _switcher.MainMenuOpened += OnMainMenuOpened;

        if (_isPlayerInited == true)
        {
            _player.Awaked += OnPlayerAwaked;
            _player.Died += OnPlayerDead;
        }
    }

    private void OnDisable()
    {
        _spawner.PlayerInited -= OnPlayerInited;
        _switcher.MainMenuOpened -= OnMainMenuOpened;

        if (_isPlayerInited == true)
        {
            _player.Awaked -= OnPlayerAwaked;
            _player.Died -= OnPlayerDead;
        }
    }

    private void OnPlayerInited(Player player)
    {
        _player = player;

        _player.Awaked += OnPlayerAwaked;
        _player.Died += OnPlayerDead;

        _isPlayerInited = true;
    }

    private void OnMainMenuOpened()
    {
        Time.timeScale = 0;

        GameStoped?.Invoke();
    }

    private void OnPlayerAwaked()
    {
        Time.timeScale = 1;

        PlayerAwaked?.Invoke();
    }

    private void OnPlayerDead()
    {
        Time.timeScale = 0;

        PlayerDied?.Invoke();
    }

    public void StartGame()
    {
        GameStarted?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
