using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static TileManager manager;
    public int X; // TODO: mayber rethink this
    public int Y;
    public float speed = 1f;
    public bool inAnimation = false;
    public Vector2 shouldMoveTo = Vector2.positiveInfinity;
    //THIS CLASS IS HERE JUST FOR THE BUTTON CLICKS, BUT TILES THEMSELVES SHOULD NOT HANDLE ANY ACTIONS, ISNTEAD
    //THEY SHOULD REPORT THAT MESSAGE TO THEIR MANAGER!
    void OnMouseDown(){
        manager.HandleTileClick(this);
    }
    public void Start()
    {
        //<TESTING>
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.2f, 0.7f), Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
         

        //</testing>
    }
    public void Update()
    {
        if(inAnimation){
            transform.position = Vector2.Lerp(transform.position, shouldMoveTo, speed * Time.deltaTime);
        }
    }
}
