using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject waterSplashPrefab;
    [SerializeField] int waterSplashMax = 5;
    private List<GameObject> waterPrefabList;
    private int waterListIndex = 0;

    private void Start()
    {
        waterPrefabList = new List<GameObject>(waterSplashMax);
        for(int i = 0; i < waterPrefabList.Capacity; ++i)
        {
            var tempWaterSplash = Instantiate<GameObject>(waterSplashPrefab,transform.parent);
            tempWaterSplash.SetActive(false);
            waterPrefabList.Add(tempWaterSplash);
        }
    }

    GameObject lastGameObject;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
            collision.gameObject.GetComponent<Key>()?.RestartPosition();

        SetSplashPosition(collision.GetContact(0).point + Vector3.up * 1.2f);
    }

    public void SetSplashPosition(Vector3 position)
    {
        var currentWaterSplash = waterPrefabList[waterListIndex];
        waterListIndex = (waterListIndex + 1) % waterPrefabList.Capacity;

        currentWaterSplash.SetActive(false);
        currentWaterSplash.SetActive(true);
        currentWaterSplash.transform.position = position;
    }
}
