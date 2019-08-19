using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Animator cardsAnimator;

    public GameObject[] rotationCards;
    public GameObject[] translationCards;
    public GameObject[] conversionCards;

    private GameObject previousRotationCard;
    private GameObject previousTranslationCard;
    private GameObject previousConversionCard;

    private TileManager tileManager;

    private void Start()
    {
        PickCards();

        tileManager = FindObjectOfType<TileManager>();
    }

    private IEnumerator CardsAnimationCoroutine()
    {
        cardsAnimator.enabled = false;
        cardsAnimator.enabled = true;

        cardsAnimator.Play("Switch Cards");

        yield return new WaitForSeconds(0.35f);

        PickCards();
    }

    public void PlayCardsAnimation()
    {
        StartCoroutine(CardsAnimationCoroutine());
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
