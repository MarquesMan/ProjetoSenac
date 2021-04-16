using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject waterSplashPrefab;
    [SerializeField] int waterSplashMax = 5;
    [SerializeField] Transform planeTransform;

    private Vector3 positionHelper = Vector3.zero;

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

        positionHelper.y = planeTransform.position.y + 0.1f;
    }

    GameObject lastGameObject;

    private void OnTriggerEnter(Collider other)
    {
        SetSplashPosition(other.transform.position);

        if (other.CompareTag("Key"))
            other.gameObject.GetComponent<Key>()?.RestartPosition();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key")) return;

        SetSplashPosition(other.transform.position);
    }

    public void SetSplashPosition(Vector3 position)
    {
        var currentWaterSplash = waterPrefabList[waterListIndex];
        waterListIndex = (waterListIndex + 1) % waterPrefabList.Capacity;

        positionHelper.x = position.x;
        positionHelper.z = position.z;

        currentWaterSplash.SetActive(false);
        currentWaterSplash.SetActive(true);
        currentWaterSplash.transform.position = positionHelper;
    }
}
