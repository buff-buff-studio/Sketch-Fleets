using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;
using ManyTools.UnityExtended.Poolable;

namespace SketchFleets
{
    public class LevelManager : MonoBehaviour
    {
        public GameObject Mothership;

        public GameObject WinPrefab;
        public GameObject WinMenu;

        public DifficultyAttributes MapDifficulty;

        public ShipAttributes PurpleShip;
        public ShipAttributes OrangeShip;
        public ShipAttributes LimeShip;

        public float WaveXSummon = 30;
        int PurpleShipsMax;
        int PurpleShips;
        int OrangeShips;
        int OrangeShipsMax;
        int LimeShips;
        int LimeShipsMax;

        Vector2 SpawnArea;

        void Start()
        {
            int multiply = MapDifficulty.MapDifficulty[MapDifficulty.Difficulty];
            PurpleShipsMax = Random.Range(2 * multiply, 5 * multiply);
            OrangeShipsMax = Random.Range(1 * multiply, 4 * multiply);
            LimeShipsMax = Random.Range(1 * multiply, 2 * multiply);

            SpawnArea = new Vector2(MapDifficulty.MapSize[MapDifficulty.Difficulty].Value.x, MapDifficulty.MapSize[MapDifficulty.Difficulty].Value.y);

            GameObject end = (GameObject)Instantiate(WinPrefab, new Vector2(MapDifficulty.MapSize[MapDifficulty.Difficulty].Value.y + 10, 0), new Quaternion(0, 0, 0, 0));
            end.GetComponent<mapEndScript>().WinMenu = WinMenu;
        }

        void Update()
        {
            if (Mothership.transform.position.x < WaveXSummon)
                return;
            Debug.Log("Wave");
            Wave();
        }

        public void Wave()
        {
            if(PurpleShips < PurpleShipsMax)
            {
                PoolMember purple = PoolManager.Instance.Request(PurpleShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.transform.position.x + 40, Mothership.transform.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                purple.Emerge(pos, transform.rotation);

                PurpleShips++;
            }

            if (OrangeShips < OrangeShipsMax)
            {
                PoolMember orange = PoolManager.Instance.Request(OrangeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.transform.position.x + 40, Mothership.transform.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                orange.Emerge(pos, transform.rotation);

                OrangeShips++;
            }

            if (LimeShips < LimeShipsMax)
            {
                PoolMember lime = PoolManager.Instance.Request(LimeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.transform.position.x + 40, Mothership.transform.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                lime.Emerge(pos, transform.rotation);

                LimeShips++;
            }

            if (LimeShips < LimeShipsMax && OrangeShips < OrangeShipsMax && PurpleShips < PurpleShipsMax)
                return;
            WaveXSummon = Mothership.transform.position.x + 45;
        }
    }
}
