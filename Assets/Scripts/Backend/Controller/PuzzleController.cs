using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class Puzzle
{
    public int puzzleId;
    public string type;
    public bool solved;
    // Add other fields as needed
}

public class PuzzleController : MonoBehaviour
{
    private string apiUrl = "http://localhost:8080/api/puzzles";

    public IEnumerator GetAllPuzzles(System.Action<List<Puzzle>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"puzzles\":" + www.downloadHandler.text + "}";
                PuzzleListWrapper wrapper = JsonUtility.FromJson<PuzzleListWrapper>(json);
                callback(wrapper.puzzles);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator GetPuzzleById(int id, System.Action<Puzzle> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Puzzle puzzle = JsonUtility.FromJson<Puzzle>(www.downloadHandler.text);
                callback(puzzle);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator CreatePuzzle(Puzzle puzzle, System.Action<Puzzle> callback)
    {
        string jsonData = JsonUtility.ToJson(puzzle);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Puzzle created = JsonUtility.FromJson<Puzzle>(www.downloadHandler.text);
                callback(created);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator UpdatePuzzle(int id, Puzzle puzzle, System.Action<Puzzle> callback)
    {
        string jsonData = JsonUtility.ToJson(puzzle);
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/{id}", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Puzzle updated = JsonUtility.FromJson<Puzzle>(www.downloadHandler.text);
                callback(updated);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator DeletePuzzle(int id, System.Action<bool> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                callback(true);
            }
            else
            {
                Debug.LogError(www.error);
                callback(false);
            }
        }
    }

    [System.Serializable]
    private class PuzzleListWrapper
    {
        public List<Puzzle> puzzles;
    }
}