using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedButtonSpawner : MonoBehaviour
{
    // The agent prefab to spawn
    public GameObject agentPrefab;

    // Array of spawn locations
    public Transform[] spawnLocations;

    void Start()
    {
        // Get the selected agent names from ButtonGenerator instance
        List<string> selectedNames = ButtonGenerator.instance.GetSelectedNames();

        // Spawn agents based on the selected agent names
        SpawnAgents(selectedNames);
    }

    void SpawnAgents(List<string> selectedNames)
    {
        int spawnIndex = 0;

        // Spawn the requested agents based on the selected names
        foreach (string agentName in selectedNames)
        {
            // Ensure we don't go out of bounds of the spawn locations array
            if (spawnIndex < spawnLocations.Length)
            {
                // Instantiate an agent prefab at the spawn location
                GameObject spawnedAgent = Instantiate(agentPrefab, spawnLocations[spawnIndex].position, Quaternion.identity);

                // Debug: Check if the agent prefab has any children
                Debug.Log($"Spawned agent at location {spawnLocations[spawnIndex].position}. Checking children...");

                // Check if there are any children
                if (spawnedAgent.transform.childCount > 0)
                {
                    Transform firstChildTransform = spawnedAgent.transform.GetChild(0); // Get the first child
                    Debug.Log("Found first child: " + firstChildTransform.name);

                    TextMeshPro agentNameTextMesh = firstChildTransform.GetComponent<TextMeshPro>(); // Get the TextMesh component
                    Debug.Log("previous name is  = " + agentNameTextMesh.text);
                    if (agentNameTextMesh != null)
                    {
                        agentNameTextMesh.text = agentName; // Set the name text of the agent
                        Debug.Log("Assigned name to TextMesh: " + agentName);
                    }
                    else
                    {
                        Debug.LogWarning("No TextMesh component found in the first child.");
                    }
                }
                else
                {
                    Debug.LogWarning("No children found in the spawned agent prefab.");
                }

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
