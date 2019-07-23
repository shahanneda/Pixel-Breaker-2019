using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileManager;

public class TileGravity : MonoBehaviour
{
    public static TileManager manager;

    public void RunCheck()
    {
        print("Running whole board gravity check");
        for (int j = 0; j < tiles.GetLength(1); j++)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                if (tiles[i, j].isDead)
                {
                    Tile above = findHighestTileThatIsNotDead(i, j);
                    if (above != null)
                    {
                        manager.SwitchTiles(tiles[i, j], above);
                    }

                }
            }

        }

        manager.RedrawTilesFromLocal();
    }

    private Tile findHighestTileThatIsNotDead(int x, int y){
        
        Tile currentTile = tiles[x, y];
        for (int i = 0; i + y < tiles.GetLength(1); i++ ){//The i is how many tiles above
            currentTile = tiles[x, y + i];
            if(!currentTile.isDead){
                return currentTile;
            }
        }
        return null;

    }
    public void RunCheckDelayed(float time){
        Invoke("RunCheck", time);
    }
}
