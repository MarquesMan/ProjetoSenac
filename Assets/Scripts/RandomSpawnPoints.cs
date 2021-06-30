using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPoints : MonoBehaviour
{
    [SerializeField]
    List<RandomSpawnPosition> listOfSpawnPointsPositions;


    List<Transform> listOfSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (listOfSpawnPointsPositions == null || listOfSpawnPointsPositions.Count <=0) return;

        listOfSpawnPoints = new List<Transform>();
        listOfSpawnPoints.Add(this.transform);

        // Remove Posicoes invalidas
        for (int i = 0; i < listOfSpawnPointsPositions.Count; ++i)
            if (listOfSpawnPointsPositions[i] != null)
                listOfSpawnPoints.Add(listOfSpawnPointsPositions[i].transform);
            

        var randomIndex = Random.Range(0, listOfSpawnPoints.Count - 1);

        Debug.Log($"{this.gameObject.name} picked spawn point n.{randomIndex}");
        
        transform.position = listOfSpawnPoints[randomIndex].position;
        transform.rotation = listOfSpawnPoints[randomIndex].rotation;
        transform.SetParent(listOfSpawnPoints[randomIndex].parent);

        listOfSpawnPoints.Clear();

        foreach (RandomSpawnPosition spawnTransform in listOfSpawnPointsPositions) 
            Destroy(spawnTransform.gameObject);

    }

    public void InsertTransform(RandomSpawnPosition newTransform)
    {
        if (listOfSpawnPointsPositions.Contains(newTransform)) return;
        listOfSpawnPointsPositions.Add(newTransform);
    }

}
