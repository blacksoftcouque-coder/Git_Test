using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 생성할 아이템 Prefab 목록. (HealItem 등, 나중에 종류 추가 가능)
    [SerializeField]
    private Item[] itemPrefabs = null;

    // 아이템이 생성될 바닥.
    [SerializeField]
    private Renderer floorRenderer = null;

    [SerializeField]
    private int maxItemCount = 3;

    [SerializeField]
    private float spawnInterval = 5f;

    // 벽에 붙어서 생성되지 않도록 바닥 가장자리에서 띄울 거리.
    [SerializeField]
    private float edgePadding = 2f;

    // 바닥 위로 띄울 높이.
    [SerializeField]
    private float spawnHeight = 0.5f;

    private List<Item> items = null;

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Init()
    {
        StartCoroutine(CoSpawnItem());
    }

    private IEnumerator CoSpawnItem()
    {
        while (true)
        {
            // Time.timeScale = 0 (게임 오버 / 클리어) 일 때는 생성이 멈춤.
            yield return new WaitForSeconds(spawnInterval);

            if (items.Count < maxItemCount)
                SpawnItem();
        }
    }

    private void SpawnItem()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0 || floorRenderer == null)
        {
            Debug.LogWarning("[ItemManager] itemPrefabs 또는 floorRenderer가 설정되지 않았습니다.");
            return;
        }

        Item prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        Item item = Instantiate(prefab, GetRandomFloorPosition(), Quaternion.identity, transform);
        item.Init(OnItemPicked);
        items.Add(item);
    }

    private Vector3 GetRandomFloorPosition()
    {
        Bounds bounds = floorRenderer.bounds;

        float x = Random.Range(bounds.min.x + edgePadding, bounds.max.x - edgePadding);
        float z = Random.Range(bounds.min.z + edgePadding, bounds.max.z - edgePadding);
        float y = bounds.max.y + spawnHeight;

        return new Vector3(x, y, z);
    }

    private void OnItemPicked(Item item)
    {
        items.Remove(item);
    }
}
