using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float _duration;
    [SerializeField] GameController _controller;

    public event UnityAction TimerEnded;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        TimerEnded += OnTimerEnd;

        _controller.GameStarted += OnGameStarted;
        _controller.GameStoped += OnGameStoped;
    }

    private void OnDisable()
    {
        TimerEnded -= OnTimerEnd;

        _controller.GameStarted -= OnGameStarted;
        _controller.GameStoped -= OnGameStoped;
    }

    private void OnGameStarted()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(nameof(StartTimer));
    }

    private void OnGameStoped()
    {
        if(_coroutine != null) 
            StopCoroutine(_coroutine);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_duration);

        TimerEnded?.Invoke();
    }

    private void OnTimerEnd()
    {
        _coroutine = StartCoroutine(nameof(StartTimer));
    }
}
