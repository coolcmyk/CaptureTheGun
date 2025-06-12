using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        StartCoroutine(playerController.GetAllPlayers(OnPlayersReceived));
    }

    void OnPlayersReceived(List<Player> players)
    {
        foreach (var p in players)
            Debug.Log("Player: " + p.name);
    }
}