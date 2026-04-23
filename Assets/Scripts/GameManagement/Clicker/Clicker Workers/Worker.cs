using UnityEngine;

[CreateAssetMenu(fileName = "NewWorker", menuName = "Clicker/Worker")]
public class Worker : ScriptableObject
{
    [Header("Main Information")]
    public string workerName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Stats:")]
    public int basePower;
    public int basePrice;

    public Worker Clone()
    {
        return (Worker)this.MemberwiseClone();
    }
}