using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterSprite : MonoBehaviour
{
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject swing;

    public void IdleSprite()
    {
        idle.gameObject.SetActive(true);
        swing.gameObject.SetActive(false);
    }

    public void SwingSprite()
    {
        idle.gameObject.SetActive(false);
        swing.gameObject.SetActive(true);
    }
}
