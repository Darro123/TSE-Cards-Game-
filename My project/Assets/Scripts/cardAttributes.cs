using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;
    public string abilityType; // "swap", "skip", "peek"

    // call this when a card is played, passing in who played it and who its aimed at
    public void UseAbility(GameObject source, GameObject target)
    {
        if (abilityType == "swap")
            SwapHands(source, target);
        else if (abilityType == "skip")
            SkipTurn(target);
        else if (abilityType == "peek")
            PeekShadow(source);
        else
            Debug.Log(cardName + " has no ability.");
    }

    // swap hands with another player
    void SwapHands(GameObject source, GameObject target)
    {
        PlayerHand sourceHand = source.GetComponent<PlayerHand>();
        PlayerHand targetHand = target.GetComponent<PlayerHand>();

        if (sourceHand == null || targetHand == null)
        {
            Debug.Log("couldnt find PlayerHand on one of the players");
            return;
        }

        List<Card> temp = sourceHand.cards;
        sourceHand.cards = targetHand.cards;
        targetHand.cards = temp;

        Debug.Log(source.name + " swapped hands with " + target.name);
    }

    // target player loses their next turn
    void SkipTurn(GameObject target)
    {
        PlayerHand targetHand = target.GetComponent<PlayerHand>();

        if (targetHand == null)
        {
            Debug.Log("couldnt find PlayerHand on target");
            return;
        }

        targetHand.skipNextTurn = true;
        Debug.Log(target.name + " will skip their next turn");
    }

    // lets the player look at their top shadow deck card
    void PeekShadow(GameObject source)
    {
        PlayerHand sourceHand = source.GetComponent<PlayerHand>();

        if (sourceHand == null)
        {
            Debug.Log("couldnt find PlayerHand on source");
            return;
        }

        if (sourceHand.shadowDeck.Count == 0)
        {
            Debug.Log(source.name + " has no shadow cards left");
            return;
        }

        // just reveals the top card for now, UI can hook into this later
        Card topCard = sourceHand.shadowDeck[0];
        Debug.Log(source.name + " peeked at their shadow card: " + topCard.cardName);
    }
}