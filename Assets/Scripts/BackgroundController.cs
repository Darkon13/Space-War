using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private GameObject _backgroundCanvas;

    private void OnEnable()
    {
        _gameController.GameStarted += OnGameStarted;
        _gameController.GameStoped += OnGameStoped;
    }

    private void OnDisable()
    {
        _gameController.GameStarted -= OnGameStarted;
        _gameController.GameStoped -= OnGameStoped;
    }

    private void OnGameStarted() => _backgroundCanvas.SetActive(true);
    private void OnGameStoped() => _backgroundCanvas.SetActive(false);
}
