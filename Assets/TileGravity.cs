using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileManager;

public class TileGravity : MonoBehaviour
{
    public static TileManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runCheck(){
        print("Runnign whole board gravity check");
        for (int j = 0; j < tiles.GetLength(1); j++){
            print("Starting on row " + j);
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                if (tiles[i, j].isDead)
                {
                    Tile above = findHighestTileThatIsNotDead(i, j);
                    if (above != null)
                    {
                        //above.MarkForDebug();

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
        print("Strating ru");
        Invoke("runCheck", time);
    }
}
