using System;
using UnityEngine;

public class DisableOutlineOnStart : MonoBehaviour
{
    private float delay = 0.01f;
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        Invoke(nameof(DisableOutline), delay);
    }
    
    private void DisableOutline()
    {
        outline.enabled = false;
    }
}
