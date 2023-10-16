using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHeatlh;

    [Header("Animation settings")]
    [SerializeField] private float _angleMax;
    [SerializeField] private float _angleForce;
    [SerializeField] private float _durationReturnToZero;
    [SerializeField] private float _durationUntilReturn;
    [SerializeField] private float _tickPerSecond;

    public event UnityAction Awaked;
    public event UnityAction Died;
    public event UnityAction Moved;  
    public event UnityAction DamageTaked;
    public event UnityAction Healed;

    public int MaxHeatlh => _maxHeatlh;
    public int Health => _health;

    private CameraBound _bound;
    private GameController _controller;
    private KeyListener _keyListener;
    private PlayerSpawner _spawner;

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private KeyBinder _keyBinder;
    private Coroutine _coroutine;

    private float _boundUp;
    private float _boundDown;

    private int _health;
    private bool _isAwake;
    private bool _isAlive;
    private bool _isInited;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Moved += OnMoved;

        if (_isInited == true)
        {
            _spawner.PlayerSpawned += OnPlayerSpawned;
            _controller.GameStoped += OnGameStoped;
            _keyListener.AnyKeyPressed += OnAnyKeyPressed;

            _keyBinder.BindCommand(InputTypes.KeyPressed, KeyCode.W, MoveUp);
            _keyBinder.BindCommand(InputTypes.KeyPressed, KeyCode.S, MoveDown);
        }
    }

    private void OnDisable()
    {
        Moved -= OnMoved;

        if (_isInited == true)
        {
            _spawner.PlayerSpawned -= OnPlayerSpawned;
            _controller.GameStoped -= OnGameStoped;
            _keyListener.AnyKeyPressed -= OnAnyKeyPressed;

            _keyBinder.UnbindAllCommands();
        }
    }

    public void Init(CameraBound cameraBound, KeyListener keyListener, GameController gameController, Sprite sprite, PlayerSpawner playerSpawner)
    {
        _controller = gameController;
        _keyListener = keyListener;
        _bound = cameraBound;
        _spriteRenderer.sprite = sprite;
        _spawner = playerSpawner;

        _boundUp = _bound.MaxY - _spriteRenderer.bounds.size.y / 2;
        _boundDown = _bound.MinY + _spriteRenderer.bounds.size.y / 2;

        _isAwake = false;
        _isAlive = true;

        _health = _maxHeatlh;

        _keyBinder = new KeyBinder(_keyListener);

        _spawner.PlayerSpawned += OnPlayerSpawned;
        _controller.GameStoped += OnGameStoped;
        _keyListener.AnyKeyPressed += OnAnyKeyPressed;

        _keyBinder.BindCommand(InputTypes.KeyPressed, KeyCode.W, MoveUp);
        _keyBinder.BindCommand(InputTypes.KeyPressed, KeyCode.S, MoveDown);

        _isInited = true;
    }

    public void Heal(int health)
    {
        if(health > 0)
        {
            _health = Mathf.Min(_health + health, _maxHeatlh);

            Healed?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        if(damage > 0)
        {
            if (_health <= damage)
            {
                _health = 0;

                Died?.Invoke();
            }
            else
            {
                _health -= damage;

                DamageTaked?.Invoke();
            }
        }
    }

    private void MoveUp() => Move(Vector2.up);

    private void MoveDown() => Move(Vector2.down);

    private void Move(Vector2 direction)
    {
        if(_isAwake == true)
        {
            Vector3 currentAngle = _transform.rotation.eulerAngles;

            _transform.Translate(direction * Time.deltaTime * _speed, Space.World);
            _transform.rotation = Quaternion.Euler(Mathf.Clamp(Utils.GetSignedAngle(currentAngle.x) + _angleForce * direction.y * Time.deltaTime, -_angleMax, _angleMax), currentAngle.y, currentAngle.z);

            Moved?.Invoke();
        }
    }

    private IEnumerator RotateToZero()
    {
        yield return new WaitForSeconds(_durationUntilReturn);

        WaitForSeconds waitForSeconds = new WaitForSeconds(_durationReturnToZero / _tickPerSecond);
        float startX = Utils.GetSignedAngle(_transform.rotation.eulerAngles.x);
        float targetX = 0;
        float delta = 1 / _tickPerSecond;
        float currentT = 0;

        while (Utils.GetSignedAngle(_transform.rotation.eulerAngles.x) != targetX)
        {
            currentT += delta;

            _transform.rotation = Quaternion.Euler(Mathf.Lerp(startX, targetX, currentT), 0, -90);

            yield return waitForSeconds;
        }
    }

    private void OnMoved()
    {
        if (transform.position.y > _boundUp || transform.position.y < _boundDown)
        {
            if (transform.position.y > _boundUp)
            {
                transform.position = new Vector3(transform.position.x, _boundUp, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, _boundDown, transform.position.z);
            }
        }

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(nameof(RotateToZero));
    }

    private void OnAnyKeyPressed()
    {
        if(_isAlive == true && _isAwake == false)
        {
            _isAwake = true;

            Awaked?.Invoke();
        }
    }

    private void OnPlayerSpawned()
    {
        _isAwake = false;
        _isAlive = true;

        _health = _maxHeatlh;
    }

    private void OnGameStoped()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ObjectMover mover))
        {
            mover.ObjectType.Action(this);
        }
    }
}
