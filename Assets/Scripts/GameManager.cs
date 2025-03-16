using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour {
    public FirstPersonController player;
    bool lastGuess = false; // Was the last door correct?
    public GameObject resultScreen;
    [SerializeField] TextMeshProUGUI resultMessage;

    [SerializeField] int maxLevel;

    [Header("Current Progress")]
    [SerializeField] int currentLevel = 1;
    [SerializeField] Room currentRoom;
    [SerializeField] Transform spawner;

    [SerializeField] int correctLevel1;
    [SerializeField] int correctLevel2;
    [SerializeField] int correctLevel3;


    [Header("Final Room")]
    [SerializeField] Room finalRoom;

    [Header("Rooms")]
    [SerializeField] GameObject roomHolder_level1;
    [SerializeField] HashSet<Room> Level1Rooms;
    [SerializeField] HashSet<Room> Level1Rooms_Used;

    [SerializeField] GameObject roomHolder_level2;
    [SerializeField] HashSet<Room> Level2Rooms;
    [SerializeField] HashSet<Room> Level2Rooms_Used;

    [SerializeField] GameObject roomHolder_level3;
    [SerializeField] HashSet<Room> Level3Rooms;
    [SerializeField] HashSet<Room> Level3Rooms_Used;

    private void Awake() {
        Level1Rooms = new HashSet<Room>();
        Level2Rooms = new HashSet<Room>();
        Level3Rooms = new HashSet<Room>();

        Level1Rooms_Used = new HashSet<Room>();
        Level2Rooms_Used = new HashSet<Room>();
        Level3Rooms_Used = new HashSet<Room>();
    }

    Room GetRandomRoom() {
        HashSet<Room> roomSet = GetRoomSet(currentLevel);

        List<Room> roomList = new List<Room>(roomSet);
        int randomIndex = Random.Range(0, roomList.Count);
        Room room = roomList[randomIndex];

        UseRoom(room);

        return room;
    }

    void UseRoom(Room room) {
        switch (currentLevel) {
            case 1:
                Level1Rooms.Remove(room);
                Level1Rooms_Used.Add(room);
                break;

            case 2:
                Level2Rooms.Remove(room);
                Level2Rooms_Used.Add(room);
                break;

            case 3:
                Level3Rooms.Remove(room);
                Level3Rooms_Used.Add(room);
                break;
        }
    }

    HashSet<Room> GetRoomSet(int level) {
        HashSet<Room> roomSet = new HashSet<Room>();
        
        switch (level) {
            case 1:
                roomSet = Level1Rooms;
                if (roomSet.Count == 0)
                    roomSet = ReplenishRooms(Level1Rooms, Level1Rooms_Used);
                break;

            case 2:
                roomSet = Level2Rooms;
                if (roomSet.Count == 0)
                    roomSet = ReplenishRooms(Level2Rooms, Level2Rooms_Used);
                break;

            case 3:
                roomSet = Level3Rooms;
                if (roomSet.Count == 0)
                    roomSet = ReplenishRooms(Level3Rooms, Level3Rooms_Used);
                break;
        }

        return roomSet;
    }

    HashSet<Room> ReplenishRooms(HashSet<Room> available, HashSet<Room> used) {
        available.UnionWith(used);
        used.Clear();
        return available;
    }

    // Start is called before the first frame update
    void Start() {
        // Define Room Sets
        foreach (Transform child in roomHolder_level1.transform)
            Level1Rooms.Add(child.GetComponent<Room>());

        foreach (Transform child in roomHolder_level2.transform)
            Level2Rooms.Add(child.GetComponent<Room>());

        foreach (Transform child in roomHolder_level3.transform)
            Level3Rooms.Add(child.GetComponent<Room>());


        // Select Starting Room
        Spawn();
    }

    void Spawn() {
        if (currentLevel > maxLevel) {
            currentRoom = finalRoom;
            spawner = currentRoom.spawnPoint;
            TeleportTo(spawner.position);
            return;
        }

        currentRoom = GetRandomRoom();
        spawner = currentRoom.spawnPoint;
        TeleportTo(spawner.position);
        if (!player.gameObject.activeSelf)
            player.gameObject.SetActive(true);
    }

    public void NextLevel() {
        switch(currentLevel) {
            case 1:
                correctLevel1++;
                break;

            case 2:
                correctLevel2++;
                break;

            case 3:
                correctLevel3++; // unused (no level 4)
                break;
        }

        currentLevel++;
        DisableMovement(true);
        Spawn();
    }

    public void Respawn() {
        if (currentLevel > 1) {
            currentLevel = LevelChecker();
        }

        lastGuess = false;
        DisableMovement(false);
        Spawn();
    }

    int LevelChecker() {
        switch (currentLevel) {
            case 2:
                if (correctLevel1 >= 3)
                    return 2;
                break;

            case 3:
                if (correctLevel2 >= 3)
                    return 3;
                break;
        }

        return 1;
    }

/*
    // Update is called once per frame
    void Update() { 
        if (Input.GetKeyDown(KeyCode.R)) { // Respawn Player in Current Room
            TeleportTo(spawner.position);
        }

        if (Input.GetKeyDown(KeyCode.P))
            NextLevel();
    }
*/

    void TeleportTo(Vector3 position) {
        player.transform.position = position;
        Vector3 newRotation = player.transform.rotation.eulerAngles;
        newRotation.y = 0;
        player.transform.rotation = Quaternion.Euler(newRotation);
        
        IEnumerator coRoutine = EnableMov();
        StartCoroutine(coRoutine);
    }

    IEnumerator EnableMov() {
        yield return new WaitForSeconds(3);
        EnableMovement();
    }




    void DisableMovement(bool doorGuess) {
        player.enabled = false;
        switch (doorGuess) {
            case true: // Correct Guess
                resultMessage.text = "<b>Acesso Concedido.</b>\n<size=28>Sistema desbloqueado.</size>";
                resultMessage.color = Color.green;
                break;

            case false: // Incorrect Guess
                resultMessage.text = "<b>Acesso Negado.</b>\n<size=28>A reiniciar defesas...</size>";
                resultMessage.color = Color.red;
                break;
        }
        resultScreen.SetActive(true);
    }

    void EnableMovement() {
        player.enabled = true;
        resultScreen.SetActive(false);
    }
}
