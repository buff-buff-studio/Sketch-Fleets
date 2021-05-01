using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Mothership;
    [SerializeField]
    private GameObject limePrefab;
    [SerializeField]
    private GameObject orangePrefab;
    [SerializeField]
    private GameObject purplePrefab;

    [SerializeField]
    private Vector3 SpawnArea;

    [SerializeField]
    private List<GameObject> limeEnemy;
    [SerializeField]
    private List<GameObject> orangeEnemy;
    [SerializeField]
    private List<GameObject> purpleEnemy;

    void Start()
    {
        SpawnArea.y = Random.Range(200, 1000);
        LimeSpawner(Random.Range(10, 30));
        OrangeSpawner(Random.Range(10, 50));
        PurpleSpawner(Random.Range(15, 30));
    }
    
    private void LimeSpawner(int n)
    {
        for(int i = 0; i < n; i++)
        {
            Vector2 pos = new Vector2(Random.Range(SpawnArea.y, SpawnArea.z), Random.Range(SpawnArea.x, -SpawnArea.x));
            GameObject lime = (GameObject)Instantiate(limePrefab, pos, transform.rotation);
            lime.transform.parent = transform;
            limeEnemy.Add(lime);
        }
    }

    private void OrangeSpawner(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector2 pos = new Vector2(Random.Range(SpawnArea.y, SpawnArea.z), Random.Range(SpawnArea.x, -SpawnArea.x));
            GameObject orange = (GameObject)Instantiate(orangePrefab, pos, transform.rotation);
            orange.GetComponent<orangeAI>().Mothership = Mothership.transform;
            orange.GetComponent<orangeAI>().InCam = false; 
            orange.transform.parent = transform;
            orangeEnemy.Add(orange);
        }
    }

    private void PurpleSpawner(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector2 pos = new Vector2(Random.Range(SpawnArea.y, SpawnArea.z), Random.Range(SpawnArea.x, -SpawnArea.x));
            GameObject purple = (GameObject)Instantiate(purplePrefab, pos, transform.rotation);
            purple.transform.parent = transform;
            purpleEnemy.Add(purple);
        }
    }
}
