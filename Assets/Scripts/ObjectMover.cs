using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ObjectMover : MonoBehaviour
{
    private CameraBound _bound;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _coroutine;

    public CollectableObject ObjectType { get; private set; }

    public event UnityAction Moved;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Moved += OnMoved;
    }

    private void OnDisable()
    {
        Moved -= OnMoved;

        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy();
    }

    public void Init(CameraBound bound, float scale)
    {
        _bound = bound;
        _transform.localScale = new Vector3(_transform.localScale.x * scale, _transform.localScale.y * scale, _transform.localScale.z * scale);
    }

    public void Spawn(CollectableObject enemyType)
    {
        ObjectType = enemyType;
        _spriteRenderer.sprite = ObjectType.Sprite;

        _coroutine = StartCoroutine(nameof(MoveLeft));
    }

    private IEnumerator MoveLeft()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while(gameObject.activeInHierarchy == true)
        {
            Move(Vector2.left);

            yield return waitForEndOfFrame;
        }
    }

    private void Move(Vector2 direction)
    {
        _transform.Translate(direction * ObjectType.Speed * Time.deltaTime, Space.World);

        Moved?.Invoke();
    }

    private void OnMoved()
    {
        if(transform.position.x < (_bound.MinX - _spriteRenderer.size.x / 2))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
