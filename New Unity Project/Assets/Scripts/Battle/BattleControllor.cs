using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, END, NONE }
public enum Team { TEAM1, TEAM2, TEAM3, TEAM4, TEAM5, TEAM6 }

public class BattleControllor : MonoBehaviour
{
    public static BattleControllor Instance;
    // this is battle panel that to show/update battle info
    public GameObject BattlePanel;
    // this is the template that instantiate to create player object
    public GameObject PlayerTemplate;

    // this is the counter to take controll from player
    private float Counter = Utilities.ROUND_COUNTER;

    // battle involve players will add to this list
    private List<GameObject> GamePlayers = new List<GameObject>();
    private int[] TeamsHealth = { 0, 0, 0, 0, 0, 0 };

    // update will check game state and check if ended
    private BattleState State = BattleState.NONE;

    // determine which teams turn
    private Queue<Team> TeamsTurn = new Queue<Team>();
    private Team CurrentTeam = Team.TEAM1;
    private int NextFirst = 0;
    private int TeamCount = 0;
    private int Round = 0; // keep count of  # round

    /// <summary>
    /// this method will take a list of player info from the loading scene,
    /// and generate player for battle
    /// </summary>
    /// <param name="Players"></param>
    public void SetupBattle()
    {   // instantiate from player template and set up player info
        List<Player> Players = new List<Player>();




        State = BattleState.START;

        foreach(var item in Players)
        {
            var player = Instantiate(PlayerTemplate, transform);
            player.transform.SetParent(transform);
            player.GetComponent<PlayerControllor>().SetupPlayerInfo(item);
            GamePlayers.Add(player);

            // calculate total teams health for game state use
            TeamsHealth[(int)item.team] += 100;
        }

    }

    private void UpdateCounter() =>
        BattlePanel
            .GetComponent<BattlePanelControllor>()
            .SetupBattleText(((int)Counter).ToString());

    private void FixedUpdate()
    {
        if (State == BattleState.NONE)
            return;

        Counter -= Time.deltaTime;

        if (Counter <= 0)
            SwitchTurn();
    }

    /// <summary>
    /// this method will determine which team is up for next round
    /// and give access to players of the team
    /// </summary>
    private void SwitchTurn()
    {
        if (TeamsTurn.Count == 0)
        { 
            EnqueueTeams();
        }

        Round += 1;
        var team = TeamsTurn.Dequeue();

        // take control from last team, assign new control to current team
        AssignControl(CurrentTeam, false);
        CurrentTeam = team;
        AssignControl(CurrentTeam, true);
    }

    private void EnqueueTeams()
    {
        for(int i=0; i<Utilities.MAX_TEAM_NUM; i++)
        {
            var enqTeam = NextFirst % Utilities.MAX_TEAM_NUM;
            NextFirst += 1; 
            if (TeamsHealth[enqTeam] != 0)
            { // enqueue this team if it is not dead
                TeamsTurn.Enqueue((Team)enqTeam);
            }
        }
    }


    /// <summary>
    /// this method will take or give control to players
    /// </summary>
    private void AssignControl(Team team, bool active)
    {
        foreach(var players in GamePlayers)
        {
            if (players
                .GetComponent<PlayerControllor>()
                .player.team == team)
                players.GetComponent<PlayerControllor>().AssignControl(active);
        }
    }
}