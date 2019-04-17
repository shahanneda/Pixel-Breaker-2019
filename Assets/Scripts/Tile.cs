﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static TileManager manager;

    [HideInInspector]
    public int X; 
    [HideInInspector]
    public int Y;

    public float speed = 1f;

    public bool inAnimation = false;

    [HideInInspector]
    public Vector2 shouldMoveTo = Vector2.positiveInfinity;
    //THIS CLASS IS HERE JUST FOR THE BUTTON CLICKS, BUT TILES THEMSELVES SHOULD NOT HANDLE ANY ACTIONS, ISNTEAD
    //THEY SHOULD REPORT THAT MESSAGE TO THEIR MANAGER!
    void OnMouseDown(){
        manager.HandleTileClick(this);
    }
    public void Start()
    {
        //<TESTING>
        GetComponent<SpriteRenderer>().color = manager.colors[Random.Range(0,manager.colors.Length-1)];
         

        //</testing>
    }
    public void Update()
    {
        if(inAnimation){
            transform.position = Vector2.Lerp(transform.position, shouldMoveTo, speed * Time.deltaTime);
        }
    }
}
