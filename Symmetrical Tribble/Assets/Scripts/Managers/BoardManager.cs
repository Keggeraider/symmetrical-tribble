using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [System.Serializable]
    public class Count {
        public int maximum;
        public int minimum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    private int numberOfPickups;
    public GameObject[] wallPrefab;
    public GameObject[] pickupPrefab;
    public GameObject[] spawnPointPrefab;
    public GameObject[] bombTriggerPrefab;
    public Count pickupCount = new Count(10, 15);
    public Count wallCount = new Count(40, 75);
    public Count bombTriggerCount = new Count(1, 3);
    public Count spawnPointCount = new Count(1, 3);

    private GameObject _target;
    private int boardSize = 10; // Set this dynamically from ground prefab at some point
    private int boardBuffer = 2; // Buffer to leave around the edge
    private Transform boardHolder;
    private List <Vector3> gridPositions = new List <Vector3>();

    void initializeList() {
        gridPositions.Clear();

        for (int x = boardBuffer - boardSize; x < boardSize - boardBuffer; x++) {
            for (int y = boardBuffer - boardSize; y < boardSize - boardBuffer; y++) {
                gridPositions.Add(new Vector3(x, 0.1f, y));
            }
        }
    }

    Vector3 randomPosition() {
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    int layoutRandomObject(GameObject[] item, int minimum, int maximum) {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++) {
            Vector3 randPos = randomPosition();
            int randItem = Random.Range(0, item.Length);

            Instantiate(item[randItem], randPos, Quaternion.identity, boardHolder);
        }

        return objectCount;
    }

    public void setupScene(int level) {
        // Attach Player's ball to Target
        _target = GameObject.FindGameObjectWithTag("Player");
        boardHolder = new GameObject ("Board").transform;

        initializeList();
        layoutRandomObject(wallPrefab, wallCount.minimum, wallCount.maximum);
        numberOfPickups = layoutRandomObject(pickupPrefab, pickupCount.minimum, pickupCount.maximum);

        layoutRandomObject(bombTriggerPrefab, bombTriggerCount.minimum, bombTriggerCount.maximum);

        // Determine number of enemy spawn points based on current level
        layoutRandomObject(spawnPointPrefab, spawnPointCount.minimum, spawnPointCount.maximum);
    }

    public GameObject getTarget() {
        return _target;
    }

    public int getNumPickups() {
        return numberOfPickups;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
