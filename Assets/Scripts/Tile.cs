﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static TileManager manager;

    [HideInInspector] public int X, Y;

    public static float speed = 5f;

    public bool inAnimation = false;
    public bool isDead = false;
    private bool isInLoadingArea = false;

    public bool getIsLoadingArea()
    {
        return isInLoadingArea;
    }

    public void setIsInLoadingArea(bool value)
    {
        isInLoadingArea = value;
    }

    [HideInInspector] public Vector2 shouldMoveTo = Vector2.positiveInfinity;

    [HideInInspector] public Sprite sprite;

    private Animator anim;
    public SpriteRenderer spriteRenderer;

    private bool _isFalling;
    public bool isFalling
    {
        get
        {
            return _isFalling;
        }
        set
        {
            _isFalling = value;
        }
    }

    public void Awake()
    {
        if (!isDead)
        {
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        shouldMoveTo = transform.position;
    }

    public void Update()
    {
        if (inAnimation)
        {
            transform.position = Vector2.Lerp(transform.position, shouldMoveTo, speed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, shouldMoveTo) < 0.01f)
        {
            inAnimation = false;
            isFalling = false;
        }
    }

    public void ChooseRandomSprite()
    {
        sprite = manager.sprites[Random.Range(0, manager.sprites.Length)];
        spriteRenderer.sprite = sprite;
    }

    public void SetSprite(Sprite newSprite)
    {
        manager.removeTileFromColorGroup(this);

        sprite = newSprite;
        spriteRenderer.sprite = sprite;
        manager.addTileToColorGroup(this);
    }

    public void InitializeTile(Sprite sprite)
    {
        this.sprite = sprite;
        spriteRenderer.sprite = sprite;
    }

    //THIS CLASS IS HERE JUST FOR THE BUTTON CLICKS, BUT TILES THEMSELVES SHOULD NOT HANDLE ANY ACTIONS, ISNTEAD
    //THEY SHOULD REPORT THAT MESSAGE TO THEIR MANAGER!
    void OnMouseDown()
    {
        if (!isDead && !isInLoadingArea) StartCoroutine(MouseDownDelay());
    }

    private IEnumerator MouseDownDelay()
    {
        yield return new WaitForSeconds(0.01f); 
        manager.HandleTileClick(this);
    }

    public void OnMouseOver()
    {
        manager.HandleTileMouseOver(this);
    }

    public void OnMouseExit()
    {
        manager.HandleTileMouseExit(this);
    }

    public void setHover(bool state)
    {
        if (isDead || isInLoadingArea)
            return;
        //anim.SetBool("isHover", state);

        if (state && spriteRenderer.enabled && spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void setSelect(bool state)
    {
        if (!isDead || !isInLoadingArea)
        {
            anim.SetBool("Selected", state);
            print("Setting selection");
        }
        if (!state)
        {
            setHover(false);//This might be a bad idea, but it fixes the selection bug
        }
    }

    public bool getInSelect()
    {
        return !isDead && !isInLoadingArea && anim.GetBool("isHover");
    }

    public void setIsDead()
    {
        isDead = true;
        manager.removeTileFromColorGroup(this);
        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(GetComponent<BoxCollider2D>());
    }
    public void MarkForDebug()
    {
        spriteRenderer.color = Color.black;
    }
}
