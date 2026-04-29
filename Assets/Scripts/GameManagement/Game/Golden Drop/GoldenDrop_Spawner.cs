using UnityEngine;
using System.Collections;

public class GoldenDrop_Spawner : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] GameObject goldenDropPrefab;
    [SerializeField] RectTransform spawnArea;

    [Header("Spawn Settings:")]
    [SerializeField] float minSpawnTime = 5f;
    [SerializeField] float maxSpawnTime = 15f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnGoldenDrop();
        }
    }

    void SpawnGoldenDrop()
    {
        if (goldenDropPrefab == null || spawnArea == null) return;

        GameObject drop = Instantiate(goldenDropPrefab, spawnArea);
        RectTransform dropRect = drop.GetComponent<RectTransform>();

        float width = spawnArea.rect.width / 2;
        float randomX = Random.Range(-width + 50f, width - 50f);
        float startY = (spawnArea.rect.height / 2) + 100f;

        dropRect.anchoredPosition = new Vector2(randomX, startY);
    }
}
