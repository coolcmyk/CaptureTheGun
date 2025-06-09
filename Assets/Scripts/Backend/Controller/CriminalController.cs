using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class Criminal
{
    public int criminalId;
    public string name; 
    public bool armed;
    public int suspicion;
}

public class CriminalController : MonoBehaviour
{
    private string apiUrl = "http://localhost:8080/api/criminals";
    public IEnumerator GetAllCriminals(System.Action<List<Criminal>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"criminals\":" + www.downloadHandler.text + "}";
                CriminalListWrapper wrapper = JsonUtility.FromJson<CriminalListWrapper>(json);
                callback(wrapper.criminals);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator GetCriminalById(int id, System.Action<Criminal> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Criminal criminal = JsonUtility.FromJson<Criminal>(www.downloadHandler.text);
                callback(criminal);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator CreateCriminal(Criminal criminal, System.Action<Criminal> callback)
    {
        string jsonData = JsonUtility.ToJson(criminal);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Criminal created = JsonUtility.FromJson<Criminal>(www.downloadHandler.text);
                callback(created);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator UpdateCriminal(int id, Criminal criminal, System.Action<Criminal> callback)
    {
        string jsonData = JsonUtility.ToJson(criminal);
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/{id}", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Criminal updated = JsonUtility.FromJson<Criminal>(www.downloadHandler.text);
                callback(updated);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator DeleteCriminal(int id, System.Action<bool> callback)
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
    private class CriminalListWrapper
    {
        public List<Criminal> criminals;
    }
}