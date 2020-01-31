using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
public class GameLogic : MonoBehaviour
{
    
    public List<PlayerObj> playersData = new List<PlayerObj>();



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
        Debug.Log($"Device with ID of {device_id} has connected!");
        PlayerObj newPlayerData = new PlayerObj();
        newPlayerData.pID = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        newPlayerData.score = 0;
        newPlayerData.numOfChips = 0;
        newPlayerData.numOfAttaches = 0;

        playersData.Add(newPlayerData);
        MakePlayerVibrate(newPlayerData.pID);
    }

    public void OnDisconnect (int device_ID)
    {
        Debug.Log($"Device with ID of {device_ID} has disconnected!");

    }

    //public void OnGameEnd (int device)

    public void OnMessage(int fromDeviceID, JToken data)
    {
        Debug.Log("message from " + fromDeviceID + ", data: " + data);
        if (data["action"] != null)
        {
            int messagingPID = AirConsole.instance.ConvertDeviceIdToPlayerNumber(fromDeviceID);
            switch (data["action"].ToString())
            {
                case "interact":
                    Camera.main.backgroundColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    break;
                case "setmodeADD":
                    SetPlayerMode(messagingPID, "add");
                    break;
                case "setmodeSUBTRACT":
                    SetPlayerMode(messagingPID, "subtract");
                    break;
                case "playerInteract":
                    InteractWithSculpture(messagingPID, data.Value<float>(data["coordX"]), data.Value<float>(data["coordY"]));
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
        PlayerObj playerToSet = playersData.FirstOrDefault<PlayerObj>(p => p.pID == playerID);
        if (playerToSet.pID != -1 && playerToSet.pID != 0)
        {
            playerToSet.interactMode = modeToSet;
        }
    }


    public void InteractWithSculpture (int playerID, float coordX, float coordY)
    {
        PlayerObj player = playersData.FirstOrDefault<PlayerObj>(p => p.pID == playerID);
        if (player.pID != -1 && player.pID != 0)
        {
            if (player.interactMode == "add")
            {

            } else if (player.interactMode == "subtract")
            {

            }

        }
    }
}
