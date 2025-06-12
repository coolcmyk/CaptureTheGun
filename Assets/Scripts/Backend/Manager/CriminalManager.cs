using UnityEngine;
using System.Collections.Generic;

public class CriminalManager : MonoBehaviour
{
    public CriminalController criminalController;

    void Start()
    {
        if (criminalController == null)
            criminalController = FindObjectOfType<CriminalController>();

        StartCoroutine(criminalController.GetAllCriminals(OnCriminalsReceived));
    }

    void OnCriminalsReceived(List<Criminal> criminals)
    {
        foreach (var c in criminals)
            Debug.Log("Criminal: " + c.name);
    }
}