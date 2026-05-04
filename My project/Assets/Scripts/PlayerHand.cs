using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public string playerName;
    
    
    public List<Card> cards = new List<Card>();
    public List<Card> shadowDeck = new List<Card>();

    
    public bool skipNextTurn = false;

    // Tracks how many cards are revealed by failed bluff callouts
    [HideInInspector] public int shadowCardsToReveal = 0;
}