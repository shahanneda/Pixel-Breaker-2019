using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBoard : MonoBehaviour
{
    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

    public void Flip()
    {

    }
}
