using UnityEngine;

public class GhostRoomManager : MonoBehaviour
{
    public GameObject[] ghostRooms;

    void Start()
    {
        RandomizeGhostRoom();
    }

    void RandomizeGhostRoom()
    {
        // Deactivate all ghost rooms first
        foreach (GameObject room in ghostRooms)
        {
            room.SetActive(false);
        }

        // Randomly select one room to activate
        int randomIndex = Random.Range(0, ghostRooms.Length);
        ghostRooms[randomIndex].SetActive(true);

        Debug.Log("Selected Ghost Room: " + ghostRooms[randomIndex].name);
    }
}
