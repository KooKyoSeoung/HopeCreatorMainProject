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
    private Vector2 controlPoint1;
    private Vector2 controlPoint2;

    public void StartProjection(float time, Vector2 startPos, Vector2 targetPos, Vector2 cp1, Vector2 cp2)
    {
        totalTime = time;
        startTime = Time.time;
        isProjecting = true;
        ballActivated = false;
        startPosition = startPos;
        targetPosition = targetPos;
        controlPoint1 = cp1;
        controlPoint2 = cp2;

        if (ballProjection != null)
        {
            ballProjection.localPosition = new Vector3(startPosition.x, startPosition.y, ballProjection.localPosition.z);
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
            
            Vector2 currentPosition = CubicBezier(startPosition, controlPoint1, controlPoint2, targetPosition, t);
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

    public void Reset()
    {
        isProjecting = false;
        ballActivated = false;

        if (ballProjection != null)
            ballProjection.gameObject.SetActive(false);

        if (ballImg != null)
            ballImg.SetActive(false);
    }

    private Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float u = 1f - t;
        return (u * u * u * p0) + (3f * u * u * t * p1) + (3f * u * t * t * p2) + (t * t * t * p3);
    }
}
