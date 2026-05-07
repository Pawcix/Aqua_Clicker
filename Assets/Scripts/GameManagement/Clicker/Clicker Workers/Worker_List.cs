using UnityEngine;
using System.Collections.Generic;

public class Worker_List : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] List<Worker> availableWorkers;
    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] Transform container;

    List<Worker_Element> spawnedElements = new List<Worker_Element>();

    void Start()
    {
        if (data == null)
        {
            return;
        }

        PrepareDataSlots();
        GenerateShop();
    }

    void PrepareDataSlots()
    {
        while (data.workerLevels.Count < availableWorkers.Count)
        {
            data.workerLevels.Add(0);
        }
    }

    public void GenerateShop()
    {
        spawnedElements.Clear();
        foreach (Transform child in container) Destroy(child.gameObject);

        for (int i = 0; i < availableWorkers.Count; i++)
        {
            if (availableWorkers[i] == null) continue;

            GameObject newItem = Instantiate(shopItemPrefab, container);
            Worker_Element ui = newItem.GetComponent<Worker_Element>();

            if (ui != null)
            {
                ui.Setup(availableWorkers[i], i, data);
                spawnedElements.Add(ui);
            }
        }
    }

    public void RefreshAllElements()
    {
        foreach (Worker_Element element in spawnedElements)
        {
            if (element != null) element.UpdateUI();
        }
    }
}