using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public RoomController roomController;

    void Start()
    {
        if (roomController == null)
            roomController = FindObjectOfType<RoomController>();

        StartCoroutine(roomController.GetAllRooms(OnRoomsReceived));
    }

    void OnRoomsReceived(List<Room> rooms)
    {
        foreach (var r in rooms)
            Debug.Log("Room: " + r.description);
    }
}