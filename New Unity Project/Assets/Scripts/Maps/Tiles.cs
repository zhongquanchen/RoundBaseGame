using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Boundary
{
	public int minX;
	public int maxX;
	public int minY;
	public int maxY;

	public Boundary(int minx, int maxx, int miny, int maxy)
    {
		minX = minx;
		minY = miny;
		maxX = maxx;
		maxY = maxy;
    }
}

public class Tile
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Cost { get; set; }
	public int Distance { get; set; }
	public int CostDistance => Cost + Distance;
	public Tile Parent { get; set; }

	//The distance is essentially the estimated distance, ignoring walls to our target. 
	//So how many tiles left and right, up and down, ignoring walls, to get there. 
	public void SetDistance(int targetX, int targetY)
	{
		this.Distance = Mathf.Abs(targetX - X) + Mathf.Abs(targetY - Y);
	}

	public static List<Tile> GetWalkableTiles(GameObject[,] map, Tile currentTile, Tile targetTile,
		CharacterInfo character, Boundary bounds)
	{
		var possibleTiles = new List<Tile>()
		{
			new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
			new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
			new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
			new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
		};

		possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

		// calculate current tiles height
		var curHeight = map[currentTile.Y, currentTile.X].transform.position.y;
		var clambHeight = character.MAX_CLAMPHEIGHT;

		return possibleTiles
				.Where(tile => tile.X >= bounds.minX && tile.X <= bounds.maxX)
				.Where(tile => tile.Y >= bounds.minY && tile.Y <= bounds.maxY)
				.Where(tile => map[tile.Y, tile.X].tag == "Tile")
				.Where(tile => Mathf.Abs(map[tile.Y, tile.X].transform.position.y - curHeight) <= clambHeight)
				.ToList();
	}
}
