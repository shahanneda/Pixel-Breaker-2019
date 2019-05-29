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
    public float tileSpaceX = 1;//TODO: maybe make this  auto generated for optimal setting maybe?
    public float tileSpaceY = 1;
    public GameObject tilePrefab;
    public static TileManager manager;

    public void SetUp()
    {
        tiles = new Tile[gameWidth, gameHeight];
        FillTiles();
    }

    public Tile AddTile(int x, int y)
    {
        tiles[x, y] = (Instantiate(tilePrefab, tileGamePosVec(x, y), Quaternion.identity, transform) as GameObject).GetComponent<Tile>();

        tiles[x, y].X = x;
        tiles[x, y].Y = y;

        return tiles[x, y];
    }

    public Tile AddTile(int x, int y, Sprite sprite)
    {
        tiles[x, y] = (Instantiate(tilePrefab, tileGamePosVec(x, y), Quaternion.identity, transform) as GameObject).GetComponent<Tile>();

        tiles[x, y].X = x;
        tiles[x, y].Y = y;

        tiles[x, y].SetSprite(sprite);

        return tiles[x, y];
    }

    private void FillTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                AddTile(x, y).ChooseRandomSprite(); ;
            }
        }

    }

    public Vector2 tileGamePosVec(int x, int y)
    {
        return new Vector2(transform.position.x + x * tileSpaceX, transform.position.y + y * tileSpaceY);
    }
}
