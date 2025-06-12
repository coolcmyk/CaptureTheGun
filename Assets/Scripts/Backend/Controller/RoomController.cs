using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class Room
{
    public int roomId;
    public string description;
    // Add other fields as needed
}

public class RoomController : MonoBehaviour
{
    private string apiUrl = "http://localhost:8080/api/rooms";

    public IEnumerator GetAllRooms(System.Action<List<Room>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"rooms\":" + www.downloadHandler.text + "}";
                RoomListWrapper wrapper = JsonUtility.FromJson<RoomListWrapper>(json);
                callback(wrapper.rooms);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator GetRoomById(int id, System.Action<Room> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Room room = JsonUtility.FromJson<Room>(www.downloadHandler.text);
                callback(room);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator CreateRoom(Room room, System.Action<Room> callback)
    {
        string jsonData = JsonUtility.ToJson(room);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Room created = JsonUtility.FromJson<Room>(www.downloadHandler.text);
                callback(created);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator UpdateRoom(int id, Room room, System.Action<Room> callback)
    {
        string jsonData = JsonUtility.ToJson(room);
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/{id}", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Room updated = JsonUtility.FromJson<Room>(www.downloadHandler.text);
                callback(updated);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator DeleteRoom(int id, System.Action<bool> callback)
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
    private class RoomListWrapper
    {
        public List<Room> rooms;
    }
}