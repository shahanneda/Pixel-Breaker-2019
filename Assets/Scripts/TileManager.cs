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
        FillTiles();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FillTiles(){
        for (int i = 0; i < tiles.GetLength(0); i++){
            for (int k = 0; k < tiles.GetLength(1); k++){
                tiles[i,k] = (Instantiate(tilePrefab,
                      new Vector2(transform.position.x + i * tileSpaceX, transform.position.y + k * tileSpaceY), 
                      Quaternion.identity, transform) as GameObject)
                    .GetComponent<Tile>();                              
            }
        }

    }
}
