using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class Weapon
{
    public int weaponId;
    public string name;
    public string status;
    // Add other fields as needed
}

public class WeaponController : MonoBehaviour
{
    private string apiUrl = "http://localhost:8080/api/weapons";

    public IEnumerator GetAllWeapons(System.Action<List<Weapon>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"weapons\":" + www.downloadHandler.text + "}";
                WeaponListWrapper wrapper = JsonUtility.FromJson<WeaponListWrapper>(json);
                callback(wrapper.weapons);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator GetWeaponById(int id, System.Action<Weapon> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}/{id}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Weapon weapon = JsonUtility.FromJson<Weapon>(www.downloadHandler.text);
                callback(weapon);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator CreateWeapon(Weapon weapon, System.Action<Weapon> callback)
    {
        string jsonData = JsonUtility.ToJson(weapon);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Weapon created = JsonUtility.FromJson<Weapon>(www.downloadHandler.text);
                callback(created);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator UpdateWeapon(int id, Weapon weapon, System.Action<Weapon> callback)
    {
        string jsonData = JsonUtility.ToJson(weapon);
        using (UnityWebRequest www = new UnityWebRequest($"{apiUrl}/{id}", "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Weapon updated = JsonUtility.FromJson<Weapon>(www.downloadHandler.text);
                callback(updated);
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    public IEnumerator DeleteWeapon(int id, System.Action<bool> callback)
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
    private class WeaponListWrapper
    {
        public List<Weapon> weapons;
    }
}