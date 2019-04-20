using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{

    public int gameWidth = 10;
    public int gameHeight = 20;
    public float tileSpaceX = 1;//TODO: maybe make this  auto generated for optimal setting maybe?
    public float tileSpaceY = 1;
    public GameObject tilePrefab;
    public static TileManager manager;

    public void SetUp(){
        manager.tiles = new Tile[gameWidth, gameHeight];
        FillTiles();
    }

    private void FillTiles()
    {
        for (int x = 0; x < manager.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < manager.tiles.GetLength(1); y++)
            {
                manager. tiles[x, y] = (Instantiate(tilePrefab,
                     tileGamePosVec(x, y),
                     Quaternion.identity, transform) as GameObject)
                    .GetComponent<Tile>();

                manager.tiles[x, y].X = x;//gives each tile its position in the grid
                manager.tiles[x, y].Y = y;

            }
        }

    }

    public Vector2 tileGamePosVec(int x, int y)
    {
        return new Vector2(transform.position.x + x * tileSpaceX, transform.position.y + y * tileSpaceY);
    }
}
