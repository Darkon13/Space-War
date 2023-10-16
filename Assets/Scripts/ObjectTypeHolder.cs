using System.Collections.Generic;
using UnityEngine;

public class ObjectTypeHolder : MonoBehaviour
{
    [SerializeField] private List<CollectableObject> _objectTypes;
    [SerializeField] private float _scale;

    public float MaxWidth {get; private set;}
    public float MaxHeight { get; private set; }
    public IReadOnlyList<CollectableObject> ObjectTypes => _objectTypes;
    public float Scale => _scale;

    private void Awake()
    {
        Vector2 rect = GetMaxSpriteSize();

        MaxWidth = rect.x;
        MaxHeight = rect.y;
    }

    private Vector2 GetMaxSpriteSize()
    {
        Vector2 resultVector = new Vector2();

        foreach (CollectableObject enemy in _objectTypes)
        {
            float width = enemy.Sprite.rect.size.x / enemy.Sprite.pixelsPerUnit * _scale;
            float height = enemy.Sprite.rect.size.y / enemy.Sprite.pixelsPerUnit * _scale;

            if (resultVector.x < width)
            {
                resultVector.x = width;
            }

            if (resultVector.y < height)
            {
                resultVector.y = height;
            }
        }

        return resultVector;
    }
}
