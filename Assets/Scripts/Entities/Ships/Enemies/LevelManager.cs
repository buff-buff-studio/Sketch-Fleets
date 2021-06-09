using System.Collections;
using ManyTools.UnityExtended;
using UnityEngine;
using SketchFleets.Data;
using ManyTools.Variables;
using ManyTools.UnityExtended.Poolable;
using SketchFleets.Entities;

namespace SketchFleets
{
    public class LevelManager : Singleton<LevelManager>
    {
        private Mothership player;
        
        private int purpleShipsMax;
        private int purpleShips;
        private int orangeShips;
        private int orangeShipsMax;
        private int limeShips;
        private int limeShipsMax;

        private int maxWaves;
        private int currentWave;

        [SerializeField]
        private StringReference mapTimer;
        [SerializeField]
        private IntReference s;
        [SerializeField]
        private IntReference m;

        [SerializeField]
        private IntReference enemiesKilled;
        private int enemiesWave;
        private int enemiesKilledWave;

        [SerializeField]
        [Tooltip("Map Width = 55")]
        private Vector2 waveSize;

        public Transform Mothership;

        public GameObject WinMenu;

        public DifficultyAttributes MapDifficulty;

        public ShipAttributes PurpleShip;
        public ShipAttributes OrangeShip;
        public ShipAttributes LimeShip;

        public Mothership Player => player;

        protected override void Awake()
        {
            base.Awake();
            player = FindObjectOfType<Mothership>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Time.timeScale = 1f;
        }

        void Start()
        {
            if(MapDifficulty.Map == 1)
            {
                s.Value = 0;
                m.Value = 0;
                enemiesKilled.Value = 0;
            }

            int multiply = MapDifficulty.MapDifficulty[MapDifficulty.Difficulty];
            purpleShipsMax = Random.Range(3 * multiply, 5 * multiply);
            orangeShipsMax = Random.Range(2 * multiply, 5 * multiply);
            limeShipsMax = Random.Range(1 * multiply, 3 * multiply);

            StartCoroutine(Timer());

            maxWaves = (int)Random.Range(MapDifficulty.MapWaves[MapDifficulty.Difficulty].Value.x, MapDifficulty.MapWaves[MapDifficulty.Difficulty].Value.y);

            enemiesWave = purpleShipsMax + limeShipsMax + orangeShipsMax;

            if(multiply > 2)
            {
                waveSize = new Vector2(waveSize.x, waveSize.y * (multiply/2));
            }
        }

        void Update()
        {
            if (enemiesKilled >= enemiesKilledWave)
            {
                if (limeShips >= limeShipsMax && orangeShips >= orangeShipsMax && purpleShips >= purpleShipsMax)
                {
                    purpleShips = 0;
                    orangeShips = 0;
                    limeShips = 0;
                }
                if (currentWave < maxWaves)
                {
                    Wave();
                }
            }
            if (currentWave >= maxWaves)
            {
                EndGame();
            }

            Debug.Log(enemiesKilled + ", " + enemiesKilledWave + ", " + enemiesWave);
        }

        public void Wave()
        {
            if(purpleShips < purpleShipsMax)
            {
                PoolMember purple = PoolManager.Instance.Request(PurpleShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + waveSize.x, Mothership.position.x + waveSize.y), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                purple.Emerge(pos, transform.rotation);
                purpleShips++;
            }

            if (orangeShips < orangeShipsMax)
            {
                PoolMember orange = PoolManager.Instance.Request(OrangeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + waveSize.x, Mothership.position.x + waveSize.y), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                orange.Emerge(pos, transform.rotation);

                orangeShips++;
            }

            if (limeShips < limeShipsMax)
            {
                PoolMember lime = PoolManager.Instance.Request(LimeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + waveSize.x, Mothership.position.x + waveSize.y), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                lime.Emerge(pos, transform.rotation);

                limeShips++;
            }

            if (limeShips >= limeShipsMax && orangeShips >= orangeShipsMax && purpleShips >= purpleShipsMax)
            {
                currentWave++;
                enemiesKilledWave = enemiesKilled.Value + enemiesWave;
            }
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
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
