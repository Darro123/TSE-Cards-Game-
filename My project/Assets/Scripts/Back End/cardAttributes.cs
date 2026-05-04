using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;
    public string abilityType; // "swap", "skip", "peek"
    public bool isFaceDown;

    // call this when a card is played, passing in who played it and who its aimed at
    public void UseAbility(GameObject source, GameObject target)
    {
        Debug.Log(cardName + " ability triggered: " + abilityType);

        if (abilityType == "swap")
            SwapHands(source, target);
        else if (abilityType == "skip")
            SkipTurn(target);
        else if (abilityType == "peek")
            PeekShadow(source);
        else if (abilityType == "shadowswap")
            ShadowSwap(source, 0, 0);
        else if (abilityType == "reveal")
            ForceReveal(target);
        else if (abilityType == "steal")
            StealCard(source, target);
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

        Debug.Log(source.name + " now has " + sourceHand.cards.Count + " cards");
        Debug.Log(target.name + " now has " + targetHand.cards.Count + " cards");

        foreach (Card c in sourceHand.cards)
            Debug.Log(source.name + " hand: " + c.cardName);

        foreach (Card c in targetHand.cards)
            Debug.Log(target.name + " hand: " + c.cardName);
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

    // force another player to reveal one of their cards
    void ForceReveal(GameObject target)
    {
        PlayerHand targetHand = target.GetComponent<PlayerHand>();

        if (targetHand == null)
        {
            targetHand = target.GetComponentInParent<PlayerHand>();
            
        }

        if (targetHand == null)
        {
            Debug.Log("couldnt find PlayerHand on target");
            return;
        }

        if (targetHand.cards.Count == 0)
        {
            Debug.Log(target.name + " has no cards to reveal");
            return;
        }

        // reveals their first card for now, UI will let the player choose later
        targetHand.cards[0].isFaceDown = false;
        Debug.Log(target.name + " was forced to reveal: " + targetHand.cards[0].cardName);
    }

    // steal a random card from another player's hand
    void StealCard(GameObject source, GameObject target)
    {
        PlayerHand sourceHand = source.GetComponent<PlayerHand>();
        PlayerHand targetHand = target.GetComponent<PlayerHand>();

        if (targetHand == null)
        {
            targetHand = target.GetComponentInParent<PlayerHand>();
            
            
        }

        if (sourceHand == null || targetHand == null)
        {
            Debug.Log("Couldnt find playerHand on one of the players");
            return;
        }

        if (targetHand.cards.Count == 0)
        {
            Debug.Log(target.name + " has no cards to steal");
            return;
        }

        // pick a random card from the targets hand
        int randomIndex = Random.Range(0, targetHand.cards.Count);
        Card stolenCard = targetHand.cards[randomIndex];

        // move it to the source player's hand
        targetHand.cards.RemoveAt(randomIndex);
        sourceHand.cards.Add(stolenCard);

        Debug.Log(source.name + " stole " + stolenCard.cardName + " from " + target.name);
    }

    // swap a card from your hand with one in your shadow deck
    void ShadowSwap(GameObject source, int handIndex, int shadowIndex)
    {
        PlayerHand sourceHand = source.GetComponent<PlayerHand>();

        if (sourceHand == null)
        {
            Debug.Log("couldnt find PlayerHand on source");
            return;
        }

        // make sure the indexes actually exist before we try swapping
        if (handIndex >= sourceHand.cards.Count || shadowIndex >= sourceHand.shadowDeck.Count)
        {
            Debug.Log("card index out of range");
            return;
        }

        // do the swap
        Card temp = sourceHand.cards[handIndex];
        sourceHand.cards[handIndex] = sourceHand.shadowDeck[shadowIndex];
        sourceHand.shadowDeck[shadowIndex] = temp;

        // the card coming out of the shadow deck is now visible to the player
        sourceHand.cards[handIndex].isFaceDown = false;

        Debug.Log(source.name + " swapped hand card " + handIndex + " with shadow card " + shadowIndex);
    }

}