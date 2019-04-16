using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    List<Tile> tiles = new List<Tile>();
    // Start is called before the first frame update
    void Start()
    {
		foreach(Transform t in transform){
            tiles.Add(t.GetComponent<Tile>());
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
