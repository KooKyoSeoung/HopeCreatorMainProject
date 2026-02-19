using UnityEngine;
using TMPro;

public class UI_Quest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questBody;

    public void SetQuest(StageData stageData)
    {
        if (stageData == null)
            return;

        if (questTitle != null)
            questTitle.text = stageData.stageName;

        if (questBody != null && stageData.quest != null)
            questBody.text = stageData.quest.description;
    }
}

