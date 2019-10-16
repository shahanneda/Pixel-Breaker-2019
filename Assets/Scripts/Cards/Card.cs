using UnityEngine;
using UnityEngine.UI;

using System.Collections;
/// <summary>
/// All cards should inherit from this class which sets up the click animation.
/// </summary>
public class Card : MonoBehaviour
{
    public GlobalEnums.Options option;

    public bool canSelect = true;
    private bool isSelected = false;

    public AudioClip selectClip;

    public GameObject cardtext;

    protected Animator animator;
    protected TileManager tileManager;

    private GameObject infoPanel;

    private GameManager gameManager;

    private AudioSource sfxSource;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        tileManager = FindObjectOfType<TileManager>();
        gameManager = FindObjectOfType<GameManager>();
        GetComponent<Button>().onClick.AddListener(Clicked);

        infoPanel = transform.Find("Info Panel").gameObject;

        sfxSource = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();

        cardtext = transform.GetChild(1).gameObject;
        cardtext.SetActive(gameManager.settingsFile.showCardText);
    }

    private void Clicked()
    {
        if (tileManager.isPlaying && (!isSelected))//Remove canselect so able tp select
        {
            Card.DeSelectAll();
            Card.CanSelect(false);

            SetSelect(true);
            SetOption();

            sfxSource.PlayOneShot(selectClip);
        }else if (isSelected){
            SetSelect(false);
            Card.DeSelectAll();
            tileManager.SetOption((int)GlobalEnums.Options.DestroyWithColors);
            tileManager.DeSelectAllTilesInSelectionBuffer();
            tileManager.selectCardColorMenu.SetActive(false);
        }
    }

    public void SetSelect(bool state)
    {
        isSelected = state;
        animator.SetBool("isSelected", state);
    }

    public void ToggleInfoPanel()
    {
        infoPanel.SetActive(!infoPanel.activeInHierarchy);
    }

    public static void DeSelectAll()
    {
        foreach (Card c in FindObjectsOfType<Card>())
        {
            c.SetSelect(false);
        }
    }

    public static void CanSelect(bool canSelect)
    {
        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.canSelect = canSelect;
        }
    }

    public virtual void SetOption()
    {
        tileManager.SetOption((int)option);
    }
}
