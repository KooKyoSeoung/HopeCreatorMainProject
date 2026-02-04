using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherSprite : MonoBehaviour
{
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject pitch;

    public void IdleSprite()
    {
        idle.gameObject.SetActive(true);
        pitch.gameObject.SetActive(false);
    }

    public void PitchSprite()
    {
        idle.gameObject.SetActive(false);
        pitch.gameObject.SetActive(true);
    }
}
