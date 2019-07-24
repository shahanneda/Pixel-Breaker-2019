using UnityEngine;
using UnityEngine.UI;

using System.Collections;
/// <summary>
/// All cards should inherit from this class which sets up the click animation.
/// </summary>
public class Card : MonoBehaviour
{

    protected Animator animator;
    protected TileManager tileManager;

    public virtual void Start(){
        animator = GetComponent<Animator>();
        tileManager = FindObjectOfType<TileManager>(); 
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    public void Clicked(){
        Card.DeSelectAll();
        SetSelect(true);
    }

    public void SetSelect(bool state){
        animator.SetBool("isSelected", state);
    }

    public static void DeSelectAll(){
        foreach(Card c in FindObjectsOfType<Card>()){
            c.SetSelect(false);
        }
    }
}
