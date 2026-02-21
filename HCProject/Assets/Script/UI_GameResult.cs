using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_GameResult : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Character")]
    [SerializeField] private Image charImage;
    [SerializeField] private Sprite clearSprite;
    [SerializeField] private Sprite overSprite;
    [SerializeField] private TextMeshProUGUI characterText;

    [Header("Info Dynamic")]
    [SerializeField] private TextMeshProUGUI outCountText;
    [SerializeField] private TextMeshProUGUI hitCountText;
    [SerializeField] private TextMeshProUGUI scoreCountText;

    public void Show(InningState state, StageResult stageResult, int scoreBefore)
    {
        bool isClear = stageResult == StageResult.Clear;

        if (titleText != null)
            titleText.text = isClear ? "게임 클리어!" : "게임 오버";

        if (charImage != null)
            charImage.sprite = isClear ? clearSprite : overSprite;

        if (characterText != null)
            characterText.text = isClear ? "와아! 우리가 이겼어요!!" : "우으... 져버렸어요..";

        if (outCountText != null)
            outCountText.text = state.OutCount.ToString();

        if (hitCountText != null)
            hitCountText.text = state.TotalHits.ToString();

        if (scoreCountText != null)
            scoreCountText.text = (state.ScoreTop - scoreBefore).ToString();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

