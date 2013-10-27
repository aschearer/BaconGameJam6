using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ListControl : MonoBehaviour {
 
    public List<TextButton> ListItems;
    
	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
    private bool[] fireDown = new bool[4];
    public float timeBeforeUpdate = 0.2f;
    private static readonly float UpdateDelay = 0.2f;
	void Update () {
        int iSelected = GetSelectedIndex();
        var selected = this.ListItems[iSelected];
        if (selected.renderer.enabled)
        {
            var firePressed = false;
            var iNewSelected = iSelected;
            
            this.timeBeforeUpdate -= Time.deltaTime;
            bool updateSelection = false;
            if (this.timeBeforeUpdate < 0)
            {
                updateSelection = true;
                timeBeforeUpdate = UpdateDelay;
            }
            
            for (var i = 0; i < this.fireDown.Length; ++i)
            {
                if (updateSelection)
                {
                    var horizontal = Input.GetAxis("Horizontal" + (i + 1));
                    if (Math.Abs(horizontal) < 0.000005)
                    {
                        // Ignore
                    }
                    else if (horizontal < 0)
                    {
                        iNewSelected = (iNewSelected + this.ListItems.Count - 1) % this.ListItems.Count;
                    }
                    else if (horizontal > 0)
                    {
                        iNewSelected = (iNewSelected + 1) % this.ListItems.Count;
                    }
                }
                
                bool firePlayerPressed = (Input.GetAxis("Fire" + (i + 1)) > 0);
                if (firePlayerPressed && !this.fireDown[i])
                {
                    this.fireDown[i] = true;
                }
                else if (!firePlayerPressed && this.fireDown[i])
                {
                    this.fireDown[i] = false;
                    firePressed = true;
                }
            }
            
            if (iSelected != iNewSelected)
            {
                selected.IsSelected = false;
                selected = this.ListItems[iNewSelected];
                selected.IsSelected = true;
                
                ClearFireDown();
            }
            else if (firePressed)
            {
                ClearFireDown();
                selected.DoAction();
            }
        }
	}
    
    void ClearFireDown()
    {
        for (int i = 0; i < fireDown.Length; ++i)
        {
            fireDown[i] = false;
        }
    }
    
    int GetSelectedIndex() {
        for (var i = 0; i < this.ListItems.Count; ++i)
        {
            if (this.ListItems[i].IsSelected)
            {
                return i;
            }
        }

        // Nothing selected?
        this.ListItems[0].IsSelected = true;
        return 0;
    }
}
