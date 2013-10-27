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
    private bool keyDown;

	// Use this for initialization
	void Start () {
        this.GameLoop.GameLoopStateChanged += this.OnGameLoopStateChanged;
        UpdateVisible(this.GameLoop.GameLoopState);
	}
    	
	// Update is called once per frame
	void Update () {
        if (this.renderer.enabled)
        {
            // key text
            KeyCode keyCode;
            switch (this.Action)
            {
            case ButtonAction.Back:
                keyCode = KeyCode.Escape;
                break;
            case ButtonAction.Start1:
                keyCode = KeyCode.Alpha1;
                break;
            case ButtonAction.Start2:
                keyCode = KeyCode.Alpha2;
                break;
            case ButtonAction.Start3:
                keyCode = KeyCode.Alpha3;
                break;
            case ButtonAction.Start4:
                keyCode = KeyCode.Alpha4;
                break;
            case ButtonAction.Help:
                keyCode = KeyCode.Slash;
                break;
            default:
                return;
            }
            
            if (keyDown && Input.GetKeyUp(keyCode))
            {
                DoAction();
                keyDown = false;
            }
            else if (!keyDown && Input.GetKeyDown(keyCode))
            {
                keyDown = true;
            }
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
            DoAction();
        }
    }
    
    void DoAction()
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
