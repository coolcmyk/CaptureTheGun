using UnityEngine;
using System.Collections.Generic;

public class FlagManager : MonoBehaviour
{
    public FlagController flagController;

    void Start()
    {
        if (flagController == null)
            flagController = FindObjectOfType<FlagController>();

        StartCoroutine(flagController.GetAllFlags(OnFlagsReceived));
    }

    void OnFlagsReceived(List<Flag> flags)
    {
        foreach (var f in flags)
            Debug.Log("Flag: " + f.form3D);
    }
}