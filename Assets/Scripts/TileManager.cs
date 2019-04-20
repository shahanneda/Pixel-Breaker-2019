﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class TileManager : MonoBehaviour
{

    public static Tile[,] tiles;
    public float gravityCheckFloat = 0.1f;
    public Color[] colors;

    Options optionSelected;
    SelectionMode currentSelectionMode;
    TileGrid grid;
    TileActions tileActions;

    List<Tile> SelectedTilesGroupOne = new List<Tile>();
    List<Tile> SelectedTilesGroupTwo = new List<Tile>();

    /*
     * @ADAM, whenever we want to move the tiles, if we just move the items of this array, and call RedrawTilesFromLocal(), everything should be handeld
     * 
     */
    void Start()
    {
        Tile.manager = this;
        TileGrid.manager = this;
        TileActions.manager = this;

        grid = GetComponent<TileGrid>();
        tileActions = GetComponent<TileActions>();
        grid.SetUp();
    }



    /// <summary>
    /// summary: this method is suppsued to move all of the tiles to their correct position in the game world after
    /// we rotate the local grid or something
    /// CALL THIS WHENEVER YOU CHANGE THE BOARD
    /// </summary>
    public void RedrawTilesFromLocal(){
        //Note to self If proframce becomes issue think about not doing this every time;
        for (int x = 0; x < tiles.GetLength(0); x++){
            for (int y = 0; y < tiles.GetLength(1); y++){
                tiles[x, y].shouldMoveTo = grid.tileGamePosVec(x, y);//Update pos from local
                tiles[x, y].inAnimation = true;
                tiles[x, y].X = x;
                tiles[x, y].Y = y;
            }
        }

    }

    /// <summary>
    /// Decides what to do when a tile is click
    /// 
    /// called from tile.cs
    /// <paramref name="tile"/>. 
    /// </summary>
    /// <param name="tile">The tile clicked</param>
    public void HandleTileClick(Tile tile)
    { // Here is where we decide what we should do wheter that a rotate or somethin like that;
        switch (optionSelected)
        {
            case Options.RotateClockWise:
                tileActions.RotateTilesAround3x3(tile.X, tile.Y);
                break;
            case Options.RotateCounterClockwise:
                tileActions.AntiRotateTilesAround3x3(tile.X, tile.Y);

                break;
            case Options.Destroy:
                DestroyTile(tile.X, tile.Y);
                break;
            case Options.DestroyWithColors:
                DestroyAllTilesOfSameColorAround(tile.X, tile.Y);
                break;
            case Options.Rotate3x3Right:
                tileActions.Rotate3x3Tiles(tile.X, tile.Y);
                break;
            case Options.ThreeByThreeSwitch:
                ThreeByThreeSwitch(tile.X,tile.Y);
                break;
        }
    }

    public void ThreeByThreeSwitch(int x, int y){

        if(SelectedTilesGroupOne.Count > 0){
           
            Tile[] tilesToSwitchTo = GetTilesIn3x3(tiles[x,y]);
            //add check for edge TODO
            SwitchTiles(SelectedTilesGroupOne.ToArray(), tilesToSwitchTo);
            SelectTiles(SelectedTilesGroupOne.ToArray(), false);
            SelectTiles(tilesToSwitchTo, false);
            SelectedTilesGroupOne.Clear();

        }else{
            SelectedTilesGroupOne.AddRange(GetTilesIn3x3(tiles[x, y]));
            SelectTiles(SelectedTilesGroupOne.ToArray(), true);

        }
    }

    private void SwitchTiles(Tile[] group1, Tile[] group2){
        if(group1.Length != group2.Length){
            return;
        }
        for (int i = 0; i < group1.Length; i++){
            //Tile tile = tiles[group2[i].X, group2[i].Y];
            tiles[group2[i].X, group2[i].Y] = group1[i];
            tiles[group1[i].X, group1[i].Y] = group2[i];
        }

        RedrawTilesFromLocal();
        CheckForGravity();
    }

    private void SelectTiles(Tile[] tilesToSelect, bool state){
        foreach(Tile t in tilesToSelect)
        {
            t.setHover(false);
            t.setSelect(state);
        }
    }
    /// <summary>
    /// Sets the option.
    /// Used from Unity UI Buttons
    /// </summary>
    /// <param name="opt">Opt.</param>
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
            case Options.ThreeByThreeSwitch:
                currentSelectionMode = SelectionMode.SaveSelection;
                break;
        }

    }
    /// <summary>
    /// Queue of objects that need to be destroyed
    /// </summary>
    public List<Tile> destructionQueue = new List<Tile>();
    /// <summary>
    /// Queue of objects that need to be checked for gravity updates.
    /// </summary>
    public List<Tile> gravityQueue = new List<Tile>();

    /// <summary>
    /// Metheod called by invoke
    /// </summary>
    public void GravityInvoke(){
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
        foreach (Tile t in destructionQueue.ToArray()) {
            DestroyTile(t.X, t.Y);
            destructionQueue.Remove(t);
        }
    }
    private void CheckNearbyTileColors(int x, int y){
        Color color = tiles[x, y].color;
        destructionQueue.Add(tiles[x, y]);
        if (x + 1 < tiles.GetLength(0) && tiles[x + 1, y].color == color && destructionQueue.IndexOf(tiles[x + 1, y]) == -1){//right

            CheckNearbyTileColors(x + 1, y);
        }
        if (x - 1 > 0 && tiles[x - 1, y].color == color && destructionQueue.IndexOf(tiles[x - 1, y]) == -1){//left
        
            CheckNearbyTileColors(x - 1, y);
        }
        if (y + 1 < tiles.GetLength(1) && tiles[x, y + 1].color == color && destructionQueue.IndexOf(tiles[x, y + 1]) == -1){//top
        
            CheckNearbyTileColors(x, y + 1);
        }
        if (y - 1 > 0 && tiles[x, y - 1].color == color && destructionQueue.IndexOf(tiles[x, y - 1]) == -1){//bottom
        
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
    /// <summary>
    /// Handles the tile mouse over. 
    /// Called form tile
    /// </summary>
    /// <param name="tile">Tile.</param>
    public void HandleTileMouseOver(Tile tile){
        switch (currentSelectionMode){
            case SelectionMode.Single:
                tile.setHover(true);
                break;
            case SelectionMode.ThreeByThree:
                foreach (Tile t in GetTilesIn3x3(tile)){
                    t.setHover(true);
                }
                break;
            case SelectionMode.SaveSelection:
                foreach (Tile t in GetTilesIn3x3(tile))
                {
                    t.setHover(true);
                }
                break;
        }
       
    }
    /// <summary>
    /// Handles the tile mouse exit.
    /// </summary>
    /// <param name="tile">Tile.</param>
    public void HandleTileMouseExit(Tile tile){
        switch (currentSelectionMode){
            case SelectionMode.Single:
                tile.setHover(false);
                break;
            case SelectionMode.ThreeByThree:
                foreach (Tile t in GetTilesIn3x3(tile))
                {
                    t.setHover(false);
                }
                break;
            case SelectionMode.SaveSelection:
                if(!SelectedTilesGroupOne.Contains(tile)){
                    foreach (Tile t in GetTilesIn3x3(tile))
                    {
                        if(!SelectedTilesGroupOne.Contains(t)){
                            t.setHover(false);
                        }
                    }
                    //tile.setSelect(false);
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

    public bool TileIsOnEdge(Tile t){
        return (t.X - 1 < 0 || t.X + 1 > tiles.GetLength(0) - 1 || t.Y - 1 < 0 || t.Y + 1 > tiles.GetLength(1) - 1);
    }
}
