using UnityEngine;
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
    }
    public void SetSelect(bool state){
        animator.SetBool("isSelected", state);
    }

}
