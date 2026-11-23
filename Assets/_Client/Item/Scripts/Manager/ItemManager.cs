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

    // 생성 위치에 다른 오브젝트가 있는지 검사할 반경. (spawnHeight보다 작아야 바닥에 걸리지 않음)
    [SerializeField]
    private float spawnCheckRadius = 0.4f;

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
        Vector3 position = Vector3.zero;

        // 기둥, 상자 같은 소품 안에 생성되지 않도록 빈 자리를 찾을 때까지 몇 번 다시 뽑음.
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(bounds.min.x + edgePadding, bounds.max.x - edgePadding);
            float z = Random.Range(bounds.min.z + edgePadding, bounds.max.z - edgePadding);
            float y = bounds.max.y + spawnHeight;
            position = new Vector3(x, y, z);

            if (!Physics.CheckSphere(position, spawnCheckRadius, ~0, QueryTriggerInteraction.Ignore))
                break;
        }

        return position;
    }

    private void OnItemPicked(Item item)
    {
        items.Remove(item);
    }
}
