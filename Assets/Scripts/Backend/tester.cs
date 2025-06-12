using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackendTestRunner : MonoBehaviour
{
    public CriminalManager criminalManager;
    public WeaponManager weaponManager;
    public ToolManager toolManager;
    public RoomManager roomManager;
    public PuzzleManager puzzleManager;
    public PlayerManager playerManager;
    public FlagManager flagManager;

    void Start()
    {
        // Find managers if not assigned
        if (!criminalManager) criminalManager = FindObjectOfType<CriminalManager>();
        if (!weaponManager) weaponManager = FindObjectOfType<WeaponManager>();
        if (!toolManager) toolManager = FindObjectOfType<ToolManager>();
        if (!roomManager) roomManager = FindObjectOfType<RoomManager>();
        if (!puzzleManager) puzzleManager = FindObjectOfType<PuzzleManager>();
        if (!playerManager) playerManager = FindObjectOfType<PlayerManager>();
        if (!flagManager) flagManager = FindObjectOfType<FlagManager>();

        // Start tests
        StartCoroutine(TestAll());
    }

    IEnumerator TestAll()
    {
        Debug.Log("Testing CriminalManager...");
        if (criminalManager && criminalManager.criminalController)
            yield return StartCoroutine(criminalManager.criminalController.GetAllCriminals(OnCriminalsReceived));

        Debug.Log("Testing WeaponManager...");
        if (weaponManager && weaponManager.weaponController)
            yield return StartCoroutine(weaponManager.weaponController.GetAllWeapons(OnWeaponsReceived));

        Debug.Log("Testing ToolManager...");
        if (toolManager && toolManager.toolController)
            yield return StartCoroutine(toolManager.toolController.GetAllTools(OnToolsReceived));

        Debug.Log("Testing RoomManager...");
        if (roomManager && roomManager.roomController)
            yield return StartCoroutine(roomManager.roomController.GetAllRooms(OnRoomsReceived));

        Debug.Log("Testing PuzzleManager...");
        if (puzzleManager && puzzleManager.puzzleController)
            yield return StartCoroutine(puzzleManager.puzzleController.GetAllPuzzles(OnPuzzlesReceived));

        Debug.Log("Testing PlayerManager...");
        if (playerManager && playerManager.playerController)
            yield return StartCoroutine(playerManager.playerController.GetAllPlayers(OnPlayersReceived));

        Debug.Log("Testing FlagManager...");
        if (flagManager && flagManager.flagController)
            yield return StartCoroutine(flagManager.flagController.GetAllFlags(OnFlagsReceived));

        Debug.Log("All backend tests finished.");
    }

    public void RunAllTests()
    {
        StartCoroutine(TestAll());
    }

    void OnCriminalsReceived(List<Criminal> criminals)
    {
        Debug.Log($"Criminals received: {criminals?.Count ?? 0}");
        if (criminals != null)
            foreach (var c in criminals)
                Debug.Log($"Criminal: {c.name}, Suspicion: {c.suspicion}");
    }

    void OnWeaponsReceived(List<Weapon> weapons)
    {
        Debug.Log($"Weapons received: {weapons?.Count ?? 0}");
        if (weapons != null)
            foreach (var w in weapons)
                Debug.Log($"Weapon: {w.name}, Status: {w.status}");
    }

    void OnToolsReceived(List<Tool> tools)
    {
        Debug.Log($"Tools received: {tools?.Count ?? 0}");
        if (tools != null)
            foreach (var t in tools)
                Debug.Log($"Tool: {t.name}, Function: {t.function}");
    }

    void OnRoomsReceived(List<Room> rooms)
    {
        Debug.Log($"Rooms received: {rooms?.Count ?? 0}");
        if (rooms != null)
            foreach (var r in rooms)
                Debug.Log($"Room: {r.description}");
    }

    void OnPuzzlesReceived(List<Puzzle> puzzles)
    {
        Debug.Log($"Puzzles received: {puzzles?.Count ?? 0}");
        if (puzzles != null)
            foreach (var p in puzzles)
                Debug.Log($"Puzzle: {p.type}, Solved: {p.solved}");
    }

    void OnPlayersReceived(List<Player> players)
    {
        Debug.Log($"Players received: {players?.Count ?? 0}");
        if (players != null)
            foreach (var p in players)
                Debug.Log($"Player: {p.name}, SuspicionLevel: {p.suspicionLevel}");
    }

    void OnFlagsReceived(List<Flag> flags)
    {
        Debug.Log($"Flags received: {flags?.Count ?? 0}");
        if (flags != null)
            foreach (var f in flags)
                Debug.Log($"Flag: {f.form3D}, Lat: {f.latitude}, Lon: {f.longitude}");
    }
}