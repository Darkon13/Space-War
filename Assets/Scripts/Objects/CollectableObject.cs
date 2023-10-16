using UnityEngine;

public abstract class CollectableObject : ScriptableObject
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected float _speed;

    public Sprite Sprite  => _sprite;
    public float Speed => _speed;

    public abstract void Action(Player player);
}
