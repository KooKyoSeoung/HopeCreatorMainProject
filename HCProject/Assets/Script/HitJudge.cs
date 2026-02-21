using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitJudge : MonoBehaviour
{
    public static HitResult Judge(float time, float start, float end, bool isStrike)
    {
        if (time < start || time > end)
            return HitResult.Strike;

        if (!isStrike)
            return HitResult.Foul;

        float center = (start + end) * 0.5f;
        float diff = Mathf.Abs(time - center);

        if (diff < 0.4f)
            return HitResult.Perfect;

        return HitResult.Good;
    }
}

public enum HitResult
{
    Perfect,
    Good,
    Foul,
    Strike,
    Ball
}
