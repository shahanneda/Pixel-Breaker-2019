using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileManager;
/// <summary>
/// Tile actions:
/// Is Responsible for actions that happen to tiles for example rotations and switches and destruction
/// </summary>
public class TileActions : MonoBehaviour
{
    public static TileManager manager;

    public void Rotate3x3Tiles(int x, int y)
    {
        if (manager.TileIsOnEdge(tiles[x, y]))
            return;
        print("Test");
        Tile bottomLeft = tiles[x - 1, y - 1];
        Tile bottomMiddle = tiles[x, y - 1];
        Tile bottomRight = tiles[x + 1, y - 1];
        Tile topLeft = tiles[x + 1, y + 1];

        tiles[x + 1, y - 1] = tiles[x + 1, y + 1]; // bottom right = top right
        tiles[x, y - 1] = tiles[x + 1, y];
        tiles[x - 1, y - 1] = bottomRight;

        tiles[x + 1, y + 1] = tiles[x - 1, y + 1];
        tiles[x + 1, y] = tiles[x, y + 1];
        tiles[x + 1, y - 1] = topLeft;

        tiles[x + 1, y + 1] = tiles[x - 1, y + 1];
        tiles[x, y + 1] = tiles[x - 1, y];
        tiles[x - 1, y + 1] = bottomLeft;

        tiles[x - 1, y + 1] = bottomLeft;
        tiles[x - 1, y] = bottomMiddle;
        tiles[x - 1, y - 1] = bottomRight;

        //manager.gravityQueue.AddRange(new List<Tile> { tiles[x,y],tiles[x + 1, y + 1], tiles[x, y + 1], tiles[x - 1, y + 1], tiles[x - 1, y], tiles[x - 1, y - 1], tiles[x, y - 1],
        //tiles[x + 1, y - 1],tiles[x + 1, y]
        //});
        //Invoke("GravityInvoke", manager.gravityCheckFloat);
        manager.tileGravity.RunCheckDelayed(manager.gravityCheckFloat);
        manager.RedrawTilesFromLocal();
    }

    //@ADAM, create the other ones of these from the design doc, draw it out on paper to help you lundersd which one is which, and be ready to be frustrated 
    public void Rotate3x3Left90Degrees(int x, int y)
    {
        if (manager.TileIsOnEdge(tiles[x, y]))
            return;
        Tile tempTile = tiles[x + 1, y + 1];
        tiles[x + 1, y + 1] = tiles[x + 1, y];
        tiles[x + 1, y] = tiles[x + 1, y - 1];
        tiles[x + 1, y - 1] = tiles[x, y - 1];
        tiles[x, y - 1] = tiles[x - 1, y - 1];
        tiles[x - 1, y - 1] = tiles[x - 1, y];
        tiles[x - 1, y] = tiles[x - 1, y + 1];
        tiles[x - 1, y + 1] = tiles[x, y + 1];
        tiles[x, y + 1] = tempTile;

        //manager.gravityQueue.AddRange(new List<Tile> { tiles[x,y],tiles[x + 1, y + 1], tiles[x, y + 1], tiles[x - 1, y + 1], tiles[x - 1, y], tiles[x - 1, y - 1], tiles[x, y - 1],
        //tiles[x + 1, y - 1],tiles[x + 1, y]
        //});
        manager.tileGravity.RunCheckDelayed(manager.gravityCheckFloat);

        //Invoke("GravityInvoke", manager.gravityCheckFloat);
        manager.RedrawTilesFromLocal();
    }

    public void Rotate3x3Right90Degrees(int x, int y)
    {
        if (manager.TileIsOnEdge(tiles[x, y]))
            return;
        Tile tempTile = tiles[x + 1, y + 1];
        tiles[x + 1, y + 1] = tiles[x, y + 1];
        tiles[x, y + 1] = tiles[x - 1, y + 1];
        tiles[x - 1, y + 1] = tiles[x - 1, y];
        tiles[x - 1, y] = tiles[x - 1, y - 1];
        tiles[x - 1, y - 1] = tiles[x, y - 1];
        tiles[x, y - 1] = tiles[x + 1, y - 1];
        tiles[x + 1, y - 1] = tiles[x + 1, y];
        tiles[x + 1, y] = tempTile;

        //manager.gravityQueue.AddRange(new List<Tile> { tiles[x,y],tiles[x + 1, y + 1], tiles[x, y + 1], tiles[x - 1, y + 1], tiles[x - 1, y], tiles[x - 1, y - 1], tiles[x, y - 1],
        //tiles[x + 1, y - 1],tiles[x + 1, y]
        //});
        //Invoke("GravityInvoke", manager.gravityCheckFloat);
        manager.tileGravity.RunCheckDelayed(manager.gravityCheckFloat);

        manager.RedrawTilesFromLocal();
    }
    //public void GravityInvoke()
    //{
    //    manager.GravityInvoke();
    //}
}
