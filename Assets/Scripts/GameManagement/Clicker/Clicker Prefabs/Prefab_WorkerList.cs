using UnityEngine;
using System.Collections.Generic;

public class Prefab_WorkerList : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] GameObject workerStatPrefab;
    [SerializeField] Transform container;

    [Header("Data:")]
    [SerializeField] List<Worker> allWorkerTemplates;

    public void UpdateWorkerList(List<int> workerLevels)
    {
        if (container == null || workerStatPrefab == null) return;

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < allWorkerTemplates.Count; i++)
        {
            if (i < workerLevels.Count && workerLevels[i] > 0)
            {
                GameObject newStat = Instantiate(workerStatPrefab, container);
                Prefab_WorkerStats statScript = newStat.GetComponent<Prefab_WorkerStats>();

                if (statScript != null)
                {
                    statScript.Setup(allWorkerTemplates[i].icon, workerLevels[i]);
                }
            }
        }
    }
}
