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
        // 게임 시작 시 3초 대기 후 투구 시작
        yield return new WaitForSeconds(readyDelay);
        
        // 기본 투구 데이터 생성
        PitchData pitchData = new PitchData();
        pitchController.StartPitch(pitchData);
    }

    public void ChangeState(GameState state)
    {
        State = state;
    }
}
