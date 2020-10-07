using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;
    public Team team;
    public CharacterID characterID;
}

public class PlayerControllor : MonoBehaviour
{
    public Player player = new Player();
    private bool accessable = false; 

    public void SetupPlayerInfo(Player player)
    {
        this.player = player;
    }

    public void AssignControl(bool active)
    {
        accessable = true;
    }

    private void Update()
    {
        if (!accessable)
            return;

    }
}

