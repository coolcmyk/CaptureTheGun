using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ClueFetcher : MonoBehaviour
{
    [Header("0 = CLUE0, 1 = CLUE1, 2 = CLUE2")]
    [SerializeField] private int clueIndex = 0;

    [SerializeField] private string apiBaseUrl = "http://localhost:8080/api/flags/";

    [SerializeField] private TMP_Text outputText;

    [ContextMenu("Fetch Clue")]
    public void FetchClue()
    {
        if (outputText != null)
            outputText.text = "Fetching clue...";
        StartCoroutine(FetchClueCoroutine());
    }

    private IEnumerator FetchClueCoroutine()
    {
        string clueEndpoint = $"CLUE{clueIndex}";
        string url = apiBaseUrl + clueEndpoint;

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                if (outputText != null)
                    outputText.text = "Error: " + www.error;
            }
            else
            {
                if (outputText != null)
                    outputText.text = www.downloadHandler.text;
            }
        }
    }
}