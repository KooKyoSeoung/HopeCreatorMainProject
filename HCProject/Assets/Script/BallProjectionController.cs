using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProjectionController : MonoBehaviour
{
    public Transform ballProjection;
    public GameObject ballImg;
    
    [SerializeField] private float minScale = 0.2f;
    [SerializeField] private float maxScale = 1.0f;

    private float startTime;
    private float totalTime;
    private bool isProjecting = false;
    private bool ballActivated = false;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    public void StartProjection(float time, Vector2 targetPos)
    {
        totalTime = time;
        startTime = Time.time;
        isProjecting = true;
        ballActivated = false;
        targetPosition = targetPos;

        if (ballProjection != null)
        {
            // 시작 위치는 투수 위치 (또는 현재 위치)
            startPosition = ballProjection.localPosition;
            ballProjection.localScale = Vector3.one * minScale;
            ballProjection.gameObject.SetActive(true);
        }

        if (ballImg != null)
        {
            ballImg.SetActive(false);
        }
    }

    void Update()
    {
        if (!isProjecting)
            return;

        float elapsed = Time.time - startTime;
        float t = Mathf.Clamp01(elapsed / totalTime);
        
        if (ballProjection != null)
        {
            float scale = Mathf.Lerp(minScale, maxScale, t);
            ballProjection.localScale = Vector3.one * scale;
            
            Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, t);
            ballProjection.localPosition = new Vector3(currentPosition.x, currentPosition.y, ballProjection.localPosition.z);
        }

        if (t >= 1.0f && !ballActivated)
        {
            ballActivated = true;
            isProjecting = false;
            
            if (ballProjection != null)
            {
                ballProjection.gameObject.SetActive(false);
            }
            
            if (ballImg != null)
            {
                ballImg.SetActive(true);
                ballImg.transform.localPosition = new Vector3(targetPosition.x, targetPosition.y, ballImg.transform.localPosition.z);
            }
        }
    }
}
