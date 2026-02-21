using UnityEngine;
using UnityEngine.UI;

public class UI_GameStart : MonoBehaviour
{
    [SerializeField] private Button startButton;

    void Start()
    {
        gameObject.SetActive(true);
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        gameObject.SetActive(false);
        if (StageManager.Instance != null && StageManager.Instance.CurrentStageData != null)
            GameManager.Instance.StartStage(StageManager.Instance.CurrentStageData);
    }
}
