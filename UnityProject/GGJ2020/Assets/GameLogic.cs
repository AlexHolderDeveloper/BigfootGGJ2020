using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
public class GameLogic : MonoBehaviour
{
    
    public List<PlayerObj> playersData = new List<PlayerObj>();
    public Text onscreenLog;
    public Transform[] perPlayerCameras;


    private void Awake()
    {
        // register events
        //AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        //AirConsole.instance.onDeviceStateChange += OnDeviceStateChange;
        //AirConsole.instance.onCustomDeviceStateChange += OnCustomDeviceStateChange;
        //AirConsole.instance.onDeviceProfileChange += OnDeviceProfileChange;
        //AirConsole.instance.onAdShow += OnAdShow;
        //AirConsole.instance.onAdComplete += OnAdComplete;
        //AirConsole.instance.onGameEnd += OnGameEnd;
        //AirConsole.instance.onHighScores += OnHighScores;
        //AirConsole.instance.onHighScoreStored += OnHighScoreStored;
        //AirConsole.instance.onPersistentDataStored += OnPersistentDataStored;
        //AirConsole.instance.onPersistentDataLoaded += OnPersistentDataLoaded;
        //AirConsole.instance.onPremium += OnPremium;
        //logWindow.text = "Connecting... \n \n";

        //InvokeRepeating("MakeAllPlayersVibrate", 3, 3);
    }


    public void StartGameRound ()
    {
        AirConsole.instance.SetActivePlayers();

    }

    public void OnConnect (int device_id)
    {
        AirConsole.instance.SetActivePlayers();
        Debug.Log($"Device with ID of {device_id} has connected!");

        //PlayerObj playerToSet = playersData.FirstOrDefault<PlayerObj>(p => p.pID == playerID);



        PlayerObj newPlayerData = new PlayerObj
        {
            pID = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id),
            score = 0,
            numOfChips = 0,
            numOfAttaches = 0
        };

        PlayerObj playerCopyCheck = playersData.FirstOrDefault(p => p.pID == newPlayerData.pID);
        if (playerCopyCheck == null)
        {
            onscreenLog.text = JsonUtility.ToJson(newPlayerData);
            playersData.Add(newPlayerData);
            MakePlayerVibrate((int)newPlayerData.pID);
        } else
        {
            onscreenLog.text = $"Welcome back Player {newPlayerData.pID}!";
        }


    }

    public void OnDisconnect (int device_ID)
    {
        Debug.Log($"Device with ID of {device_ID} has disconnected!");

    }

    //public void OnGameEnd (int device)

    public void OnMessage(int fromDeviceID, JToken data)
    {
        int messagingPID = AirConsole.instance.ConvertDeviceIdToPlayerNumber(fromDeviceID);

        //Debug.Log("message from " + fromDeviceID + ", data: " + data);
        Debug.Log("message from " + messagingPID + ", data: " + data);
        if (data["action"] != null)
        {
            switch (data["action"].ToString())
            {
                case "interact":
                    Camera.main.backgroundColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                    break;
                case "setmodeADD":
                    SetPlayerMode(messagingPID, "add");
                    break;
                case "setmodeSUBTRACT":
                    SetPlayerMode(messagingPID, "subtract");
                    break;
                case "playerInteract":
                    InteractWithSculpture(messagingPID, float.Parse(data["coordX"].ToString()), float.Parse(data["coordY"].ToString()));
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }


    public void SetActivePlayers()
    {
        //Set the currently connected devices as the active players (assigning them a player number)
        AirConsole.instance.SetActivePlayers();

        string activePlayerIds = "";
        foreach (int id in AirConsole.instance.GetActivePlayerDeviceIds)
        {
            activePlayerIds += id + "\n";
        }

        //Log to on-screen Console
        //logWindow.text = logWindow.text.Insert(0, "Active Players were set to:\n" + activePlayerIds + " \n \n");
    }

    public void MakeAllPlayersVibrate ()
    {
        var customData = new
        {
            vibrate = "250",
        };
        AirConsole.instance.Broadcast(customData);
        Debug.Log("Told all players to buzz!");
    }

    public void MakePlayerVibrate(int playerID)
    {
        var customData = new
        {
            vibrate = "250",
        };

        AirConsole.instance.Message(AirConsole.instance.ConvertPlayerNumberToDeviceId(playerID), customData);
        Debug.Log($"Told the device with player ID of {playerID} to vibrate!");
    }

    public void SetPlayerMode(int playerID, string modeToSet)
    {
        PlayerObj playerToSet = playersData.FirstOrDefault(p => p.pID == playerID);
        if (playerToSet != null)
        {
            playerToSet.interactMode = modeToSet;
            Debug.Log("Player mode updated! It is now: " + modeToSet);
        }
    }


    public void InteractWithSculpture (int playerID, float coordX, float coordY)
    {
        onscreenLog.text = $"Player with id of {playerID} touched the screen at x: {coordX} y: {coordY} ";
        PlayerObj player = playersData.FirstOrDefault(p => p.pID == playerID);
        if (player != null)
        {
            if (player.interactMode == "add")
            {
                Debug.Log("Adding a new chunk of clay now!");
                GameObject newClay = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newClay.transform.parent = perPlayerCameras[playerID].transform;
                newClay.transform.position = new Vector3(0, 0, 0);
                //newClay.transform.position = perPlayerCameras[playerID].transform.position + (perPlayerCameras[playerID].transform.forward * 10);
                newClay.transform.localPosition = new Vector3(
                    newClay.transform.localPosition.x + (coordX / 100),
                    newClay.transform.localPosition.y + (coordY / 100),
                    10
                );
                newClay.transform.parent = transform.parent;


                //newClay.transform.localPosition = new Vector3(
                //    newClay.transform.localPosition.x + (coordX / 100),
                //    newClay.transform.localPosition.y + (coordY / 100),
                //    newClay.transform.localPosition.z
                //    );
            } else if (player.interactMode == "subtract")
            {

            }

        }
    }
}
