using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int gameWidth = 10;
    public int gameHeight = 20;

    public float tileSpaceX = 1;//TODO: maybe make this  auto generated for optimal setting maybe?
    public float tileSpaceY = 1;

    public GameObject tilePrefab;

    Tile[,] tiles;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new Tile [gameWidth,gameHeight];
        //TODO:: Level editor maybe? maybe also refactor this function to diffrent file
        Tile.manager = this;//sets this as the manager for all the tiels
        FillTiles();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FillTiles(){
        for (int x = 0; x < tiles.GetLength(0); x++){
            for (int y = 0; y< tiles.GetLength(1); y++){
                tiles[x,y] = (Instantiate(tilePrefab,
                     tileGamePosVec(x,y), 
                     Quaternion.identity, transform) as GameObject)
                    .GetComponent<Tile>();

                tiles[x, y].X = x;//gives each tile its position in the grid
                tiles[x, y].Y = y;

            }
        }

    }
    /**
     * summary this method is suppsued to move all of the tiles to their correct position in the game world after
     * we rotate the local grid or something                
     */
    private void RedrawTilesFromLocal(){
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].transform.position = tileGamePosVec(x, y);//Update pos from local
            }
        }

    }

    private Vector2 tileGamePosVec(int x, int y){
        return new Vector2(transform.position.x + x * tileSpaceX, transform.position.y + y * tileSpaceY);
    }
    public void HandleTileClick(Tile tile){ // Here is where we decide what we should do wheter that a rotate or somethin like that;
        tile.gameObject.SetActive(false);
    }


}
