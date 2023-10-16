using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Create new object/Enemy", order = 51)]
public class Enemy : CollectableObject
{
    [SerializeField] private int _damage;

    public override void Action(Player player)
    {
        player.TakeDamage(_damage);
    }
}
