using UnityEngine;

[CreateAssetMenu(fileName = "New MedKit", menuName = "Create new object/MedKit", order = 51)]
public class MedKit : CollectableObject
{
    [SerializeField] private int _health;

    public override void Action(Player player)
    {
        player.Heal(_health);
    }
}
