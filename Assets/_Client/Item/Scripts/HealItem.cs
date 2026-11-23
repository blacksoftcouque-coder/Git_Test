using UnityEngine;

// Player의 체력을 일정량 회복시키는 아이템.
public class HealItem : Item
{
    [SerializeField]
    private float healAmount = 30f;

    protected override void ApplyEffect(Player player)
    {
        player.Heal(healAmount);
    }
}
