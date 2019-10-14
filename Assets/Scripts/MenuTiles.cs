using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTiles : MonoBehaviour
{
    public GameObject[] tiles;

    private float maxX = 0;
    private float maxY = 0;

    public float minTime = 2f;
    public float maxTime = 6f;
    private int previousIndex = -1;

    private void Start()
    {
        StartCoroutine(ShowTile());

        maxX = 0.468f * Screen.width;
        maxY = 0.444f * Screen.height;
    }

    private IEnumerator ShowTile()
    {
        yield return new WaitForSeconds(Random.Range(minTime,maxTime));

        int index = Random.Range(0, tiles.Length);

        while (index == previousIndex)
        {
            index = Random.Range(0, tiles.Length);
        }

        previousIndex = index;

        GameObject tile = tiles[index];
        tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
        tile.GetComponent<Animator>().Play("Fade In");

        StartCoroutine(ShowTile());
    }
}
