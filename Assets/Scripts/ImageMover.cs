using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ImageMover : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private float _duration;
    [SerializeField] private float _tickPerSecond;

    public event UnityAction ImageMoved;

    private RawImage _rawImage;
    private Coroutine _coroutine;

    void Awake()
    {
        _rawImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        ChangeUVRectX(0);

        ImageMoved += OnMoved;
        _gameController.PlayerAwaked += OnPlayerAwaked;
        _gameController.GameStoped += OnGameStoped;
    }

    private void OnDisable()
    {
        ImageMoved -= OnMoved;
        _gameController.PlayerAwaked -= OnPlayerAwaked;
        _gameController.GameStoped -= OnGameStoped;
    }

    private void OnMoved()
    {
        ChangeUVRectX(0);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(nameof(MoveImage));
    }

    private void OnPlayerAwaked()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(nameof(MoveImage));
    }
    private void OnGameStoped()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator MoveImage()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_duration / _tickPerSecond);
        float delta = 1 / _duration / _tickPerSecond;
        float startX = 0;
        float targetX = 1;

        while(_rawImage.uvRect.x != targetX)
        {
            ChangeUVRectX(Mathf.Min(_rawImage.uvRect.x + delta, targetX));

            yield return waitForSeconds;
        }

        ChangeUVRectX(startX);

        ImageMoved?.Invoke();
    }


    private void ChangeUVRectX(float x)
    {
        _rawImage.uvRect = new Rect(x, _rawImage.uvRect.y, _rawImage.uvRect.width, _rawImage.uvRect.height);
    }
}
