using UnityEngine;

public enum StageResult
{
    Playing,
    Clear,
    Failed,
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Stage")]
    [SerializeField] private StageData currentStageData;

    public StageData CurrentStageData => currentStageData;
    public StageResult Result { get; private set; }

    private int scoreBefore;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (currentStageData != null)
            GameManager.Instance.StartStage(currentStageData);
    }

    public void InitStage(StageData stageData)
    {
        currentStageData = stageData;
        Result = StageResult.Playing;

        if (stageData != null && stageData.inningSetup != null)
        {
            GameManager.Instance.InitializeInning(stageData.inningSetup);
            scoreBefore = GetCurrentScore();
        }
        else
        {
            GameManager.Instance.InitializeInning(null);
            scoreBefore = 0;
        }
    }

    public void OnResultApplied(HitResult hitResult, InningState state)
    {
        if (Result != StageResult.Playing)
            return;

        if (CheckQuestComplete(state))
        {
            Result = StageResult.Clear;
            Debug.Log($"Stage Clear! : {currentStageData.stageName}");
            return;
        }

        if (state.OutCount >= 3)
        {
            Result = StageResult.Failed;
            Debug.Log($"Stage Failed 3 Outs : {currentStageData.stageName}");
            return;
        }
    }

    private bool CheckQuestComplete(InningState state)
    {
        if (currentStageData == null || currentStageData.quest == null)
            return false;

        StageQuest quest = currentStageData.quest;

        switch (quest.questType)
        {
            case QuestType.Score:
                int scored = GetCurrentScore() - scoreBefore;
                return scored >= quest.targetValue;
            case QuestType.Hit:
                return state.TotalHits >= quest.targetValue;
            case QuestType.Homerun:
                return state.TotalHomeruns >= quest.targetValue;
            default:
                return false;
        }
    }

    private int GetCurrentScore()
    {
        return GameManager.Instance.CurrentInning.ScoreTop;
    }
}


