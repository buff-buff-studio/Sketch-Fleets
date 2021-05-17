using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;
using SketchFleets;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Mothership;

    [SerializeField]
    private DifficultyAttributes attributes;
    [SerializeField]
    private GameObject mapEnd;
    [SerializeField]
    private GameObject winMenu;

    private int difficulty;
    private int multiply;

    [SerializeField]
    private List<GameObject> limeEnemy;
    [SerializeField]
    private List<GameObject> orangeEnemy;
    [SerializeField]
    private List<GameObject> purpleEnemy;

    void Start()
    {
        difficulty = attributes.Difficulty;
        multiply = attributes.MapDifficulty[difficulty];

        if(difficulty != 0)
        {
            LimeSpawner(Random.Range(10* multiply, 15* multiply));
            OrangeSpawner(Random.Range(10* multiply, 20* multiply));
            PurpleSpawner(Random.Range(10* multiply, 25* multiply));

            GameObject end = (GameObject)Instantiate(mapEnd, new Vector2(attributes.MapSize[difficulty].Value.y + 10, 0), new Quaternion(0, 0, 0, 0));
            end.GetComponent<mapEndScript>().WinMenu = winMenu;
        }

    }
    
    private void LimeSpawner(int n)
    {
        Vector2 spawn = new Vector2(attributes.MapSize[difficulty].Value.x, attributes.MapSize[difficulty].Value.y);
        for(int i = 0; i < n; i++)
        {
            Vector2 pos = new Vector2(Random.Range(attributes.MapStartSpawn, Random.Range(spawn.x, spawn.y)), Random.Range(attributes.MapHeight, -attributes.MapHeight));

            GameObject lime = (GameObject)Instantiate(attributes.Lime, pos, transform.rotation);

            lime.transform.parent = transform;
            limeEnemy.Add(lime);
        }
    }

    private void OrangeSpawner(int n)
    {
        Vector2 spawn = new Vector2(attributes.MapSize[difficulty].Value.x, attributes.MapSize[difficulty].Value.y);
        for (int i = 0; i < n; i++)
        {
            Vector2 pos = new Vector2(Random.Range(attributes.MapStartSpawn, Random.Range(spawn.x, spawn.y)), Random.Range(attributes.MapHeight, -attributes.MapHeight));

            GameObject orange = (GameObject)Instantiate(attributes.Orange, pos, transform.rotation);

            orange.GetComponent<orangeAI>().Mothership = Mothership.transform;
            orange.GetComponent<orangeAI>().InCam = false; 
            orange.transform.parent = transform;
            orangeEnemy.Add(orange);
        }
    }

    private void PurpleSpawner(int n)
    {
        Vector2 spawn = new Vector2(attributes.MapSize[difficulty].Value.x, attributes.MapSize[difficulty].Value.y);
        for (int i = 0; i < n; i++)
        {
            Vector2 pos = new Vector2(Random.Range(attributes.MapStartSpawn, Random.Range(spawn.x, spawn.y)), Random.Range(attributes.MapHeight, -attributes.MapHeight));

            GameObject purple = (GameObject)Instantiate(attributes.Purple, pos, transform.rotation);

            purple.transform.parent = transform;
            purpleEnemy.Add(purple);
        }
    }

    private void BossSpawner(int n)
    {
        Vector2 pos = new Vector2(attributes.MapSize[4].Value.y, 0);

        GameObject boss = (GameObject)Instantiate(attributes.Boss[Random.Range(0,attributes.Boss.Length)], pos, new Quaternion(0, 0, 0, 0));
    }
}
