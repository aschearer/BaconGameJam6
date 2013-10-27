using UnityEngine;
using System.Collections;

public enum ButtonAction
{
    Help,
    Start1, // NOTE: Start1-4 have to have the same index as their number
    Start2,
    Start3,
    Start4, // NOTE: Start1-4 have to have the same index as their number
    Back,
}

public class TextButton : MonoBehaviour {
    
    public ButtonAction Action;
    public GameLoop GameLoop;

	// Use this for initialization
	void Start () {
        this.GameLoop.GameLoopStateChanged += this.OnGameLoopStateChanged;
        UpdateVisible(this.GameLoop.GameLoopState);
	}
    	
	// Update is called once per frame
	void Update () {
        if (this.renderer.enabled)
        {
            // $TODO: key text
        }
    }
    
    void OnMouseOver()
    {
        // $TODO:
    }
    
    void OnGameLoopStateChanged(object sender, GameLoopStateEventArgs args)
    {
        UpdateVisible(args.GameLoopState);
    }
    
    void UpdateVisible(GameLoopState gameLoopState)
    {
        bool gamePlaying = (gameLoopState == GameLoopState.Playing);
        bool hideIfGamePlaying = (this.Action != ButtonAction.Back);
        
        bool enabledState = (gamePlaying != hideIfGamePlaying);
        if (enabledState != this.renderer.enabled)
        {
            this.renderer.enabled = enabledState;
        }
    }
    
    void OnMouseUpAsButton() {
        if (this.renderer.enabled)
        {
            switch (this.Action)
            {
            case ButtonAction.Start1:
            case ButtonAction.Start2:
            case ButtonAction.Start3:
            case ButtonAction.Start4:
                this.GameLoop.StartNewGame((int)this.Action);
                break;
            case ButtonAction.Help:
                // $TODO: show help
                break;
            case ButtonAction.Back:
                this.GameLoop.EndGame();
                break;
            }
        }
	}
}
