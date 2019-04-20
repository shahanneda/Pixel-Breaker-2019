using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class TileManager : MonoBehaviour
{

    public Tile[,] tiles;
    public float gravityCheckFloat = 0.1f;
    public Color[] colors;
    Options optionSelected;
    SelectionMode currentSelectionMode;
    TileGrid grid;

    /*
     * @ADAM, whenever we want to move the tiles, if we just move the items of this array, and call RedrawTilesFromLocal(), everything should be handeld
     * 
     */
    // Start is called before the first frame update
    void Start()
    {
        Tile.manager = this;//sets this as the manager for all the tiel
        TileGrid.manager = this;

        grid = GetComponent<TileGrid>();
        grid.SetUp();
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
                tiles[x, y].shouldMoveTo = grid.tileGamePosVec(x, y);//Update pos from local

                tiles[x, y].inAnimation = true;

                tiles[x, y].X = x;
                tiles[x, y].Y = y;
            }
        }

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
            case Options.Rotate3x3Right:
                Rotate3x3Tiles(tile.X,tile.Y);
                break;
        }
       
    }
    public void Rotate3x3Tiles(int x, int y){
        if (TileIsOnEdge(tiles[x, y]))
            return;
        print("Test");
        Tile bottomLeft   = tiles[x - 1,y - 1];
        Tile bottomMiddle = tiles[x, y - 1];
        Tile bottomRight  = tiles[x + 1, y - 1];
        Tile topLeft = tiles[x + 1, y + 1];

        tiles[x + 1, y - 1] = tiles[x + 1, y + 1]; // bottom right = top right
        tiles[x, y - 1] = tiles[x + 1, y];
        tiles[x - 1, y - 1] = bottomRight;

        tiles[x + 1, y + 1] = tiles[x - 1, y + 1];
        tiles[x + 1, y]     = tiles[x    , y + 1];
        tiles[x + 1, y - 1] = topLeft;

        tiles[x + 1, y + 1] = tiles[x - 1, y + 1];
        tiles[x,     y + 1] = tiles[x - 1, y];
        tiles[x - 1, y + 1] = bottomLeft;

        tiles[x - 1, y + 1] = bottomLeft;
        tiles[x - 1, y]     = bottomMiddle;
        tiles[x - 1, y - 1] = bottomRight;

        gravityQueue.AddRange(new List<Tile> { tiles[x,y],tiles[x + 1, y + 1], tiles[x, y + 1], tiles[x - 1, y + 1], tiles[x - 1, y], tiles[x - 1, y - 1], tiles[x, y - 1],
        tiles[x + 1, y - 1],tiles[x + 1, y]
        });
        Invoke("GravityInvoke", gravityCheckFloat);
        RedrawTilesFromLocal();
    }

    //@ADAM, create the other ones of these from the design doc, draw it out on paper to help you lundersd which one is which, and be ready to be frustrated 
    public void RotateTilesAround3x3(int x, int y){
        if (TileIsOnEdge(tiles[x, y]))
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
        if (TileIsOnEdge(tiles[x,y]))
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
        switch(optionSelected){
            case Options.Destroy:
            case Options.DestroyWithColors:
                currentSelectionMode = SelectionMode.Single;
                break;

            case Options.Rotate3x3Right:
            case Options.RotateClockWise:
            case Options.RotateCounterClockwise:
                currentSelectionMode = SelectionMode.ThreeByThree;
                break;
        }

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
            Tile empty = Instantiate(grid.tilePrefab, this.transform).GetComponent<Tile>();
            empty.setIsDead();
            tiles[x, y+ scalingY] = tiles[x, y + 1 + scalingY];
            tiles[x, y + 1 + scalingY] = empty;
        }
        RedrawTilesFromLocal();

    }

    public void HandleTileMouseOver(Tile tile){
        switch (currentSelectionMode){
            case SelectionMode.Single:
                tile.setSelect(true);
                break;
            case SelectionMode.ThreeByThree:
                foreach (Tile t in GetTilesIn3x3(tile)){
                    t.setSelect(true);
                }
                break;
        }
       
    }
    public void HandleTileMouseExit(Tile tile)
    {
        switch (currentSelectionMode)
        {
            case SelectionMode.Single:
                tile.setSelect(false);
                break;
            case SelectionMode.ThreeByThree:
                foreach (Tile t in GetTilesIn3x3(tile))
                {
                    t.setSelect(false);
                }
                break;
        }
    }

    private Tile[] GetTilesIn3x3(Tile tile){
        if(TileIsOnEdge(tile)){
            return new Tile[] { tile };
        }
        return new Tile[]{
            tiles[tile.X-1,tile.Y+1], tiles[tile.X,tile.Y+1], tiles[tile.X+1,tile.Y+1],
            tiles[tile.X-1,tile.Y  ], tiles[tile.X,tile.Y  ], tiles[tile.X+1,tile.Y  ],
            tiles[tile.X-1,tile.Y-1], tiles[tile.X,tile.Y-1], tiles[tile.X+1,tile.Y-1]
        };
    }

    private bool TileIsOnEdge(Tile t){
        return (t.X - 1 < 0 || t.X + 1 > tiles.GetLength(0) - 1 || t.Y - 1 < 0 || t.Y + 1 > tiles.GetLength(1) - 1);
    }
}
