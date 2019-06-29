using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBoard : MonoBehaviour
{
    public GlobalEnums.FlipOptions flipOption;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

    public void Flip()
    {
        tileManager.FlipBoard(flipOption);
    }
}
