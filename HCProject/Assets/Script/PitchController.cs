using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchController : MonoBehaviour
{
    public BallProjectionController ballProjection;
    public PitcherSprite pitcherSprite;
    public BatterSprite batterSprite;

    public HitResult LastResult { get; private set; }

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

        if (pitcherSprite != null)
            pitcherSprite.PitchSprite();
        
        if (ballProjection != null)
            ballProjection.StartProjection(pitch.totalTime, pitch.startPosition, pitch.targetPosition, pitch.controlPoint1, pitch.controlPoint2);

        SoundManager.Instance.PlaySFX("Pitch_Release");
        GameManager.Instance.ChangeState(GameState.Pitching);
    }

    public void ResetPitch()
    {
        if (pitcherSprite != null)
            pitcherSprite.IdleSprite();

        if (ballProjection != null)
            ballProjection.ResetProjection();
    }

    private void OnSwing(float normalizedTime)
    {
        LastResult = HitJudge.Judge(normalizedTime, pitch.hitStart, pitch.hitEnd, pitch.isStrike);

        SoundManager.Instance.PlaySFX("Bat_Swing");

        if (LastResult == HitResult.Foul || LastResult == HitResult.Good || LastResult == HitResult.Perfect)
            SoundManager.Instance.PlaySFX("Hit");

        if (ballProjection != null && LastResult != HitResult.Strike)
            ballProjection.StopProjection();

        batterSprite.SwingSprite();

        if (LastResult != HitResult.Strike)
            GameManager.Instance.ChangeState(GameState.Result);
    }

    private void OnPitchFinished()
    {
        if (inputUsed)
        {
            if (LastResult == HitResult.Strike)
            {
                SoundManager.Instance.PlaySFX("Catch");
                GameManager.Instance.ChangeState(GameState.Result);
            }
            return;
        }

        LastResult = pitch.isStrike ? HitResult.Strike : HitResult.Ball;
        SoundManager.Instance.PlaySFX("Catch");
        GameManager.Instance.ChangeState(GameState.Result);
    }
}
