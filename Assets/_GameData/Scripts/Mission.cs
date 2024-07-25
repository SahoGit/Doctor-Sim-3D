using UnityEngine;

[CreateAssetMenu(fileName="Mission", menuName="Missions/Create New Mission", order=1)]
public class Mission : ScriptableObject {
    public int missionNumber;
    public string missionDescription;

    [Header("For Time Based Missions")]
    public bool isTimeBased = true;
    public int missionTime = 90;   
}