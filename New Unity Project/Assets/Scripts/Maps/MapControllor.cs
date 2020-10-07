using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapControllor : MonoBehaviour
{
    public static MapControllor Instance;

    public int MapHeight, MapWidth;

    [Header("Grids&Terrain")]
    [SerializeField]
    private GameObject Terrain;
    public GameObject[,] Grids;

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    public void Initialize()
    {
        // x is width, y is height
        Grids = new GameObject[MapWidth, MapHeight];
        Debug.Log("Map heigh is " + MapHeight + "Map with is " + MapWidth);
        for(int i=0; i<MapHeight; i++)
        {
            var row = Terrain.transform.Find(i.ToString()).gameObject;
            for(int j=0; j<MapWidth; j++)
            {
                var tile = row.transform.Find(j.ToString()).gameObject;
                tile.GetComponent<GridControlor>().InstantiateGrid();
                tile.GetComponent<GridControlor>().ActivateGrids(false);
            }
        }
    }

    public void SetupGrids(int X, int Y, GameObject gb)
    {
        Grids[Y, X] = gb;
    }

    public void ActivateAllGrids(bool activate)
    {
        for(int i=0; i<MapHeight; i++)
        {
            for (int j=0; j<MapWidth; j++)
            {
                Grids[i, j].GetComponent<GridControlor>().ActivateGrids(activate);
                Grids[i, j].GetComponent<GridControlor>().SetUpGridMat(WalkDescription.NotWalkable);
            }
        }
    }

    public List<Tile> ShowMovableTiles(CharacterInfo character)
    {
        ActivateAllGrids(true);

        var list = FindRangeTiles(character);
        var reachableTile = new List<Tile>();

        var minX = list.OrderBy(x => x.X).First().X;
        var maxX = list.OrderBy(x => x.X).Last().X;
        var minY = list.OrderBy(x => x.Y).First().Y;
        var maxY = list.OrderBy(x => x.Y).Last().Y;
        var bounds = new Boundary(minX, maxX, minY, maxY);

        foreach(var tile in list)
        {
            //Grids[tile.Y, tile.X].GetComponent<GridControlor>().SetUpGridMat(WalkDescription.Walkable);
            if (isReachable(character, new Vector2(tile.X, tile.Y), bounds))
                reachableTile.Add(tile);
        }

        foreach(var tile in reachableTile)
            Grids[tile.Y, tile.X].GetComponent<GridControlor>().SetUpGridMat(WalkDescription.Walkable);

        return reachableTile;
    }

    /// <summary>
    /// search for the tiles that is in character's range
    /// </summary>
    public List<Tile> FindRangeTiles(CharacterInfo character)
    {
        var tileInRange = new List<Tile>();

        var curTileX = (int)character.CurrentPositionID.x;
        var curTileY = (int)character.CurrentPositionID.y;

        for(int i=0; i<MapHeight; i++)
        {
            for (int j=0; j<MapWidth; j++)
            {
                var sourcePos = new Vector2(curTileX, curTileY);
                var distance = Vector2.Distance(sourcePos, new Vector2(j, i));

                if (distance <= character.MAX_MOVABLE_DIS)
                {
                    var tile = new Tile();
                    tile.X = j;
                    tile.Y = i;
                    tileInRange.Add(tile);
                }
            }
        }
        return tileInRange;
    }

    public bool isReachable(CharacterInfo character, Vector2 Destination, Boundary bounds)
    {

        int tilex = (int)character.CurrentPositionID.x;
        int tiley = (int)character.CurrentPositionID.y;

        var start = new Tile();
        start.X = tilex;
        start.Y = tiley;

        int desTileX = (int)Destination.x;
        int desTileY = (int)Destination.y;
        var dest = new Tile();
        dest.X = desTileX;
        dest.Y = desTileY;

        start.SetDistance(dest.X, dest.Y);

        var activeTiles = new List<Tile>();
        var visitedTiles = new List<Tile>();
        activeTiles.Add(start);


        while(activeTiles.Any())
        {
            var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

            if (checkTile.X == dest.X && checkTile.Y == dest.Y)
            {
                return true;
            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);

            var walkableTiles = Tile.GetWalkableTiles(this.Grids, checkTile, dest, character, bounds);

            foreach(var walkableTile in walkableTiles)
            {
                // check if the tile has been visited
                if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    continue;

                // it is in active list but has a better distance cost 
                if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                {
                    var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                    if (existingTile.CostDistance > checkTile.CostDistance)
                    {
                        activeTiles.Remove(existingTile);
                        activeTiles.Add(walkableTile);
                    }
                }
                // never seen this tile before
                else
                {
                    activeTiles.Add(walkableTile);
                }
            }
        }

        return false;
    }
}
