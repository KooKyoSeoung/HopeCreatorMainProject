using UnityEngine;

[System.Serializable]
public class PitchData
{
    public float totalTime = 1.5f;

    public float hitStart = 0.65f;
    public float hitEnd = 0.75f;

    public bool isStrike;
    public PitchType pitchType;
    public Vector2 startPosition;
    public Vector2 targetPosition;
    public Vector2 controlPoint1;
    public Vector2 controlPoint2;
}