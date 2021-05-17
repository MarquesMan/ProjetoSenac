using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPoints : MonoBehaviour
{
    [SerializeField]
    List<Transform> listOfSpawnPoints; 

    // Start is called before the first frame update
    void Start()
    {
        if (listOfSpawnPoints == null || listOfSpawnPoints.Count <=0) return;        
        listOfSpawnPoints.Add(this.transform);

        for (int i = listOfSpawnPoints.Count - 1; i >= 0; i--)
            if (listOfSpawnPoints[i] == null)
                listOfSpawnPoints.RemoveAt(i);
            

        var randomIndex = Random.Range(0, listOfSpawnPoints.Count - 1);
        Debug.LogWarning(randomIndex);

        transform.position = listOfSpawnPoints[randomIndex].position;
        transform.rotation = listOfSpawnPoints[randomIndex].rotation;
        transform.SetParent(listOfSpawnPoints[randomIndex].parent);
        listOfSpawnPoints.Remove(transform);

        foreach (Transform spawnTransform in listOfSpawnPoints) Destroy(spawnTransform.gameObject);

    }
}
