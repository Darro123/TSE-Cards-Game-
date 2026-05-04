using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelection : MonoBehaviour
{
    public static UISelection Instance;

    public Card selectedCard;
    public GameObject selectedTarget;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectCard(Card card)
    {
        selectedCard = card;
        Debug.Log("Selected card: " + card.cardName);
    }

    public void SelectTarget(GameObject target)
    {
        selectedTarget = target;
        Debug.Log("Selected target: " + target.name);
    }
}