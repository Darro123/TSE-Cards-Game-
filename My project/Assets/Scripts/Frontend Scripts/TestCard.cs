using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : MonoBehaviour
{
    public Card card;
    public GameObject player1;
    public GameObject player2;

    void Start()
    {
        card.UseAbility(player1, player2);
    }
}
