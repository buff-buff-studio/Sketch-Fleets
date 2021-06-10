using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private List<GameObject> ObstaclePrefab;
    [SerializeField]
    private GameObject Obstacle;
    [SerializeField]
    private GameObject CautionImage;
    [SerializeField]
    private Vector3 SpawnArea;
    [SerializeField]
    private Transform Mothership;
    [SerializeField]
    private Canvas canvas;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        StartCoroutine(SpawnerUpdate());
    }
    #endregion

    #region Obstacle Spawner
    /// <summary>
    /// Every 5 minutes there is a 25% chance that an obstacle will be generated.
    /// 2 obstacles do not coexist at the same time.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnerUpdate()
    {
        yield return new WaitForSeconds(5);
        int i = Random.Range(1, 101);
        if (i < 26 && Obstacle == null)
        {
            GameObject ob = ObstaclePrefab[Random.Range(0, ObstaclePrefab.Count)];
            char direction = ob.GetComponent<ObstacleScript>().Direction;
            Vector2 vect = Vector2.zero;
            Vector3 rot = Vector3.zero;
            if (direction == 'R')
            {
                vect = new Vector2(Mothership.position.x + SpawnArea.z, Random.Range(SpawnArea.x, SpawnArea.y));

                GameObject obstacle = (GameObject)Instantiate(ob, vect, transform.rotation);
                Obstacle = obstacle;
                obstacle.transform.eulerAngles = rot;

                StartCoroutine(ColliderAllert());
            }
            else if (direction == 'D')
            {
                vect = new Vector2(Mothership.position.x + SpawnArea.z/2, Mothership.position.y + SpawnArea.z/2);

                GameObject obstacle = (GameObject)Instantiate(ob, vect, transform.rotation);
                Obstacle = obstacle;
                obstacle.transform.eulerAngles = rot;
            }
            else if (direction == 'A')
            {
                vect = new Vector2(Random.Range(Mothership.position.x + SpawnArea.z, Mothership.position.x + SpawnArea.z*1.25f), Random.Range(SpawnArea.x, SpawnArea.y));
                rot = new Vector3(0, 0, Random.Range(0, 181));

                GameObject obstacle = (GameObject)Instantiate(ob, vect, transform.rotation);
                obstacle.transform.eulerAngles = rot;
                Obstacle = obstacle;
            }
        }
        StartCoroutine(SpawnerUpdate());
    }

    /// <summary>
    /// Shows the obstacle alert 
    /// </summary>
    /// <returns></returns>
    IEnumerator ColliderAllert()
    {
        CautionImage.SetActive(true);
        yield return new WaitForSeconds(1);
        CautionImage.SetActive(false);
    }
    #endregion
}
