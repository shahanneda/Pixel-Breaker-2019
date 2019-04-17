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

    public Color[] colors;

    enum Options {RotateClockWise, RotateCounterClockwise, Destroy,DestroyWithColors};

    Options optionSelected;

    Tile[,] tiles;
    /*
     * @ADAM, whenever we want to move the tiles, if we just move the items of this array, and call RedrawTilesFromLocal(), everything should be handeld
     * 
     */
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
     * summary: this method is suppsued to move all of the tiles to their correct position in the game world after
     * we rotate the local grid or something  
     * CALL THIS WHENEVER YOU CHANGE THE BOARD

     */
    private void RedrawTilesFromLocal(){
        //Note to self If proframce becomes issue think about not doing this every time;
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].shouldMoveTo = tileGamePosVec(x, y);//Update pos from local

                tiles[x, y].inAnimation = true;

                tiles[x, y].X = x;
                tiles[x, y].Y = y;
            }
        }

    }

    private Vector2 tileGamePosVec(int x, int y){
        return new Vector2(transform.position.x + x * tileSpaceX, transform.position.y + y * tileSpaceY);
    }
    public void HandleTileClick(Tile tile){ // Here is where we decide what we should do wheter that a rotate or somethin like that;
        //tile.gameObject.SetActive(false);
        switch(optionSelected){
            case Options.RotateClockWise:
                RotateTilesAround3x3(tile.X, tile.Y);
                break;
            case Options.RotateCounterClockwise:
                break;
            case Options.Destroy:
                DestroyTile(tile.X, tile.Y);
                break;
            case Options.DestroyWithColors:
                DestroyAllTilesOfSameColorAround(tile.X, tile.Y);
                foreach(Tile t in destructionQueue.ToArray()){
                    DestroyTile(t.X, t.Y);
                    destructionQueue.Remove(t);
                }
                break;
        }
       
    }

    public void RotateTilesAround3x3(int x, int y){//TODO: find programgbe way to this if we want to do more than 3x3
        if (y > tiles.GetLength(1) || y - 1 < 0 || x > tiles.GetLength(0) || x-1 < 0)//TODO: FIX THIS FOR DETECTING EDGE
            return;

        Tile tempTile = tiles[x-1,y-1];
        tiles[x - 1, y - 1] = tiles[x, y - 1];
        tiles[x, y - 1] = tiles[x + 1, y-1];
        tiles[x + 1, y - 1] = tiles[x + 1, y];
        tiles[x + 1, y] = tiles[x + 1, y + 1];
        tiles[x + 1, y + 1] = tiles[x, y + 1];
        tiles[x, y + 1] = tiles[x -1, y + 1];
        tiles[x - 1, y + 1] = tiles[x -1, y];
        tiles[x - 1, y] = tempTile;
        RedrawTilesFromLocal();
    }

    public void SetOption(int opt){
        optionSelected = (Options)opt;
    }
    List<Tile> destructionQueue = new List<Tile>();

    private void DestroyAllTilesOfSameColorAround(int x, int y){//TODO: get this fully working, right now its only checking in a 3x3, it was meant to be a recursive function but it keeps giving stack overflows when i do that
        Color color = tiles[x, y].color;
        destructionQueue.Add(tiles[x, y]);

        if(tiles[x+1,y].color == color && destructionQueue.IndexOf(tiles[x + 1, y]) == -1)
        {//right

            DestroyAllTilesOfSameColorAround(x+1,y);
        }

        if (tiles[x - 1, y].color == color && destructionQueue.IndexOf(tiles[x - 1, y]) == -1)//left
        {
            DestroyAllTilesOfSameColorAround(x - 1, y);
        }

        if (tiles[x , y + 1].color == color && destructionQueue.IndexOf(tiles[x , y + 1]) == -1)//top
        {
            DestroyAllTilesOfSameColorAround(x, y + 1);
        }

        if (tiles[x, y - 1].color == color && destructionQueue.IndexOf(tiles[x, y - 1]) == -1)//bottom
        {
            DestroyAllTilesOfSameColorAround(x, y - 1);
        }


        //if (tiles[x + 1, y - 1].color == color)//bottom right
        //{
        //    DestroyTile(x, y + 1);
        //}

        //if (tiles[x - 1, y - 1].color == color)//bottom left
        //{
        //    DestroyTile(x, y + 1);
        //}

        //if (tiles[x + 1, y + 1].color == color)//top right
        //{
        //    DestroyTile(x, y + 1);
        //}

        //if (tiles[x -1, y + 1].color == color)//top left
        //{
        //    DestroyTile(x, y + 1);
        //}

    }
    private void DestroyAllTilesaaOfSameColorAround(int x, int y,int direction)//0 = norht 1 = east 3 = south  4 = east
    {//TODO: get this fully working, right now its only checking in a 3x3, it was meant to be a recursive function but it keeps giving stack overflows when i do that
        Color color = tiles[x, y].color;
        DestroyTile(x, y);

        if (tiles[x + 1, y].color == color)
        {//right
            DestroyTile(x + 1, y);
        }

        if (tiles[x - 1, y].color == color)//left
        {
            DestroyTile(x - 1, y);
        }

        if (tiles[x, y + 1].color == color)//top
        {
            DestroyTile(x, y + 1);
        }

        if (tiles[x, y - 1].color == color)//bottom
        {
            DestroyTile(x, y + 1);
        }


        //if (tiles[x + 1, y - 1].color == color)//bottom right
        //{
        //    DestroyTile(x, y + 1);
        //}

        //if (tiles[x - 1, y - 1].color == color)//bottom left
        //{
        //    DestroyTile(x, y + 1);
        //}

        //if (tiles[x + 1, y + 1].color == color)//top right
        //{
        //    DestroyTile(x, y + 1);
        //}

        //if (tiles[x -1, y + 1].color == color)//top left
        //{
        //    DestroyTile(x, y + 1);
        //}

    }
    private void DestroyTile(int x, int y){
        //Destroy(tiles[x, y].gameObject);
       

        Destroy(tiles[x, y].gameObject);
        //THIS IS SO ALL BLOCK ABOVE FALL DOWN
        for (int scalingY = 0; scalingY < tiles.GetLength(1)-y-1; scalingY++){
            Tile empty = Instantiate(tilePrefab, this.transform).GetComponent<Tile>();
            empty.setIsDead();
            tiles[x, y+ scalingY] = tiles[x, y + 1 + scalingY];
            tiles[x, y + 1 + scalingY] = empty;
        }
        RedrawTilesFromLocal();

    }

}
