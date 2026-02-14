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

    [Header("Stage Count (Optional)")]
    [SerializeField] private StageCountSetup stageCountSetup;

    [Header("References")]
    public PitchController pitchController;

    [Header("Result UI")]
    [SerializeField] private GameObject strikeUI;
    [SerializeField] private GameObject ballUI;
    [SerializeField] private GameObject foulUI;
    [SerializeField] private GameObject hitUI;

    private PitchGenerator pitchGenerator;
    private CountState currentCountState;

    public CountState CurrentCount => currentCountState;

    void Awake()
    {
        Instance = this;
        pitchGenerator = new PitchGenerator();
        currentCountState = new CountState();
    }

    void Start()
    {
        InitializeCount(stageCountSetup);
        HideResultUI();
        StartCoroutine(GameLoop());
    }

    public void InitializeCount(StageCountSetup countSetup)
    {
        currentCountState.InitCountState(countSetup);
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

            ShowResultUI(pitchController.LastResult);

            yield return new WaitForSeconds(resultDelay);

            HideResultUI();
            pitchController.ResetPitch();
            pitchController.batterSprite.IdleSprite();
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
        else if ((result == HitResult.Perfect || result == HitResult.Good) && hitUI != null)
            hitUI.SetActive(true);
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
    }

    public void ChangeState(GameState state)
    {
        State = state;
    }
}
