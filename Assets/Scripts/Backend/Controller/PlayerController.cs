using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class Player
{
    public int playerId;
    public string name;
    public int suspicionLevel;
    // Add other fields as needed
}

public class PlayerController : MonoBehaviour
{
    private string apiUrl = "http://localhost:8080/api/players";

    public IEnumerator GetAllPlayers(System.Action<List<Player>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"players\":" + www.downloadHandler.text + "}";
                PlayerListWrapper wrapper = JsonUtility.FromJson<PlayerListWrapper>(json);
                callback(wrapper.players);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator GetPlayerById(int id, System.Action<Player> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Player player = JsonUtility.FromJson<Player>(www.downloadHandler.text);
                callback(player);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator CreatePlayer(Player player, System.Action<Player> callback)
    {
        string jsonData = JsonUtility.ToJson(player);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Player created = JsonUtility.FromJson<Player>(www.downloadHandler.text);
                callback(created);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator UpdatePlayer(int id, Player player, System.Action<Player> callback)
    {
        string jsonData = JsonUtility.ToJson(player);
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/{id}", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Player updated = JsonUtility.FromJson<Player>(www.downloadHandler.text);
                callback(updated);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator DeletePlayer(int id, System.Action<bool> callback)
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
    private class PlayerListWrapper
    {
        public List<Player> players;
    }
}