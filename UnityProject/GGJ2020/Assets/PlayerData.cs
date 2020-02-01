using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerObj
{
    public int? pID;
    public int score;
    public int numOfChips;
    public int numOfAttaches;
    public float clayPercentage;
    public string interactMode;
    public string playerName = "";
    public int votesReceived = 0; // must be at least 0, shouldn't (hopefully) be higher than 24
    public int votesGiven = 0; // should never be higher than 3;
}


