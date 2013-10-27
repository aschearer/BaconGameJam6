using UnityEngine;
using System.Collections;
using BaconGameJam6.Models.Player;

public class UIManagerScript : MonoBehaviour
{
    public GameObject Player1StartPlane;
    public GameObject Player2StartPlane;
    public GameObject Player3StartPlane;
    public GameObject Player4StartPlane;
    public GameObject InGamePlane;
    public MonoBehaviour GameLoop;

    private GameObject player1StartPlane;
    private GameObject player2StartPlane;
    private GameObject player3StartPlane;
    private GameObject player4StartPlane;
    private GameObject player1ActivePlane;
    private GameObject player2ActivePlane;
    private GameObject player3ActivePlane;
    private GameObject player4ActivePlane;

    // Use this for initialization
    void Start()
    {
        const float uiX = -16.5f;
        const float uiyStart = 8.8f;
        const float uiyStep = 1.6f;
        const float uiZ = 0;
        this.player1StartPlane = Instantiate(this.Player1StartPlane, new Vector3(uiX, uiyStart, uiZ), this.Player1StartPlane.transform.rotation) as GameObject;
        this.player2StartPlane = Instantiate(this.Player2StartPlane, new Vector3(uiX, uiyStart + uiyStep, uiZ), this.Player2StartPlane.transform.rotation) as GameObject;
        this.player3StartPlane = Instantiate(this.Player3StartPlane, new Vector3(uiX, uiyStart + 2f * uiyStep, uiZ), this.Player3StartPlane.transform.rotation) as GameObject;
        this.player4StartPlane = Instantiate(this.Player4StartPlane, new Vector3(uiX, uiyStart + 3f * uiyStep, uiZ), this.Player4StartPlane.transform.rotation) as GameObject;

        this.player1ActivePlane = Instantiate(this.InGamePlane, new Vector3(uiX, uiyStart, uiZ), this.Player1StartPlane.transform.rotation) as GameObject;
        this.player2ActivePlane = Instantiate(this.InGamePlane, new Vector3(uiX, uiyStart + uiyStep, uiZ), this.Player2StartPlane.transform.rotation) as GameObject;
        this.player3ActivePlane = Instantiate(this.InGamePlane, new Vector3(uiX, uiyStart + 2f * uiyStep, uiZ), this.Player3StartPlane.transform.rotation) as GameObject;
        this.player4ActivePlane = Instantiate(this.InGamePlane, new Vector3(uiX, uiyStart + 3f * uiyStep, uiZ), this.Player4StartPlane.transform.rotation) as GameObject;
        this.player1ActivePlane.SetActive(false);
        this.player2ActivePlane.SetActive(false);
        this.player3ActivePlane.SetActive(false);
        this.player4ActivePlane.SetActive(false);

        (this.GameLoop as GameLoop).PlayerStateChanged += this.PlayerStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerStateChanged(object sender, PlayerStateEventArgs args)
    {
        if (args.PlayerStateEvent == PlayerStateEvent.Joined)
        {
            this.SetPlayerActive(args.PlayerId);
        }
        else if (args.PlayerStateEvent == PlayerStateEvent.Left)
        {
            this.SetPlayerLeave(args.PlayerId);
        }
    }

    public void SetPlayerActive(PlayerId playerID)
    {
        switch (playerID)
        {
            case PlayerId.One:
                this.player1StartPlane.SetActive(false);
                this.player1ActivePlane.SetActive(true);
                break;
            case PlayerId.Two:
                this.player2StartPlane.SetActive(false);
                this.player2ActivePlane.SetActive(true);
                break;
            case PlayerId.Three:
                this.player3StartPlane.SetActive(false);
                this.player3ActivePlane.SetActive(true);
                break;
            case PlayerId.Four:
                this.player4StartPlane.SetActive(false);
                this.player4ActivePlane.SetActive(true);
                break;
        }
    }

    public void SetPlayerLeave(PlayerId playerID)
    {
        switch (playerID)
        {
            case PlayerId.One:
                this.player1StartPlane.SetActive(true);
                this.player1ActivePlane.SetActive(false);
                break;
            case PlayerId.Two:
                this.player2StartPlane.SetActive(true);
                this.player2ActivePlane.SetActive(false);
                break;
            case PlayerId.Three:
                this.player3StartPlane.SetActive(true);
                this.player3ActivePlane.SetActive(false);
                break;
            case PlayerId.Four:
                this.player4StartPlane.SetActive(true);
                this.player4ActivePlane.SetActive(false);
                break;
        }
    }
}
