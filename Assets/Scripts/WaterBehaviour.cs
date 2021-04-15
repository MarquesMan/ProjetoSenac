using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject waterSplashPrefab;

    private List<GameObject> waterPrefabList;

    private void Start()
    {
        //waterPrefabList = new 
    }

    GameObject lastGameObject;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
            collision.gameObject.GetComponent<Key>()?.RestartPosition();

        
        if (lastGameObject is null || !lastGameObject.Equals(collision.gameObject))
        {
            SetSplashPosition(collision.GetContact(0).point + Vector3.up * 1.2f);
            lastGameObject = collision.gameObject;
        }

        if (lastGameObject is null)
            lastGameObject = collision.gameObject;

        /*waterSplashPrefab.transform.localScale = Vector3.one * (collision.collider.attachedRigidbody.mass / 100);
        foreach(Transform childTransform in waterSplashPrefab.GetComponentsInChildren<Transform>())
        {
            Debug.LogError("A");
            childTransform.localScale  = Vector3.one * (collision.collider.attachedRigidbody.mass / 100);
        }*/

    }

    private void OnCollisionExit(Collision collision)
    {
        if (lastGameObject.Equals(collision.gameObject)) lastGameObject = null;
    }

    public void SetSplashPosition(Vector3 position)
    {
        waterSplashPrefab.SetActive(false);
        waterSplashPrefab.SetActive(true);
        waterSplashPrefab.transform.position = position;
    }
}
