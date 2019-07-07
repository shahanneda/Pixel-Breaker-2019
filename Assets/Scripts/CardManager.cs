using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //NOTE: current cards avaliable are placeholders and are not meant to be in the positions they are currently at.

    public Animator cardsAnimator;

    public GameObject[] rotationCards;
    public GameObject[] translationCards;
    public GameObject[] conversionCards;

    private GameObject[] previousCards = new GameObject[3];

    private void Start()
    {
        PickCards();
    }

    private void ReplacePreviousCard(int index, GameObject newCard)
    {
        if (previousCards[index] != null) previousCards[index].SetActive(false);
        previousCards[index] = newCard;
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

        if (previousCards[0] != null)
        {
            while (previousCards[0].Equals(newRotationCard))
            {
                newRotationCard = rotationCards[Random.Range(0, rotationCards.Length)];
            }
        }

        ReplacePreviousCard(0, newRotationCard);
        newRotationCard.SetActive(true);
        #endregion

        #region New Translation Card
        GameObject newTranslationCard = translationCards[Random.Range(0, translationCards.Length)];

        if (previousCards[1] != null)
        {
            while (previousCards[1].Equals(newTranslationCard))
            {
                newTranslationCard = translationCards[Random.Range(0, translationCards.Length)];
            }
        }

        ReplacePreviousCard(1, newTranslationCard);
        newTranslationCard.SetActive(true);
        #endregion

        #region New Conversion Card
        GameObject newConversionCard = conversionCards[Random.Range(0, conversionCards.Length)];

        if (previousCards[2] != null)
        {
            while (previousCards[2].Equals(newConversionCard))
            {
                newConversionCard = conversionCards[Random.Range(0, conversionCards.Length)];
            }
        }

        ReplacePreviousCard(2, newConversionCard);
        newConversionCard.SetActive(true);
        #endregion
    }
}
