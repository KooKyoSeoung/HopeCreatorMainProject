using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Board : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreTopText;
    [SerializeField] private TextMeshProUGUI scoreBottomText;

    [Header("Inning")]
    [SerializeField] private TextMeshProUGUI inningText;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;

    [Header("Count")]
    [SerializeField] private TextMeshProUGUI strikeCountText;
    [SerializeField] private TextMeshProUGUI ballCountText;

    [Header("Out")]
    [SerializeField] private Image out1;
    [SerializeField] private Image out2;
    [SerializeField] private Sprite outFullSprite;
    [SerializeField] private Sprite outNullSprite;

    [Header("Base")]
    [SerializeField] private Image base1;
    [SerializeField] private Image base2;
    [SerializeField] private Image base3;
    [SerializeField] private Sprite baseFullSprite;
    [SerializeField] private Sprite baseNullSprite;

    public void UpdateBoard(InningState state)
    {
        UpdateScore(state);
        UpdateInning(state);
        UpdateCount(state);
        UpdateOut(state);
        UpdateBase(state);
    }

    private void UpdateScore(InningState state)
    {
        if (scoreTopText != null)
            scoreTopText.text = state.ScoreTop.ToString();

        if (scoreBottomText != null)
            scoreBottomText.text = state.ScoreBottom.ToString();
    }

    private void UpdateInning(InningState state)
    {
        if (inningText != null)
            inningText.text = state.Inning.ToString();

        if (upArrow != null)
            upArrow.SetActive(state.IsTop);

        if (downArrow != null)
            downArrow.SetActive(!state.IsTop);
    }

    private void UpdateCount(InningState state)
    {
        if (strikeCountText != null)
            strikeCountText.text = state.StrikeCount.ToString();

        if (ballCountText != null)
            ballCountText.text = state.BallCount.ToString();
    }

    private void UpdateOut(InningState state)
    {
        if (out1 != null)
            out1.sprite = state.OutCount >= 1 ? outFullSprite : outNullSprite;

        if (out2 != null)
            out2.sprite = state.OutCount >= 2 ? outFullSprite : outNullSprite;
    }

    private void UpdateBase(InningState state)
    {
        if (base1 != null)
            base1.sprite = state.RunnerOnFirst ? baseFullSprite : baseNullSprite;

        if (base2 != null)
            base2.sprite = state.RunnerOnSecond ? baseFullSprite : baseNullSprite;

        if (base3 != null)
            base3.sprite = state.RunnerOnThird ? baseFullSprite : baseNullSprite;
    }
}

