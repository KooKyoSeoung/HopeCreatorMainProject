using UnityEngine;

public enum QuestType
{
    Score,
    Hit,
    Homerun,
}

[System.Serializable]
public class StageQuest
{
    public QuestType questType;
    public int targetValue = 1;
    [TextArea] public string description;
}

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    public string stageName;
    public InningSetup inningSetup;
    public StageQuest quest;
}


