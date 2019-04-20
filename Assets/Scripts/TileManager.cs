using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int gameWidth = 10;
    public int gameHeight = 20;

    public float tileSpaceX = 1;//TODO: maybe make this  auto generated for optimal setting maybe?
    public float tileSpaceY = 1;
    public float gravityCheckFloat = 0.1f;
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
                AntiRotateTilesAround3x3(tile.X, tile.Y);
              
                break;
            case Options.Destroy:
                DestroyTile(tile.X, tile.Y);
                break;
            case Options.DestroyWithColors:
                DestroyAllTilesOfSameColorAround(tile.X, tile.Y);
                ;
                break;
        }
       
    }
    //@ADAM, create the other ones of these from the design doc, draw it out on paper to help you lundersd which one is which, and be ready to be frustrated 
    public void RotateTilesAround3x3(int x, int y){
        if (x-1 < 0 || x+1 > tiles.GetLength(0)-1 || y-1 < 0 || y+1 > tiles.GetLength(1)-1 )//for checking
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
       
        gravityQueue.AddRange(new List<Tile> { tiles[x,y],tiles[x + 1, y + 1], tiles[x, y + 1], tiles[x - 1, y + 1], tiles[x - 1, y], tiles[x - 1, y - 1], tiles[x, y - 1],
        tiles[x + 1, y - 1],tiles[x + 1, y]
        });
        Invoke("GravityInvoke", gravityCheckFloat);
        RedrawTilesFromLocal();
    }
    public void AntiRotateTilesAround3x3(int x, int y)//TODO: make this rotate left
    {
        if (x - 1 < 0 || x + 1 > tiles.GetLength(0) - 1 || y - 1 < 0 || y + 1 > tiles.GetLength(1) - 1)
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

        gravityQueue.AddRange(new List<Tile> { tiles[x,y],tiles[x + 1, y + 1], tiles[x, y + 1], tiles[x - 1, y + 1], tiles[x - 1, y], tiles[x - 1, y - 1], tiles[x, y - 1],
        tiles[x + 1, y - 1],tiles[x + 1, y]
        });
        Invoke("GravityInvoke",gravityCheckFloat);
        RedrawTilesFromLocal();
    }

    public void SetOption(int opt){
        optionSelected = (Options)opt;
    }
    List<Tile> destructionQueue = new List<Tile>();
    List<Tile> gravityQueue = new List<Tile>();

    private void GravityInvoke(){
        CheckForGravity();
    }
    private void CheckForGravity(int count = 0){
        
        print(gravityQueue.ToArray().Length);

        foreach(Tile tile in gravityQueue.ToArray()){
            if(tile.Y+1 < tiles.GetLength(1) && !tiles[tile.X, tile.Y+1].isDead && !gravityQueue.Contains(tiles[tile.X, tile.Y + 1])){
                gravityQueue.Add(tiles[tile.X, tile.Y + 1]);//NOTE this line is adding multiple tiems @ preformance if needed
            }
            if(tile.Y != 0){
                for (int scalingY = tile.Y; tiles[tile.X, scalingY - 1].isDead; scalingY--)
                {

                    Tile temp = tiles[tile.X, scalingY];
                    tiles[tile.X, scalingY] = tiles[tile.X, scalingY - 1];
                    tiles[tile.X, scalingY - 1] = temp;
                    tiles[tile.X, scalingY].isFalling = true;
                    tiles[tile.X, scalingY - 1].isFalling = true;
                }
            }

           
            if(count >= 3){
                gravityQueue.Remove(tile);
            }
        }
        if(count <= 3){
            CheckForGravity(count+1);
        }else{
            RedrawTilesFromLocal();
        }
        

    }
    private void DestroyAllTilesOfSameColorAround(int x, int y){
        CheckNearbyTileColors(x,y);
        ApplyDestructionQueue();

    }

    private void ApplyDestructionQueue(){
        foreach (Tile t in destructionQueue.ToArray())
        {
            DestroyTile(t.X, t.Y);
            destructionQueue.Remove(t);
        }
    }
    private void CheckNearbyTileColors(int x, int y){
        Color color = tiles[x, y].color;
        destructionQueue.Add(tiles[x, y]);
        if (x + 1 < tiles.GetLength(0) && tiles[x + 1, y].color == color && destructionQueue.IndexOf(tiles[x + 1, y]) == -1)//right
        {

            CheckNearbyTileColors(x + 1, y);
        }

        if (x - 1 > 0 && tiles[x - 1, y].color == color && destructionQueue.IndexOf(tiles[x - 1, y]) == -1)//left
        {
            CheckNearbyTileColors(x - 1, y);
        }

        if (y + 1 < tiles.GetLength(1) && tiles[x, y + 1].color == color && destructionQueue.IndexOf(tiles[x, y + 1]) == -1)//top
        {
            CheckNearbyTileColors(x, y + 1);
        }

        if (y - 1 > 0 && tiles[x, y - 1].color == color && destructionQueue.IndexOf(tiles[x, y - 1]) == -1)//bottom
        {
            CheckNearbyTileColors(x, y - 1);
        }
    }
    private void DestroyTile(int x, int y){
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
