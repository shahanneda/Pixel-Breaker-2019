using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static TileManager manager;
    public int X; // TODO: mayber rethink this
    public int Y;
    //THIS CLASS IS HERE JUST FOR THE BUTTON CLICKS, BUT TILES THEMSELVES SHOULD NOT HANDLE ANY ACTIONS, ISNTEAD
    //THEY SHOULD REPORT THAT MESSAGE TO THEIR MANAGER!
    void OnMouseDown(){
        manager.HandleTileClick(this);
    }
    public void Start()
    {
        //<TESTING>
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.2f, 0.7f), Random.Range(0.2f, 0.8f), 0.5f);
         

        //</testing>
    }
}
