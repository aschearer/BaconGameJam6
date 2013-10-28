using System;
using System.Linq;

using UnityEngine;
using BaconGameJam6.Models.Simulations;

public class BlinkingLight
{
    private Simulation simulation;
    private Color colorToBlink;
    private Color colorToEnd;
    private float elapsedTime = 0f;
    private int offset;
    private bool isMatch;
    private static readonly float FailBlinkTime = 0.1f;
    private static readonly float MatchBlinkTime = 0.16f;
    private static readonly float TotalFailBlinkTime = 0.4f;
    private static readonly float TotalMatchBlinkTime = 1.12f;
    private static readonly float TotalMatchOnTime = 0.96f;
    
    public BlinkingLight(Simulation simulation, Transform transform, Color colorToBlink, Color colorToEnd, int offset, bool isMatch)
    {
        this.simulation = simulation;
        this.Transform = transform;
        this.colorToBlink = colorToBlink;
        this.colorToEnd = colorToEnd;
        this.offset = offset;
        this.isMatch = isMatch;
    }
    
    public bool IsCompleted { get; private set; }
    public Transform Transform { get; private set; }
    
    public void Update(float elapsedTime)
    {
        this.elapsedTime += elapsedTime;
        
        if (!simulation.HasPlayer)
        {
            this.IsCompleted = true;
            return;
        }
        
        if (this.elapsedTime >= (this.isMatch ? TotalMatchBlinkTime : TotalFailBlinkTime))
        {
            this.IsCompleted = true;
            this.Transform.renderer.material.color = this.colorToEnd;
        }
        else
        {
            bool frameTime = false;
            if (this.isMatch)
            {
                int newFrame = (int)Math.Floor(this.elapsedTime / MatchBlinkTime) % 3;
                frameTime = (offset == newFrame) || (this.elapsedTime >= TotalMatchOnTime);
            }
            else
            {
                int newFrame = (int)Math.Floor(this.elapsedTime / FailBlinkTime) % 2;
                frameTime = (0 == newFrame);
            }
            Color newColor = this.colorToBlink;
            if (!frameTime)
            {
                newColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
            
            if (this.Transform.renderer.material.color != newColor)
            {
                this.Transform.renderer.material.color = newColor;
            }
        }
    }
}

