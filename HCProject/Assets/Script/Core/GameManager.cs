using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }

    [Header("Loop Setting")]
    public float readyDelay = 3f;
    public float resultDelay = 3f;

    public PitchController pitchController;

    private const float STRIKE_X = 0.675f;
    private const float STRIKE_Y = 0.9f;
    private const float BALL_X = 0.9f;
    private const float BALL_Y = 1.25f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(readyDelay);
        
        PitchData pitchData = GenerateRandomPitch();
        pitchController.StartPitch(pitchData);
    }

    private PitchData GenerateRandomPitch()
    {
        PitchData pitchData = new PitchData();
        
        // 일단 확률은 50%
        pitchData.isStrike = Random.Range(0, 2) == 0;
        
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
        
        return pitchData;
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

    public void ChangeState(GameState state)
    {
        State = state;
    }
}
