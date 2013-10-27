using System;
using System.Linq;

using UnityEngine;

public class BlinkingLight
{
    private Color colorToBlink;
    private Color colorToEnd;
    private float elapsedTime = 0f;
    private static readonly float FrameBlinkTime = 0.1f;
    private static readonly float TotalBlinkTime = 0.5f;
    
    public BlinkingLight(Transform transform, Color colorToBlink, Color colorToEnd)
    {
        this.Transform = transform;
        this.colorToBlink = colorToBlink;
        this.colorToEnd = colorToEnd;
    }
    
    public bool IsCompleted { get; private set; }
    public Transform Transform { get; private set; }
    
    public void Update(float elapsedTime)
    {
        float lastElapsedTime = this.elapsedTime;
        this.elapsedTime += elapsedTime;
        
        if (this.elapsedTime >= TotalBlinkTime)
        {
            this.IsCompleted = true;
            this.Transform.renderer.material.color = this.colorToEnd;
        }
        else if (Math.Floor(this.elapsedTime / FrameBlinkTime) != Math.Floor(lastElapsedTime / FrameBlinkTime))
        {
            if (this.Transform.renderer.material.color == this.colorToBlink)
            {
                this.Transform.renderer.material.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
            else
            {
                this.Transform.renderer.material.color = this.colorToBlink;
            }
        }
    }
}

