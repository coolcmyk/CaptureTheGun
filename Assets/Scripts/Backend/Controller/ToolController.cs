using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class Tool
{
    public int toolId;
    public string name;
    public string function;
    // Add other fields as needed
}

public class ToolController : MonoBehaviour
{
    private string apiUrl = "http://localhost:8080/api/tools";

    public IEnumerator GetAllTools(System.Action<List<Tool>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"tools\":" + www.downloadHandler.text + "}";
                ToolListWrapper wrapper = JsonUtility.FromJson<ToolListWrapper>(json);
                callback(wrapper.tools);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator GetToolById(int id, System.Action<Tool> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Tool tool = JsonUtility.FromJson<Tool>(www.downloadHandler.text);
                callback(tool);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator CreateTool(Tool tool, System.Action<Tool> callback)
    {
        string jsonData = JsonUtility.ToJson(tool);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Tool created = JsonUtility.FromJson<Tool>(www.downloadHandler.text);
                callback(created);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator UpdateTool(int id, Tool tool, System.Action<Tool> callback)
    {
        string jsonData = JsonUtility.ToJson(tool);
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/{id}", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Tool updated = JsonUtility.FromJson<Tool>(www.downloadHandler.text);
                callback(updated);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator DeleteTool(int id, System.Action<bool> callback)
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
    private class ToolListWrapper
    {
        public List<Tool> tools;
    }
}