using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }

    [Header("Loop Setting")]
    [SerializeField] private float readyDelay = 3f;
    [SerializeField] private float resultDelay = 3f;

    [Header("References")]
    public PitchController pitchController;
    public UI_Board uiBoard;
    public UI_Quest uiQuest;
    public UI_GameResult uiGameResult;

    [Header("Result UI")]
    [SerializeField] private GameObject strikeUI;
    [SerializeField] private GameObject ballUI;
    [SerializeField] private GameObject foulUI;
    [SerializeField] private GameObject hitUI;
    [SerializeField] private GameObject homerunUI;

    private PitchGenerator pitchGenerator;
    private InningState currentInningState;
    private int scoreBefore;

    public InningState CurrentInning => currentInningState;

    void Awake()
    {
        Instance = this;
        pitchGenerator = new PitchGenerator();
        currentInningState = new InningState();
    }

    void Start()
    {
        HideResultUI();
    }

    public void StartStage(StageData stageData)
    {
        StageManager.Instance.InitStage(stageData);
        UpdateBoard();
        UpdateQuest(stageData);
        HideResultUI();
        HideGameResult();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM("BGM");

        StopAllCoroutines();
        StartCoroutine(GameLoop());
    }

    public void InitializeInning(InningSetup setup)
    {
        currentInningState.Initialize(setup);
        if (setup != null)
            scoreBefore = setup.isTop ? setup.scoreTop : setup.scoreBottom;
        else
            scoreBefore = 0;
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            ChangeState(GameState.Idle);
            yield return new WaitForSeconds(readyDelay);

            PitchData pitchData = pitchGenerator.Generate();
            pitchController.StartPitch(pitchData);

            yield return new WaitUntil(() => State == GameState.Result);

            HitResult result = pitchController.LastResult;
            currentInningState.ApplyResult(result);
            StageManager.Instance.OnResultApplied(result, currentInningState);
            UpdateBoard();
            ShowResultUI(result);

            yield return new WaitForSeconds(resultDelay);

            HideResultUI();
            pitchController.ResetPitch();
            pitchController.batterSprite.IdleSprite();

            StageResult stageResult = StageManager.Instance.Result;
            if (stageResult == StageResult.Clear || stageResult == StageResult.Failed)
            {
                ChangeState(GameState.Idle);
                ShowGameResult(stageResult);
                yield break;
            }
        }
    }

    private void ShowResultUI(HitResult result)
    {
        if (result == HitResult.Strike && strikeUI != null)
            strikeUI.SetActive(true);
        else if (result == HitResult.Ball && ballUI != null)
            ballUI.SetActive(true);
        else if (result == HitResult.Foul && foulUI != null)
            foulUI.SetActive(true);
        else if (result == HitResult.Good && hitUI != null)
            hitUI.SetActive(true);
        else if (result == HitResult.Perfect && homerunUI != null)
            homerunUI.SetActive(true);
    }

    private void HideResultUI()
    {
        if (strikeUI != null)
            strikeUI.SetActive(false);
        if (ballUI != null)
            ballUI.SetActive(false);
        if (foulUI != null)
            foulUI.SetActive(false);
        if (hitUI != null)
            hitUI.SetActive(false);
        if (homerunUI != null)
            homerunUI.SetActive(false);
    }

    private void UpdateBoard()
    {
        if (uiBoard != null)
            uiBoard.UpdateBoard(currentInningState);
    }

    private void UpdateQuest(StageData stageData)
    {
        if (uiQuest != null)
            uiQuest.SetQuest(stageData);
    }

    private void ShowGameResult(StageResult stageResult)
    {
        if (uiGameResult != null)
            uiGameResult.Show(currentInningState, stageResult, scoreBefore);
    }

    private void HideGameResult()
    {
        if (uiGameResult != null)
            uiGameResult.Hide();
    }

    public void ChangeState(GameState state)
    {
        State = state;
    }
}
