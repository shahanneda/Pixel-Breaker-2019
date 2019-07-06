using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //NOTE: current cards avaliable are placeholders and are not meant to be in the positions they are currently at.

    public GameObject[] rotationCards;
    public GameObject[] translationCards;
    public GameObject[] conversionCards;

    [SerializeField] private List<GameObject> previousCards = new List<GameObject>();

    private void Start()
    {
        PickCards();
    }

    private void DisablePreviousCards()
    {
        foreach (GameObject card in previousCards)
        {
            card.SetActive(false);
        }

        previousCards.Clear();
    }

    public void PickCards()
    {
        DisablePreviousCards();

        #region New Rotation Card
        GameObject newRotationCard = rotationCards[Random.Range(0, rotationCards.Length)];

        while (previousCards.Contains(newRotationCard))
        {
            newRotationCard = rotationCards[Random.Range(0, rotationCards.Length)];
        }

        newRotationCard.SetActive(true);
        previousCards.Add(newRotationCard);
        #endregion

        #region New Translation Card
        GameObject newTranslationCard = translationCards[Random.Range(0, translationCards.Length)];

        while (previousCards.Contains(newTranslationCard))
        {
            newTranslationCard = translationCards[Random.Range(0, translationCards.Length)];
        }

        newTranslationCard.SetActive(true);
        previousCards.Add(newTranslationCard);
        #endregion

        #region New Conversion Card
        GameObject newConversionCard = conversionCards[Random.Range(0, conversionCards.Length)];

        while (previousCards.Contains(newConversionCard))
        {
            newConversionCard = conversionCards[Random.Range(0, conversionCards.Length)];
        }

        newConversionCard.SetActive(true);
        previousCards.Add(newConversionCard);
        #endregion
    }
}
