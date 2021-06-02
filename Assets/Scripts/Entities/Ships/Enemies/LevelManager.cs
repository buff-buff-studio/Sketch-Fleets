using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;
using ManyTools.Variables;
using ManyTools.UnityExtended.Poolable;

namespace SketchFleets
{
    public class LevelManager : MonoBehaviour
    {
        private int purpleShipsMax;
        private int purpleShips;
        private int orangeShips;
        private int orangeShipsMax;
        private int limeShips;
        private int limeShipsMax;

        private int maxWaves;
        private int currentWave;
        private int enemys;

        [SerializeField]
        private StringReference mapTimer;
        [SerializeField]
        private IntReference s;
        [SerializeField]
        private IntReference m;

        [SerializeField]
        private IntReference enemyKills;

        public Transform Mothership;

        public GameObject WinMenu;

        public DifficultyAttributes MapDifficulty;

        public ShipAttributes PurpleShip;
        public ShipAttributes OrangeShip;
        public ShipAttributes LimeShip;

        public float WaveXSummon = 30;

        void Start()
        {
            if(MapDifficulty.Map == 1)
            {
                enemyKills.Value = 0;
                s.Value = 0;
                m.Value = 0;
            }

            int multiply = MapDifficulty.MapDifficulty[MapDifficulty.Difficulty];
            purpleShipsMax = Random.Range(3 * multiply, 5 * multiply);
            orangeShipsMax = Random.Range(1 * multiply, 4 * multiply);
            limeShipsMax = Random.Range(1 * multiply, 2 * multiply);

            StartCoroutine(Timer());

            maxWaves = (int)Random.Range(MapDifficulty.MapWaves[MapDifficulty.Difficulty].Value.x, MapDifficulty.MapWaves[MapDifficulty.Difficulty].Value.y);
        }

        void Update()
        {
            if (Mothership.position.x > WaveXSummon && currentWave < maxWaves)
            {
                Wave();
            }
            if (currentWave >= maxWaves && enemyKills >= enemyKills + enemys)
            {
                EndGame();
            }
        }

        public void Wave()
        {
            if(purpleShips < purpleShipsMax)
            {
                PoolMember purple = PoolManager.Instance.Request(PurpleShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + 40, Mothership.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                purple.Emerge(pos, transform.rotation);

                purpleShips++;
                enemys++;
            }

            if (orangeShips < orangeShipsMax)
            {
                PoolMember orange = PoolManager.Instance.Request(OrangeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + 40, Mothership.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                orange.Emerge(pos, transform.rotation);

                orangeShips++;
                enemys++;
            }

            if (limeShips < limeShipsMax)
            {
                PoolMember lime = PoolManager.Instance.Request(LimeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + 40, Mothership.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                lime.Emerge(pos, transform.rotation);

                limeShips++;
                enemys++;
            }

            if (limeShips < limeShipsMax && orangeShips < orangeShipsMax && purpleShips < purpleShipsMax)
                return;
            currentWave++;
            WaveXSummon = Mothership.position.x + 45;
        }

        public void EndGame()
        {
            WinMenu.SetActive(true);
            Time.timeScale = 0;
        }

        IEnumerator Timer()
        {
            while (true)
            {
                if(Time.deltaTime != 0)
                {
                    s.Value++;
                    if (s >= 60)
                    {
                        s.Value = 0;
                        m.Value++;
                    }
                }
                mapTimer.Value = string.Format("{0:00}:{1:00}", m.Value, s.Value);
                yield return new WaitForSeconds(1);
            }
        }
    }
}
