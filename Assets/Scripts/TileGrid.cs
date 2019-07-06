using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileManager;


/// <summary>
/// Tile grid.
/// Responsible for setting up the grid of tiles
/// </summary>
public class TileGrid : MonoBehaviour
{
    public int gameWidth = 10;
    public int gameHeight = 20;
    public int numberOfRowsFilled = 7;
    public float tileSpaceX = 1;//TODO: maybe make this  auto generated for optimal setting maybe?
    public float tileSpaceY = 1;
    public GameObject tilePrefab;
    public static TileManager manager;
    private Tile[] loadingArea;
    public float loadingAreaY = -2.73f;

    public Transform board;

    public void SetUp()
    {
        tiles = new Tile[gameWidth, gameHeight];
        loadingArea = new Tile[gameWidth];
        FillTiles();
    }

    public Tile AddTile(int x, int y)
    {
        tiles[x, y] = GetNewTile(x, y);
        return tiles[x, y];
    }
    public Tile AddTileToLoadingArea(int x)
    {
        Tile t = (Instantiate(tilePrefab, new Vector2(transform.position.x + x * tileSpaceX, loadingAreaY * 2.88340611f), Quaternion.identity, board) as GameObject).GetComponent<Tile>();
        loadingArea[x] = t;
        t.setIsInLoadingArea(true);
        return t;
    }
    public void fillTileLoadingArea()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            AddTileToLoadingArea(i).ChooseRandomSprite();
        }
    }
    public void SwitchTilesFromLoadingAreaToLastRow(){
        for (int i = 0; i < tiles.GetLength(0); i++){
            Tile tileToLoad = loadingArea[i];
            tileToLoad.setIsInLoadingArea(false);
            tileToLoad.X = i;
            tileToLoad.Y = 0;
            tileToLoad.shouldMoveTo = tileGamePosVec(tileToLoad.X, tileToLoad.Y); //Update pos from local
            tileToLoad.inAnimation = true;
            print(tileToLoad.Y);
            tiles[tileToLoad.X, tileToLoad.Y] = tileToLoad;
        }
    }
    public Tile AddTile(int x, int y, Sprite sprite)
    {
        AddTile(x, y);
        tiles[x, y].SetSprite(sprite);
        return tiles[x, y];
    }
    public Tile GetNewTile(int x, int y){
        Tile t = (Instantiate(tilePrefab, tileGamePosVec(x, y), Quaternion.identity, board) as GameObject).GetComponent<Tile>();
        t.X = x;
        t.Y = y;
        return t;
    }

    public void RegisterStandbyTilePositionInBoard(Tile t, int x, int y){
        t.X = x;
        t.Y = y;
        t.transform.position = tileGamePosVec(x, y);
    }

    private void FillTiles()
    {
        fillTileLoadingArea();
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < numberOfRowsFilled; y++)
            {
                AddTile(x, y).ChooseRandomSprite(); ;
            }
            for (int y = numberOfRowsFilled; y < tiles.GetLength(1); y++)
            {
                AddTile(x, y).setIsDead();
            }
        }

    }

    public Vector2 tileGamePosVec(int x, int y)
    {
        return new Vector2(transform.position.x + x * tileSpaceX, transform.position.y + y * tileSpaceY);
    }
}
