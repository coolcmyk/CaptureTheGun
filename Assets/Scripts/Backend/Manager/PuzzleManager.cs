using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleController puzzleController;

    void Start()
    {
        if (puzzleController == null)
            puzzleController = FindObjectOfType<PuzzleController>();

        StartCoroutine(puzzleController.GetAllPuzzles(OnPuzzlesReceived));
    }

    void OnPuzzlesReceived(List<Puzzle> puzzles)
    {
        foreach (var p in puzzles)
            Debug.Log("Puzzle: " + p.type);
    }
}