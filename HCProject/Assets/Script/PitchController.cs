using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchController : MonoBehaviour
{
    public BallProjectionController ballProjection;
    public PitcherSprite pitcherSprite;

    private PitchData pitch;
    private float startTime;
    private bool inputUsed;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Pitching)
            return;

        float elapsed = Time.time - startTime;
        float normalizedTime = elapsed / pitch.totalTime;

        if (!inputUsed && Input.GetKeyDown(KeyCode.Space))
        {
            inputUsed = true;
            OnSwing(normalizedTime);
        }

        if (elapsed >= pitch.totalTime)
            OnPitchFinished();
    }

    public void StartPitch(PitchData data)
    {
        pitch = data;
        startTime = Time.time;
        inputUsed = false;

        // 투수 스프라이트 변경
        if (pitcherSprite != null)
            pitcherSprite.PitchSprite();
        
        // BallProjection 시작
        if (ballProjection != null)
            ballProjection.StartProjection(pitch.totalTime);

        GameManager.Instance.ChangeState(GameState.Pitching);
    }

    private void OnSwing(float normalizedTime)
    {
        HitResult result = HitJudge.Judge(normalizedTime, pitch.hitStart, pitch.hitEnd);

        GameManager.Instance.ChangeState(GameState.Result);
        // Result;
    }

    private void OnPitchFinished()
    {
        if (inputUsed)
            return;

        GameManager.Instance.ChangeState(GameState.Result);

        
    }
}
