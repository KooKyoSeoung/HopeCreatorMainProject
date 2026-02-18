using UnityEngine;

[System.Serializable]
public class InningSetup
{
    [Range(0, 2)]
    public int strikeCount = 0;

    [Range(0, 3)]
    public int ballCount = 0;

    [Range(0, 2)]
    public int outCount = 0;

    [Min(1)]
    public int inning = 1;

    // true : 초, false : 말
    public bool isTop = true;

    public bool runnerOnFirst = false;
    public bool runnerOnSecond = false;
    public bool runnerOnThird = false;
}

public class InningState
{
    public int StrikeCount { get; private set; }
    public int BallCount { get; private set; }
    public int OutCount { get; private set; }
    public int Inning { get; private set; }
    public bool IsTop { get; private set; }
    public bool RunnerOnFirst { get; private set; }
    public bool RunnerOnSecond { get; private set; }
    public bool RunnerOnThird { get; private set; }

    public void Initialize(InningSetup setup)
    {
        if (setup == null)
        {
            StrikeCount = 0;
            BallCount = 0;
            OutCount = 0;
            Inning = 1;
            IsTop = true;
            RunnerOnFirst = false;
            RunnerOnSecond = false;
            RunnerOnThird = false;
            return;
        }

        StrikeCount = Mathf.Clamp(setup.strikeCount, 0, 2);
        BallCount = Mathf.Clamp(setup.ballCount, 0, 3);
        OutCount = Mathf.Clamp(setup.outCount, 0, 2);
        Inning = Mathf.Max(1, setup.inning);
        IsTop = setup.isTop;
        RunnerOnFirst = setup.runnerOnFirst;
        RunnerOnSecond = setup.runnerOnSecond;
        RunnerOnThird = setup.runnerOnThird;
    }

    public void ApplyResult(HitResult result)
    {
        switch (result)
        {
            case HitResult.Strike:
                OnStrike();
                break;
            case HitResult.Ball:
                OnBall();
                break;
            case HitResult.Foul:
                OnFoul();
                break;
        }
    }

    private void OnStrike()
    {
        StrikeCount++;

        if (StrikeCount >= 3)
        {
            RecordOut();
        }
    }

    private void OnBall()
    {
        BallCount++;

        if (BallCount >= 4)
        {
            ApplyWalk();
            ResetCount();
        }
    }

    private void OnFoul()
    {
        if (StrikeCount < 2)
        {
            StrikeCount++;
        }
    }

    private void RecordOut()
    {
        OutCount++;
        ResetCount();

        if (OutCount >= 3)
        {
            AdvanceHalfInning();
        }
    }

    private void ApplyWalk()
    {
        if (RunnerOnFirst && RunnerOnSecond && RunnerOnThird)
        {
            // 득점 처리
            RunnerOnThird = true;
            RunnerOnSecond = true;
            RunnerOnFirst = true;
        }
        else if (RunnerOnFirst && RunnerOnSecond)
        {
            RunnerOnThird = true;
            RunnerOnSecond = true;
            RunnerOnFirst = true;
        }
        else if (RunnerOnFirst)
        {
            RunnerOnSecond = true;
            RunnerOnFirst = true;
        }
        else
        {
            RunnerOnFirst = true;
        }
    }

    private void AdvanceHalfInning()
    {
        OutCount = 0;
        ResetCount();
        ClearRunners();

        if (IsTop)
        {
            IsTop = false;
        }
        else
        {
            IsTop = true;
            Inning++;
        }
    }

    private void ResetCount()
    {
        StrikeCount = 0;
        BallCount = 0;
    }

    private void ClearRunners()
    {
        RunnerOnFirst = false;
        RunnerOnSecond = false;
        RunnerOnThird = false;
    }
}
