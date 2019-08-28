using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject[] rotationCards;
    public GameObject[] translationCards;
    public GameObject[] conversionCards;

    public RectTransform cardsContainer;

    private GameObject previousRotationCard;
    private GameObject previousTranslationCard;
    private GameObject previousConversionCard;

    public Transform cancelButton;

    private bool inAnim = false;
    private bool animComingBack = false;
    private float cardAnimationLimit;
    private float cardAnimationSpeed = 0f;

    private TileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();

        float cardScale = Screen.width / 1500f;
        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.transform.localScale = new Vector3(cardScale, cardScale, 1);
            card.gameObject.SetActive(false);
        }

        cancelButton.localScale = new Vector3(cardScale, cardScale, 1);
        cardAnimationLimit = Screen.width / 8f;
        cardAnimationSpeed = 9f * Screen.width / 32f + 80f;

        PickCards();
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
                    cardsContainer.anchoredPosition += new Vector2(cardAnimationSpeed * Time.deltaTime, 0);

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
        #region New Rotation Card
        GameObject newRotationCard = rotationCards[Random.Range(0, rotationCards.Length)];

        if (previousRotationCard != null)
        {
            while (previousRotationCard.Equals(newRotationCard))
            {
                newRotationCard = rotationCards[Random.Range(0, rotationCards.Length)];
            }
        }

        if (previousRotationCard != null) previousRotationCard.SetActive(false);
        previousRotationCard = newRotationCard;
        newRotationCard.SetActive(true);
        #endregion

        #region New Translation Card
        GameObject newTranslationCard = translationCards[Random.Range(0, translationCards.Length)];

        if (previousTranslationCard != null)
        {
            while (previousTranslationCard.Equals(newTranslationCard))
            {
                newTranslationCard = translationCards[Random.Range(0, translationCards.Length)];
            }
        }

        if (previousTranslationCard != null) previousTranslationCard.SetActive(false);
        previousTranslationCard = newTranslationCard;
        newTranslationCard.SetActive(true);
        #endregion

        #region New Conversion Card
        GameObject newConversionCard = conversionCards[Random.Range(0, conversionCards.Length)];

        if (previousConversionCard != null)
        {
            while (previousConversionCard.Equals(newConversionCard))
            {
                newConversionCard = conversionCards[Random.Range(0, conversionCards.Length)];
            }
        }

        if (previousConversionCard != null) previousConversionCard.SetActive(false);
        previousConversionCard = newConversionCard;
        newConversionCard.SetActive(true);
        #endregion
    }

    public void CancelCard()
    {
        tileManager.SetOption((int)GlobalEnums.Options.DestroyWithColors);
        Card.DeSelectAll();
        Card.CanSelect(true);
    }
}
