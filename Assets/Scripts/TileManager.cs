using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;

public class TileManager : MonoBehaviour
{
    public static Tile[,] tiles;
    public float gravityCheckFloat = 0.1f;
    public Sprite[] sprites;

    public Transform board;

    public GameObject selectCardColorMenu;

    public MusicManager musicManager;

    public AudioSource sfxSource;
    public AudioClip tileActionClip;

    public Text movesUntilNextRow;

    Options optionSelected = Options.DestroyWithColors;
    SelectionMode currentSelectionMode = SelectionMode.Single;
    TileGrid grid;
    TileActions tileActions;
    public TileGravity tileGravity;

    List<Tile> SelectedTilesGroupOne = new List<Tile>();
    List<Tile> SelectedTilesGroupTwo = new List<Tile>();

    private ScoreManager scoreManager;
    private CardManager cardManager;

    private List<Tile> savedTiles = new List<Tile>();
    private Sprite colorOfSwitch;

    private int amountOfTurns = 0;
    private int amountOfTurnsToAddRow = 4;
    private int addRowCounter;

    public bool CanSelectTile { get; set; }

    public bool isPlaying = true;

    void Start()
    {
        Tile.manager = this;
        TileGrid.manager = this;
        TileActions.manager = this;
        TileGravity.manager = this;

        scoreManager = FindObjectOfType<ScoreManager>();
        cardManager = FindObjectOfType<CardManager>();
        musicManager = FindObjectOfType<MusicManager>();

        sfxSource = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();

        tileGravity = GetComponent<TileGravity>();
        grid = GetComponent<TileGrid>();
        tileActions = GetComponent<TileActions>();
        grid.SetUp();

        CanSelectTile = true;
    }

    /// <summary>
    /// summary: this method is suppsued to move all of the tiles to their correct position in the game world after
    /// we rotate the local grid or something
    /// CALL THIS WHENEVER YOU CHANGE THE BOARD
    /// </summary>
    public void RedrawTilesFromLocal()
    {
        //Note to self If proframce becomes issue think about not doing this every time;
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].shouldMoveTo = grid.tileGamePosVec(x, y); //Update pos from local
                tiles[x, y].inAnimation = true;
                tiles[x, y].X = x;
                tiles[x, y].Y = y;
            }
        }
    }

    /// <summary>
    /// Decides what to do when a tile is click
    /// 
    /// called from tile.cs
    /// <paramref name="tile"/>. 
    /// </summary>
    /// <param name="tile">The tile clicked</param>
    public void HandleTileClick(Tile tile)
    {
        if (CanSelectTile && !tile.isDead && isPlaying)
        {
            switch (optionSelected)
            {
                case Options.FlipHorizontal:
                    FlipBoard(FlipOptions.Horizontal);
                    SetOption((int)Options.DestroyWithColors);
                    break;
                case Options.FlipVertical:
                    FlipBoard(FlipOptions.Vertical);
                    SetOption((int)Options.DestroyWithColors);
                    break;
                case Options.SwitchEdgeRows:
                    if (optionSelected.Equals(Options.SwitchEdgeRows))
                        SwitchEdgeRows();
                    SetOption((int)Options.DestroyWithColors);
                    break;
                case Options.SwitchEdgeColumns:
                    SwitchEdgeColumns();
                    SetOption((int)Options.DestroyWithColors);
                    break;
                case Options.Rotate3x3Left90Degrees:
                    tileActions.Rotate3x3Left90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Left90Degrees(tile.X, tile.Y);
                    break;
                case Options.Rotate3x3Left180Degrees:
                    tileActions.Rotate3x3Left90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Left90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Left90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Left90Degrees(tile.X, tile.Y);
                    break;
                case Options.Rotate3x3Right90Degrees:
                    tileActions.Rotate3x3Right90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Right90Degrees(tile.X, tile.Y);
                    break;
                case Options.Rotate3x3Right180Degrees:
                    tileActions.Rotate3x3Right90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Right90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Right90Degrees(tile.X, tile.Y);
                    tileActions.Rotate3x3Right90Degrees(tile.X, tile.Y);
                    break;
                case Options.Rotate2x2Left90Degrees:
                    tileActions.Rotate2x2Left90Degrees(tile.X, tile.Y);
                    break;
                case Options.Rotate2x2Left180Degrees:
                    Vector2 tilePos = new Vector2(tile.X, tile.Y);
                    tileActions.Rotate2x2Left90Degrees((int)tilePos.x, (int)tilePos.y);
                    tileActions.Rotate2x2Left90Degrees((int)tilePos.x, (int)tilePos.y);
                    break;
                case Options.Destroy:
                    DestroyTile(tile.X, tile.Y, true);
                    break;
                case Options.DestroyWithColors:
                    DestroyAllTilesOfSameColorAround(tile.X, tile.Y);
                    AddRowCounter();
                    break;
                case Options.ThreeByThreeSwitch:
                    ThreeByThreeSwitch(tile.X, tile.Y);
                    break;
                case Options.SwitchColorOfOne:
                    //selectedTile = tile;
                    savedTiles.Add(tile);
                    selectCardColorMenu.SetActive(true);
                    CanSelectTile = false;
                    break;
                case Options.TranslateOneTile:
                    if (savedTiles.Count <= 0)
                    {
                        savedTiles.Add(tile);
                        tile.setSelect(true);
                    }
                    else
                    {
                        if (!savedTiles.Contains(tile))
                        {
                            savedTiles[0].setSelect(false);

                            SwitchTiles(savedTiles[0], tile);
                            savedTiles.Clear();

                            AfterTurnChecks();
                        }
                    }
                    break;
                case Options.SwitchAdjacentRows:
                    if (savedTiles.Count <= 0)
                    {
                        savedTiles.Add(tile);

                        for (int i = 0; i < grid.gameWidth; i++)
                        {
                            tiles[i, savedTiles[0].Y].setSelect(true);
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(tile.Y - savedTiles[0].Y) == 1)
                        {
                            for (int i = 0; i < grid.gameWidth; i++)
                            {
                                tiles[i, savedTiles[0].Y].setSelect(false);
                            }

                            SwitchRowOfTiles(savedTiles[0].Y, tile.Y);
                            savedTiles.Clear();

                            tileGravity.RunCheckDelayed(0.4f);
                            AfterTurnChecks();
                        }
                    }
                    break;
                case Options.SwitchAdjacentColumns:
                    if (savedTiles.Count <= 0)
                    {
                        savedTiles.Add(tile);

                        for (int i = 0; i < grid.gameHeight; i++)
                        {
                            tiles[savedTiles[0].X, i].setSelect(true);
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(tile.X - savedTiles[0].X) == 1)
                        {
                            for (int i = 0; i < grid.gameHeight; i++)
                            {
                                tiles[savedTiles[0].X, i].setSelect(false);
                            }

                            SwitchColumnOfTiles(savedTiles[0].X, tile.X);
                            savedTiles.Clear();

                            AfterTurnChecks();
                        }
                    }
                    break;
                case Options.HorizontalFlip2x2:
                    try
                    {
                        SwitchTiles(tiles[tile.X, tile.Y + 1], tiles[tile.X + 1, tile.Y + 1]);
                        SwitchTiles(tile, tiles[tile.X + 1, tile.Y]);

                        AfterTurnChecks();
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case Options.VerticalFlip2x2:
                    try
                    {
                        SwitchTiles(tiles[tile.X + 1, tile.Y], tiles[tile.X + 1, tile.Y + 1]);
                        SwitchTiles(tiles[tile.X, tile.Y], tiles[tile.X, tile.Y + 1]);

                        AfterTurnChecks();
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case Options.HorizontalFlip3x3:
                    SwitchTiles(tiles[tile.X - 1, tile.Y + 1], tiles[tile.X + 1, tile.Y + 1]);
                    SwitchTiles(tiles[tile.X - 1, tile.Y], tiles[tile.X + 1, tile.Y]);
                    SwitchTiles(tiles[tile.X - 1, tile.Y - 1], tiles[tile.X + 1, tile.Y - 1]);

                    AfterTurnChecks();

                    break;
                case Options.VerticalFlip3x3:
                    SwitchTiles(tiles[tile.X - 1, tile.Y + 1], tiles[tile.X - 1, tile.Y - 1]);
                    SwitchTiles(tiles[tile.X, tile.Y + 1], tiles[tile.X, tile.Y - 1]);
                    SwitchTiles(tiles[tile.X + 1, tile.Y + 1], tiles[tile.X + 1, tile.Y - 1]);

                    AfterTurnChecks();

                    break;
                case Options.SwitchColorOfTwo:
                    if (savedTiles.Contains(tile))
                    {
                        savedTiles.Remove(tile);
                        tile.setSelect(false);
                    }
                    else
                    {
                        savedTiles.Add(tile);
                        tile.setSelect(true);

                        if (savedTiles.Count == 2)
                        {
                            CanSelectTile = false;
                            selectCardColorMenu.SetActive(true);
                        }
                    }
                    break;
                case Options.SwitchColorOfThree:
                    if (savedTiles.Contains(tile))
                    {
                        savedTiles.Remove(tile);
                        tile.setSelect(false);
                    }
                    else
                    {
                        if (tile.sprite.Equals(colorOfSwitch))
                        {
                            savedTiles.Add(tile);
                            tile.setSelect(true);
                        }

                        if (savedTiles.Count == 3)
                        {
                            CanSelectTile = false;
                            selectCardColorMenu.SetActive(true);
                        }
                    }
                    break;
                case Options.SwitchAdjacent2x2:
                    if (savedTiles.Count == 0 || Vector2.Distance(new Vector2(tile.X, tile.Y), new Vector2(savedTiles[0].X, savedTiles[0].Y)) <= 2)
                    {
                        if (savedTiles.Count == 1)
                        {
                            Tile[] tileGroup1 = GetTilesIn2x2(savedTiles[0]);
                            Tile[] tileGroup2 = GetTilesIn2x2(tile);

                            for (int i = 0; i < 4; i++)
                            {
                                tileGroup1[i].setSelect(false);
                                SwitchTiles(tileGroup1[i], tileGroup2[i]);
                            }

                            savedTiles.Clear();

                            tileGravity.RunCheck();
                            AfterTurnChecks();
                            break;
                        }

                        savedTiles.Add(tile);

                        foreach (Tile t in GetTilesIn2x2(tile))
                        {
                            t.setSelect(true);
                        }
                    }
                    break;
                case Options.Move3ToTop:
                    savedTiles.Add(tile);
                    tile.setSelect(true);

                    if (savedTiles.Count == 3)
                    {
                        foreach (Tile t in savedTiles)
                        {
                            t.setSelect(false);
                            SwitchTiles(t, tiles[t.X, grid.gameHeight - 1]);
                        }

                        savedTiles.Clear();

                        tileGravity.RunCheck();
                        AfterTurnChecks();
                    }
                    break;
                case Options.Move2x2ToTop:
                    if (TwoByTwoPossible(tile))
                    {
                        Tile[] tiles2x2 = GetTilesIn2x2(tile);

                        SwitchTiles(tiles2x2[0], tiles[tiles2x2[0].X, grid.gameHeight - 2]);
                        SwitchTiles(tiles2x2[1], tiles[tiles2x2[1].X, grid.gameHeight - 2]);
                        SwitchTiles(tiles2x2[2], tiles[tiles2x2[2].X, grid.gameHeight - 1]);
                        SwitchTiles(tiles2x2[3], tiles[tiles2x2[3].X, grid.gameHeight - 1]);

                        tileGravity.RunCheck();
                    }
                    break;
            }

            sfxSource.PlayOneShot(tileActionClip);

            if (!optionSelected.Equals(Options.SwitchColorOfOne) && !optionSelected.Equals(Options.SwitchColorOfTwo) && !optionSelected.Equals(Options.SwitchColorOfThree) && !optionSelected.Equals(Options.TranslateOneTile) && !optionSelected.Equals(Options.SwitchAdjacentRows) && !optionSelected.Equals(Options.SwitchAdjacentColumns) && !optionSelected.Equals(Options.SwitchAdjacent2x2) && !optionSelected.Equals(Options.Move3ToTop))
            {
                AfterTurnChecks();
            }
        }
    }

    public void SetOption(int opt)
    {
        Options option = (Options)opt;

        optionSelected = option;

        switch (optionSelected)
        {
            case Options.Destroy:
            case Options.DestroyWithColors:
                currentSelectionMode = SelectionMode.Single;
                break;
            case Options.Rotate3x3Left90Degrees:
            case Options.Rotate3x3Right90Degrees:
            case Options.Rotate3x3Right180Degrees:
            case Options.Rotate3x3Left180Degrees:
            case Options.HorizontalFlip3x3:
            case Options.VerticalFlip3x3:
                currentSelectionMode = SelectionMode.ThreeByThree;
                break;
            case Options.ThreeByThreeSwitch:
            case Options.TranslateOneTile:
            case Options.SwitchAdjacentRows:
            case Options.SwitchAdjacentColumns:
            case Options.SwitchColorOfTwo:
            case Options.SwitchColorOfThree:
            case Options.Move3ToTop:
                currentSelectionMode = SelectionMode.SaveSelection;
                savedTiles.Clear();
                SelectedTilesGroupOne.Clear();
                SelectedTilesGroupTwo.Clear();
                break;
            case Options.SwitchColorOfOne:
                currentSelectionMode = SelectionMode.Single;
                break;
            case Options.HorizontalFlip2x2:
            case Options.VerticalFlip2x2:
            case Options.Rotate2x2Left90Degrees:
            case Options.Rotate2x2Left180Degrees:
            case Options.SwitchAdjacent2x2:
            case Options.Move2x2ToTop:
                currentSelectionMode = SelectionMode.TwoByTwo;
                break;
        }
    }

    private void AfterTurnChecks()
    {
        Card.DeSelectAll();
        Card.CanSelect(true);

        musicManager.CheckNextSong();

        amountOfTurns++;
        CheckAmountOfTurns();

        RedrawTilesFromLocal();
    }

    public void SwitchColorOfTiles(Sprite newColor)
    {
        foreach (Tile tile in savedTiles)
        {
            tile.SetSprite(newColor);
            tile.setSelect(false);
        }

        savedTiles.Clear();

        CanSelectTile = true;

        AfterTurnChecks();
    }

    public void FlipBoard(FlipOptions flipOption)
    {
        //gravityQueue.Clear();
        if (flipOption.Equals(FlipOptions.Horizontal))
        {
            for (int column = 0; column < Mathf.FloorToInt(grid.gameWidth / 2); column++)
            {
                SwitchColumnOfTiles(column, grid.gameWidth - column - 1);
            }
        }
        else
        {
            int amountOfFullRows = AmountOfFullRows();

            for (int row = 0; row < Mathf.FloorToInt(grid.gameHeight / 2); row++)
            {
                SwitchRowOfTiles(row, grid.gameHeight - row - (grid.gameHeight - amountOfFullRows) - 1);
            }
        }


        tileGravity.RunCheckDelayed(0.5f);
        AfterTurnChecks();
    }

    public void ThreeByThreeSwitch(int x, int y)
    {
        if (SelectedTilesGroupOne.Count > 0)
        {
            Tile[] tilesToSwitchTo = GetTilesIn3x3(tiles[x, y]);

            foreach (Tile tile in tilesToSwitchTo)
            {
                if (SelectedTilesGroupOne.Contains(tile)) return;
            }

            //Add check for edge TODO
            SwitchTiles(SelectedTilesGroupOne.ToArray(), tilesToSwitchTo);
            SelectTiles(SelectedTilesGroupOne.ToArray(), false);
            SelectTiles(tilesToSwitchTo, false);
            SelectedTilesGroupOne.Clear();
        }
        else
        {
            SelectedTilesGroupOne.AddRange(GetTilesIn3x3(tiles[x, y]));
            SelectTiles(SelectedTilesGroupOne.ToArray(), true);
        }
    }

    public void SwitchEdgeRows()
    {
        SwitchRowOfTiles(0, AmountOfFullRows() - 1);

        tileGravity.RunCheckDelayed(0.5f);
        AfterTurnChecks();
    }

    public void SwitchEdgeColumns()
    {
        SwitchColumnOfTiles(0, grid.gameWidth - 1);

        tileGravity.RunCheckDelayed(0.5f);
        AfterTurnChecks();
    }

    private int AmountOfFullRows()
    {
        int amountOfFullRows = 0;

        for (int y = 0; y < grid.gameHeight; y++)
        {
            for (int x = 0; x < grid.gameWidth; x++)
            {
                if (!tiles[x, y].isDead)
                {
                    amountOfFullRows++;
                    break;
                }
            }
        }

        return amountOfFullRows;
    }

    private void CheckAmountOfTurns()
    {
        SetOption((int)Options.DestroyWithColors);
        cardManager.PlayCardsAnimation();

        movesUntilNextRow.text = (amountOfTurnsToAddRow - amountOfTurns % amountOfTurnsToAddRow).ToString();

        if (amountOfTurns % amountOfTurnsToAddRow == 0)
        {
            AddRowOfTiles();
        }
    }

    private void AddRowCounter()
    {
        addRowCounter++;

        if (addRowCounter >= 10)
        {
            DecreaseTimeBetweenAddRow();
            addRowCounter = 0;
        }
    }

    public void DecreaseTimeBetweenAddRow()
    {
        if (amountOfTurnsToAddRow > 2) amountOfTurnsToAddRow--;
    }

    private void SwitchTiles(Tile[] group1, Tile[] group2)
    {
        StartCoroutine(SwtichTilesCoroutine(group1, group2));
    }

    private IEnumerator SwtichTilesCoroutine(Tile[] group1, Tile[] group2)
    {
        if (group1.Length != group2.Length)
        {
            yield return null;
        }

        List<Tile> group1Top = new List<Tile>();
        int group1TopY = 0;
        for (int i = 0; i < group1.Length; i++)
        {
            if (group1[i].Y > group1TopY)
            {
                group1TopY = group1[i].Y;
            }
        }
        for (int i = 0; i < group1.Length; i++)
        {
            if (group1[i].Y == group1TopY)
            {
                group1Top.Add(group1[i]);
            }
        }

        List<Tile> group2Top = new List<Tile>();
        int group2TopY = 0;
        for (int i = 0; i < group2.Length; i++)
        {
            if (group2[i].Y > group2TopY)
            {
                group2TopY = group2[i].Y;
            }
        }
        for (int i = 0; i < group2.Length; i++)
        {
            if (group2[i].Y == group2TopY)
            {
                group2Top.Add(group2[i]);
            }
        }

        for (int i = 0; i < group1.Length; i++)
        {
            //Tile tile = tiles[group2[i].X, group2[i].Y];
            try
            {
                tiles[group2[i].X, group2[i].Y] = group1[i];
                tiles[group1[i].X, group1[i].Y] = group2[i];
            }
            catch
            {
                continue;
            }
        }

        //for (int i = 0; i < grid.gameHeight; i++)
        //{
        //    try
        //    {
        //        gravityQueue.Add(tiles[group1[0].X, i]);
        //        gravityQueue.Add(tiles[group1[1].X, i]);
        //        gravityQueue.Add(tiles[group1[2].X, i]);

        //        gravityQueue.Add(tiles[group2[0].X, i]);
        //        gravityQueue.Add(tiles[group2[1].X, i]);
        //        gravityQueue.Add(tiles[group2[2].X, i]);
        //    }
        //    catch
        //    {
        //        continue;
        //    }
        //}

        RedrawTilesFromLocal();

        yield return new WaitForSeconds(0.2f);

        //CheckForGravity();
        tileGravity.RunCheck();
    }

    private void SelectTiles(Tile[] tilesToSelect, bool state)
    {
        foreach (Tile t in tilesToSelect)
        {
            t.setHover(false);
            t.setSelect(state);
        }
    }
    /// <summary>
    /// Sets the option.
    /// Used from Unity UI Buttons
    /// </summary>
    /// <param name="opt">Opt.</param>

    public void SetColorOfSwitch(Sprite color)
    {
        colorOfSwitch = color;
    }
    /// <summary>
    /// Queue of objects that need to be destroyed
    /// </summary>
    public List<Tile> destructionQueue = new List<Tile>();
    /// <summary>
    /// Queue of objects that need to be checked for gravity updates.
    /// </summary>
    //public List<Tile> gravityQueue = new List<Tile>();

    /// <summary>
    /// Metheod called by invoke
    /// </summary>
    //public void GravityInvoke()
    //{
    //    CheckForGravity();
    //}

    //private void CheckForGravity(int count = 0)
    //{
    //    foreach (Tile tile in gravityQueue.ToArray())
    //    {
    //        if (tile.Y + 1 < tiles.GetLength(1) && !tiles[tile.X, tile.Y + 1].isDead && !gravityQueue.Contains(tiles[tile.X, tile.Y + 1]))
    //        {
    //            gravityQueue.Add(tiles[tile.X, tile.Y + 1]);//NOTE this line is adding multiple tiems @ preformance if needed
    //        }

    //        if (tile.Y != 0)
    //        {

    //            for (int scalingY = tile.Y; tiles[tile.X, scalingY - 1].isDead; scalingY--)
    //            {

    //                Tile temp = tiles[tile.X, scalingY];
    //                tiles[tile.X, scalingY] = tiles[tile.X, scalingY - 1];
    //                tiles[tile.X, scalingY - 1] = temp;
    //                tiles[tile.X, scalingY].isFalling = true;
    //                tiles[tile.X, scalingY - 1].isFalling = true;
    //                //print(scalingY + " if next one is error it must be " + tile.X + " or " + (scalingY-1));

    //            }



    //        }
    //        if (count >= 3)
    //        {
    //            gravityQueue.Remove(tile);
    //        }
    //    }

    //    if (count <= 3)
    //    {
    //        CheckForGravity(count + 1);
    //    }
    //    else
    //    {
    //        RedrawTilesFromLocal();
    //    }
    //}

    private void DestroyAllTilesOfSameColorAround(int x, int y)
    {
        CheckNearbyTileColors(x, y);
        ApplyDestructionQueue();
    }

    private void ApplyDestructionQueue()
    {
        scoreManager.AddScore((int)Mathf.Pow(destructionQueue.Count, 2));

        foreach (Tile t in destructionQueue.ToArray())
        {
            DestroyTile(t.X, t.Y, false);
            destructionQueue.Remove(t);
        }
    }

    private void CheckNearbyTileColors(int x, int y)
    {
        if (!tiles[x, y].isDead)
        {
            Sprite sprite = tiles[x, y].sprite;
            destructionQueue.Add(tiles[x, y]);
            if (x + 1 < tiles.GetLength(0) && tiles[x + 1, y].sprite == sprite && destructionQueue.IndexOf(tiles[x + 1, y]) == -1)
            {
                //right
                CheckNearbyTileColors(x + 1, y);
            }
            if (x - 1 >= 0 && tiles[x - 1, y].sprite == sprite && destructionQueue.IndexOf(tiles[x - 1, y]) == -1)
            {
                //left
                CheckNearbyTileColors(x - 1, y);
            }
            if (y + 1 < tiles.GetLength(1) && tiles[x, y + 1].sprite == sprite && destructionQueue.IndexOf(tiles[x, y + 1]) == -1)
            {
                //top
                CheckNearbyTileColors(x, y + 1);
            }
            if (y - 1 >= 0 && tiles[x, y - 1].sprite == sprite && destructionQueue.IndexOf(tiles[x, y - 1]) == -1)
            {
                //bottom
                CheckNearbyTileColors(x, y - 1);
            }
        }
    }

    private void DestroyTile(int x, int y, bool addScore)
    {
        if (!tiles[x, y].isDead)
        {

            Destroy(tiles[x, y].gameObject);
            //tiles[x, y].setIsDead();

            //THIS IS SO ALL BLOCK ABOVE FALL DOWN
            for (int scalingY = 0; scalingY < tiles.GetLength(1) - y - 1; scalingY++)
            {
                Tile empty = Instantiate(grid.tilePrefab, board).GetComponent<Tile>();
                empty.setIsDead();
                tiles[x, y + scalingY] = tiles[x, y + 1 + scalingY];
                tiles[x, y + 1 + scalingY] = empty;
            }

            if (addScore) scoreManager.AddScore(1);
            RedrawTilesFromLocal();
        }
    }

    /// <summary>
    /// Handles the tile mouse over. 
    /// Called form tile
    /// </summary>
    /// <param name="tile">Tile.</param>
    public void HandleTileMouseOver(Tile tile)
    {
        if (CanSelectTile && isPlaying)
        {
            switch (currentSelectionMode)
            {
                case SelectionMode.Single:
                    tile.setHover(true);
                    break;
                case SelectionMode.ThreeByThree:
                    foreach (Tile t in GetTilesIn3x3(tile))
                    {
                        t.setHover(true);
                    }
                    break;
                case SelectionMode.TwoByTwo:
                    foreach (Tile t in GetTilesIn2x2(tile))
                    {
                        t.setHover(true);
                    }
                    break;
                case SelectionMode.SaveSelection:
                    if (optionSelected.Equals(Options.ThreeByThreeSwitch))
                    {
                        foreach (Tile t in GetTilesIn3x3(tile))
                        {
                            t.setHover(true);
                        }
                    }
                    else if (optionSelected.Equals(Options.TranslateOneTile))
                    {
                        tile.setHover(true);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Handles the tile mouse exit.
    /// </summary>
    /// <param name="tile">Tile.</param>
    public void HandleTileMouseExit(Tile tile)
    {
        switch (currentSelectionMode)
        {
            case SelectionMode.Single:
                tile.setHover(false);
                break;
            case SelectionMode.ThreeByThree:
                foreach (Tile t in GetTilesIn3x3(tile))
                {
                    t.setHover(false);
                }
                break;
            case SelectionMode.TwoByTwo:
                foreach (Tile t in GetTilesIn2x2(tile))
                {
                    t.setHover(false);
                }
                break;
            case SelectionMode.SaveSelection:
                if (!SelectedTilesGroupOne.Contains(tile))
                {
                    foreach (Tile t in GetTilesIn3x3(tile))
                    {
                        if (!SelectedTilesGroupOne.Contains(t))
                        {
                            t.setHover(false);
                        }
                    }
                    //tile.setSelect(false);
                }
                break;
        }
    }

    private Tile[] GetTilesIn2x2(Tile tile)
    {
        if (TwoByTwoPossible(tile))
        {
            return new Tile[]
            {
                tile,
                tiles[tile.X + 1, tile.Y],
                tiles[tile.X, tile.Y + 1],
                tiles[tile.X + 1, tile.Y + 1]
            };
        }
        else return new Tile[] { tile };
    }

    private Tile[] GetTilesIn3x3(Tile tile)
    {
        if (TileIsOnEdge(tile))
        {
            return new Tile[] { tile };
        }

        return new Tile[]{
            tiles[tile.X-1,tile.Y+1], tiles[tile.X,tile.Y+1], tiles[tile.X+1,tile.Y+1],
            tiles[tile.X-1,tile.Y  ], tiles[tile.X,tile.Y  ], tiles[tile.X+1,tile.Y  ],
            tiles[tile.X-1,tile.Y-1], tiles[tile.X,tile.Y-1], tiles[tile.X+1,tile.Y-1]
        };
    }

    public bool TwoByTwoPossible(Tile tile)
    {
        return !(tile.X >= grid.gameWidth - 1 || tile.Y >= AmountOfFullRows() - 1);
    }

    public bool TileIsOnEdge(Tile t)
    {
        return (t.X - 1 < 0 || t.X + 1 > tiles.GetLength(0) - 1 || t.Y - 1 < 0 || t.Y + 1 > tiles.GetLength(1) - 1);
    }

    public void SwitchRowOfTiles(int rowNumber1, int rowNumber2)
    {
        //print("Switching row " + rowNumber2 + " with row " + rowNumber1);

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            Tile tile1 = tiles[i, rowNumber1];
            Tile tile2 = tiles[i, rowNumber2];
            //if (tile1.isDead || tile2.isDead)
            //{

            //}
            SwitchTiles(tile1, tile2);
        }

        RedrawTilesFromLocal();
    }

    public void SwitchColumnOfTiles(int columnNumber1, int columnNumber2)
    {
        //print("Switching column " + columnNumber2 + " with column " + columnNumber1);

        for (int i = 0; i < tiles.GetLength(1); i++)
        {
            Tile tile1 = tiles[columnNumber1, i];
            Tile tile2 = tiles[columnNumber2, i];
            //if(tile1.isDead || tile2.isDead){

            //}

            SwitchTiles(tile1, tile2);
        }

        RedrawTilesFromLocal();
    }

    public void SwitchTiles(Tile tile1, Tile tile2)
    {
        int t1x = tile1.X;
        int t1y = tile1.Y;

        int t2x = tile2.X;
        int t2y = tile2.Y;

        tile1.X = t2x;
        tile1.Y = t2y;

        tile2.X = t1x;
        tile2.Y = t1y;

        tiles[t1x, t1y] = tile2;
        tiles[t2x, t2y] = tile1;
    }

    public void AddRowOfTiles()
    {
        CheckForIsLastRowFilledAndDeleteDeadTiles();

        for (int row = tiles.GetLength(1) - 1; row > 0; row--)
        {
            SwitchRowOfTiles(row, row - 1);
        }

        grid.SwitchTilesFromLoadingAreaToLastRow();
        grid.fillTileLoadingArea();

        RedrawTilesFromLocal();
    }

    private bool CheckForIsLastRowFilledAndDeleteDeadTiles()
    {
        bool isDead = false;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            Tile t = tiles[i, tiles.GetLength(1) - 1];
            if (!t.isDead)
            {
                musicManager.QueueOutro();

                print("PLAYER DEAD");
                isDead = true;
                Destroy(t);
                t.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                Destroy(t);
            }
        }
        return isDead;
    }

    public bool CheckIfTense()
    {
        return AmountOfFullRows() >= Mathf.FloorToInt(0.75f * grid.gameHeight);
    }
}
