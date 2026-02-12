using System.Collections.Generic;
using UnityEngine;

public class PitchGenerator
{
    private const float STRIKE_X = 0.675f;
    private const float STRIKE_Y = 0.9f;
    private const float BALL_X = 0.9f;
    private const float BALL_Y = 1.25f;

    private struct PitchTypeConfig
    {
        public float totalTime;
        public Vector2 startOffset;
        public Vector2 cpOffset1;
        public Vector2 cpOffset2;
        public float hitStart;
        public float hitEnd;
    }

    private static readonly Dictionary<PitchType, PitchTypeConfig> pitchConfigs = new Dictionary<PitchType, PitchTypeConfig>
    {
        { PitchType.Fastball, new PitchTypeConfig {
            totalTime = 1.0f, startOffset = new Vector2(0f, 0.5f),
            cpOffset1 = new Vector2(0f, 0f),
            cpOffset2 = new Vector2(0f, 0f),
            hitStart = 0.60f, hitEnd = 0.90f
        }},
        { PitchType.Curve, new PitchTypeConfig {
            totalTime = 1.8f, startOffset = new Vector2(0f, 1.4f),
            cpOffset1 = new Vector2(0f, 1.3f),
            cpOffset2 = new Vector2(0f, 1.6f),
            hitStart = 1.4f, hitEnd = 1.7f
        }},
        { PitchType.Slider, new PitchTypeConfig {
            totalTime = 1.3f, startOffset = new Vector2(0.3f, 0.9f),
            cpOffset1 = new Vector2(0f, 0.4f),
            cpOffset2 = new Vector2(-0.8f, 0.4f),
            hitStart = 0.9f, hitEnd = 1.2f
        }},
        { PitchType.Changeup, new PitchTypeConfig {
            totalTime = 2.0f, startOffset = new Vector2(-0.8f, 0.7f),
            cpOffset1 = new Vector2(0f, 0.6f),
            cpOffset2 = new Vector2(0f, 0.9f),
            hitStart = 1.6f, hitEnd = 1.9f
        }},
        { PitchType.Cutter, new PitchTypeConfig {
            totalTime = 1.1f, startOffset = new Vector2(0.2f, 0.6f),
            cpOffset1 = new Vector2(0f, 0.2f),
            cpOffset2 = new Vector2(-0.8f, 0.2f),
            hitStart = 0.7f, hitEnd = 1.0f
        }},
    };

    public PitchData Generate()
    {
        PitchData pitchData = new PitchData();

        // 일단 확률은 50%
        pitchData.isStrike = (Random.Range(0, 2) == 0);

        if (pitchData.isStrike)
        {
            float x = Random.Range(-STRIKE_X, STRIKE_X);
            float y = Random.Range(-STRIKE_Y, STRIKE_Y);
            pitchData.targetPosition = new Vector2(x, y);
        }
        else
        {
            pitchData.targetPosition = GetRandomBallPosition();
        }

        pitchData.pitchType = (PitchType)Random.Range(0, 5);
        ApplyPitchConfig(pitchData);

        return pitchData;
    }

    private void ApplyPitchConfig(PitchData pitchData)
    {
        PitchTypeConfig config = pitchConfigs[pitchData.pitchType];

        pitchData.totalTime = config.totalTime;
        pitchData.hitStart = config.hitStart;
        pitchData.hitEnd = config.hitEnd;

        pitchData.startPosition = pitchData.targetPosition + new Vector2(-config.startOffset.x, config.startOffset.y);

        Vector2 onePoint = Vector2.Lerp(pitchData.startPosition, pitchData.targetPosition, 1f / 3f);
        Vector2 twoPoint = Vector2.Lerp(pitchData.startPosition, pitchData.targetPosition, 2f / 3f);

        pitchData.controlPoint1 = onePoint + config.cpOffset1;
        pitchData.controlPoint2 = twoPoint + config.cpOffset2;
    }

    private Vector2 GetRandomBallPosition()
    {
        int region = Random.Range(0, 4);

        switch (region)
        {
            case 0: // 위
                return new Vector2(
                    Random.Range(-BALL_X, BALL_X),
                    Random.Range(STRIKE_Y, BALL_Y)
                );
            case 1: // 아래
                return new Vector2(
                    Random.Range(-BALL_X, BALL_X),
                    Random.Range(-BALL_Y, -STRIKE_Y)
                );
            case 2: // 왼쪽
                return new Vector2(
                    Random.Range(-BALL_X, -STRIKE_X),
                    Random.Range(-BALL_Y, BALL_Y)
                );
            case 3: // 오른쪽
                return new Vector2(
                    Random.Range(STRIKE_X, BALL_X),
                    Random.Range(-BALL_Y, BALL_Y)
                );
        }

        return Vector2.zero;
    }
}

