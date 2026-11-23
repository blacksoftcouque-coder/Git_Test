using System;
using UnityEngine;

// 모든 아이템의 공통 부모 클래스.
// Player와 충돌하면 효과를 적용하고, ItemManager에게 알린 뒤 사라짐.
public abstract class Item : MonoBehaviour
{
    private Action<Item> onItemPicked = null;

    public void Init(Action<Item> onItemPicked)
    {
        this.onItemPicked = onItemPicked;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null)
            return;

        ApplyEffect(player);

        onItemPicked?.Invoke(this);
        Destroy(gameObject);
    }

    // 아이템 종류별 효과. (Heal, 이속 증가 등)
    protected abstract void ApplyEffect(Player player);
}
