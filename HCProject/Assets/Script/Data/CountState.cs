using UnityEngine;

[System.Serializable]
public class StageCountSetup
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

public class CountState
{
    public int StrikeCount { get; private set; }
    public int BallCount { get; private set; }
    public int OutCount { get; private set; }
    public int Inning { get; private set; }
    public bool IsTop { get; private set; }
    public bool RunnerOnFirst { get; private set; }
    public bool RunnerOnSecond { get; private set; }
    public bool RunnerOnThird { get; private set; }

    public void InitCountState(StageCountSetup setup)
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
}
