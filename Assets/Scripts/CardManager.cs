using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject[] rotationCards;
    public GameObject[] translationCards;
    public GameObject[] conversionCards;

    public RectTransform cardsContainer;

    private int previousRotationIndex = 0;
    private int previousTranslationIndex = 0;
    private int previousConversionIndex = 0;

    public Transform cancelButton;

    private bool inAnim = true;// we want to reset initial positions
    private bool animComingBack = false;
    private float cardAnimationLimit;
    private float cardAnimationSpeed = 0f;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();

        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.gameObject.SetActive(false);
        }

        cardAnimationLimit = Screen.width / 8f;
        cardAnimationSpeed = 9f * Screen.width / 32f + 80f;

        PickCards();
        PlayCardsAnimation();
    }

    private void Update()
    {
        if (inAnim)
        {
            if (cardsContainer.anchoredPosition.x <= 0)
            {
                inAnim = false;
                animComingBack = false;
            }
            else if (animComingBack)
            {
                cardsContainer.anchoredPosition -= new Vector2(cardAnimationSpeed * Time.deltaTime, 0);
            }
            else
            {
                if (cardsContainer.anchoredPosition.x <= cardAnimationLimit)
                {
                    print("reseting");
                    cardsContainer.anchoredPosition += new Vector2(cardAnimationSpeed * Time.deltaTime, 0);
                }

                else
                    animComingBack = true;
            }
        }
    }

    private IEnumerator CardsAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.35f);
        PickCards();
    }

    public void PlayCardsAnimation()
    {
        StartCoroutine(CardsAnimationCoroutine());

        cardsContainer.anchoredPosition = new Vector2(0.1f, 0);
        inAnim = true;
    }

    public void PickCards()
    {
        print("picking cards");
        #region New Rotation Card
        GameObject newRotationCard = rotationCards[Random.Range(0, rotationCards.Length)];

        rotationCards[previousRotationIndex].SetActive(false);
        translationCards[previousTranslationIndex].SetActive(false);
        conversionCards[previousConversionIndex].SetActive(false);

        previousRotationIndex = GetNewCardIndex(rotationCards.Length - 1, previousRotationIndex);
        previousTranslationIndex = GetNewCardIndex(translationCards.Length - 1, previousTranslationIndex);
        previousConversionIndex = GetNewCardIndex(conversionCards.Length - 1, previousConversionIndex);

        rotationCards[previousRotationIndex].SetActive(true);
        translationCards[previousTranslationIndex].SetActive(true);
        conversionCards[previousConversionIndex].SetActive(true);
        #endregion 
    }

    public void CancelCard()
    {
        tileManager.SetOption((int)GlobalEnums.Options.DestroyWithColors);
        tileManager.DeSelectAllTilesInSelectionBuffer();
        tileManager.selectCardColorMenu.SetActive(false);
        tileManager.CanSelectTile = true;

        Card.DeSelectAll();
        Card.CanSelect(true);
    }

    private int GetNewCardIndex(int maxIndex, int prevIndex)
    {
        int newIndex = prevIndex;

        while (newIndex == prevIndex)
        {
            newIndex = Random.Range(0, maxIndex);
        }

        return newIndex;
    }
}
