using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedButtonSpawner : MonoBehaviour
{ // The agent prefab to spawn
    public GameObject agentPrefab;

    // Array of spawn locations
    public Transform[] spawnLocations;

    void Start()
    {
        // Get the selected agent names from ButtonGenerator instance
        List<string> selectedNames = ButtonGenerator.instance.GetSelectedNames();

        // Spawn agents based on the selected agent names
        SpawnAgents(selectedNames.Count);  // The number of agents is the count of selected names
    }

    void SpawnAgents(int totalAgents)
    {
        int spawnIndex = 0;

        // Spawn the requested number of agents
        for (int i = 0; i < totalAgents; i++)
        {
            // Ensure we don't go out of bounds of the spawn locations array
            if (spawnIndex < spawnLocations.Length)
            {
                // Spawn an agent at the corresponding location
                Instantiate(agentPrefab, spawnLocations[spawnIndex].position, Quaternion.identity);
                spawnIndex++;
            }
            else
            {
                Debug.LogWarning("Not enough spawn locations for all the requested agents.");
                break; // Stop if we run out of spawn locations
            }
        }
    }
}
