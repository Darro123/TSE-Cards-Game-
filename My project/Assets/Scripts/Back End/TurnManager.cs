using UnityEngine;
using System.Collections.Generic;

public enum TurnPhase { StartOfTurn, AwaitingAction, AwaitingCallout, ResolvingAction, EndOfTurn }

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public GameObject[] players; // Assign your Player GameObjects here
    public int currentPlayerIndex = 0;
    public TurnPhase currentPhase = TurnPhase.StartOfTurn;

    private PlayerHand[] playerHands;

    // Temporary storage for a card played face down
    private Card cardActuallyPlayed;
    private string claimedAbility;
    private bool isBluffing;
    private GameObject currentTarget;

    private void Awake() => Instance = this;

    private void Start()
    {
        playerHands = new PlayerHand[players.Length];
        for (int i = 0; i < players.Length; i++)
            playerHands[i] = players[i].GetComponent<PlayerHand>();

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        Debug.Log("StartPlayerTurn" + currentPlayerIndex);


        currentPhase = TurnPhase.StartOfTurn;
        PlayerHand current = CurrentHand();

        // Check if SkipTurn ability was applied by a card
        if (current.skipNextTurn)
        {
            current.skipNextTurn = false;
            Debug.Log(current.playerName + " skipped!");
            EndTurn();
            return;
        }

        // Hide any cards that were revealed by Liar penalties last turn
        ResetPenaltyReveals(current);

        currentPhase = TurnPhase.AwaitingAction;
    }

    // Call this from a UI button to play a card face down
    public void PlayCardFaceDown(Card card, GameObject target, bool bluffing, string claim)
    {
        if (currentPhase != TurnPhase.AwaitingAction) return;

        cardActuallyPlayed = card;
        isBluffing = bluffing;
        currentTarget = target;
        claimedAbility = claim;

        currentPhase = TurnPhase.AwaitingCallout;
        Debug.Log(CurrentHand().playerName + " plays a card face down and claims: " + claim);
    }

    public void ResolveLiarCall(bool callMade, int callerIndex)
    {
        if (currentPhase != TurnPhase.AwaitingCallout) return;
        currentPhase = TurnPhase.ResolvingAction;

        if (callMade)
        {
            if (isBluffing)
            {
                // Liar caught Transfer card to the person who called them out
                Debug.Log("Liar Caught!");
                TransferCard(CurrentPlayer(), players[callerIndex], cardActuallyPlayed);
            }
            else
            {
                // Truth told Penalize caller and activate card
                Debug.Log("Truth told! Caller penalized.");
                ApplyPenalty(playerHands[callerIndex], 2);
                ExecuteCardLogic();
                CurrentHand().cards.Remove(cardActuallyPlayed);
            }
        }
        else
        {
            // No one called liar - ability happens
            ExecuteCardLogic();
            CurrentHand().cards.Remove(cardActuallyPlayed);
        }

        EndTurn();
    }

    private void ExecuteCardLogic()
    {
        // Temporarily set the card's ability to the CLAIMED one
        string realAbility = cardActuallyPlayed.abilityType;
        cardActuallyPlayed.abilityType = claimedAbility;

        // Calls card.cs
        cardActuallyPlayed.UseAbility(CurrentPlayer(), currentTarget);

        // Reset the card's real ability type
        cardActuallyPlayed.abilityType = realAbility;
    }

    public void EndTurn()
    {

        Debug.Log("Player" + currentPlayerIndex + "'s has ended");
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        StartPlayerTurn();
    }

    // Helper to transfer cards (used when catching a liar)
    private void TransferCard(GameObject from, GameObject to, Card c)
    {
        from.GetComponent<PlayerHand>().cards.Remove(c);
        to.GetComponent<PlayerHand>().cards.Add(c);
    }

    // Penalty for failed liar calls: Reveal shadow cards
    private void ApplyPenalty(PlayerHand hand, int count)
    {
        hand.shadowCardsToReveal = count;
        
    }

    private void ResetPenaltyReveals(PlayerHand hand)
    {
        hand.shadowCardsToReveal = 0;
    }

    public GameObject CurrentPlayer() => players[currentPlayerIndex];
    public PlayerHand CurrentHand() => playerHands[currentPlayerIndex];
}
