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
    private bool buttonEnabled;

	// Use this for initialization
	void Start () {
        this.GameLoop.GameLoopStateChanged += this.OnGameLoopStateChanged;
        UpdateVisible(this.GameLoop.GameLoopState);
        this.isSelected = (this.Action == ButtonAction.Start2);
        UpdateColor();
	}
    	
	// Update is called once per frame
	void Update () {
        if (this.buttonEnabled)
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
    
    private bool mouseOver;
    private bool mouseDown;
    void OnMouseEnter()
    {
        this.mouseOver = true;
        UpdateColor();
    }
    
    void OnMouseDown()
    {
        this.mouseDown = true;
        UpdateColor();
    }
    
    void OnMouseUp()
    {
        this.mouseDown = false;
        UpdateColor();
    }
    
    void OnMouseExit()
    {
        this.mouseOver = false;
        this.mouseDown = false;
        UpdateColor();
    }
    
    void UpdateColor()
    {
        Color newColor = Color.white;

        if (this.isSelected)
        {
            newColor = Color.cyan;
        }

        if (this.mouseDown)
        {
            newColor = Color.blue;
        }
        else if (this.mouseOver)
        {
            newColor = Color.green;
        }

        if (this.transform.renderer.material.color != newColor)
        {
            this.transform.renderer.material.color = newColor;
        }
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
        if (enabledState != this.buttonEnabled)
        {
            this.buttonEnabled = enabledState;
        }
    }
    
    void OnMouseUpAsButton() {
        if (this.renderer.enabled)
        {
            DoAction();
        }
    }
    
    private bool isSelected;
    public bool IsSelected
    {
        get
        {
            return this.isSelected;
        }
        
        set
        {
            this.isSelected = value;
            UpdateColor();
        }
    }
    
    public void DoAction()
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
