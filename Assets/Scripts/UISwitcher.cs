using UnityEngine;
using UnityEngine.Events;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] GameController _gameController;
    [SerializeField] PlayerSpawner _playerSpawner;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] KeyListener _keyListener;
    [SerializeField] RectTransform _mainMenuPanel;
    [SerializeField] RectTransform _deathScreenPanel;
    [SerializeField] RectTransform _hudPanel;

    public event UnityAction MainMenuOpened;

    private KeyBinder keyBinder;

    private void Awake()
    {
        keyBinder = new KeyBinder(_keyListener);

        DisableAllPanels();
        EnableMainMenu();
    }

    private void OnEnable()
    {
        _playerSpawner.PlayerInited += InitHealthBar;
        _gameController.GameStarted += RefreshHealthBar;
        _gameController.GameStarted += EnableHUD;
        _gameController.PlayerDied += EnableDeathScreen;

        keyBinder.BindCommand(InputTypes.KeyDown, KeyCode.Escape, EnableMainMenu);
    }

    private void OnDisable()
    {
        _playerSpawner.PlayerInited -= InitHealthBar;
        _gameController.GameStarted -= RefreshHealthBar;
        _gameController.GameStarted -= EnableHUD;
        _gameController.PlayerDied -= EnableDeathScreen;

        keyBinder.UnbindAllCommands();
    }

    public void EnableMainMenu()
    {
        EnablePanel(_mainMenuPanel);

        MainMenuOpened?.Invoke();
    }

    private void EnableDeathScreen() => EnablePanel(_deathScreenPanel);

    private void EnableHUD() => EnablePanel(_hudPanel);

    private void InitHealthBar(Player player)
    {
        _healthBar.Init(player);
    }

    private void RefreshHealthBar()
    {
        _healthBar.Refresh();
    }

    private void EnablePanel(RectTransform panel)
    {
        if(panel.gameObject.activeInHierarchy == false)
        {
            DisableAllPanels();
            panel.gameObject.SetActive(true);
        }
    }

    private void DisableAllPanels()
    {
        _mainMenuPanel.gameObject.SetActive(false);
        _deathScreenPanel.gameObject.SetActive(false);
        _hudPanel.gameObject.SetActive(false);
    }
}
