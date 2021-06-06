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
        private int c;
        private int s;
        private int m;

        public Transform Mothership;

        public GameObject WinMenu;

        public DifficultyAttributes MapDifficulty;

        public ShipAttributes PurpleShip;
        public ShipAttributes OrangeShip;
        public ShipAttributes LimeShip;

        public float WaveXSummon = 30;

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
            if (currentWave >= maxWaves)
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
            }

            if (orangeShips < orangeShipsMax)
            {
                PoolMember orange = PoolManager.Instance.Request(OrangeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + 40, Mothership.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                orange.Emerge(pos, transform.rotation);

                orangeShips++;
            }

            if (limeShips < limeShipsMax)
            {
                PoolMember lime = PoolManager.Instance.Request(LimeShip.Prefab);

                Vector2 pos = new Vector2(Random.Range(Mothership.position.x + 40, Mothership.position.x + 80), Random.Range(MapDifficulty.MapHeight, -MapDifficulty.MapHeight));

                lime.Emerge(pos, transform.rotation);

                limeShips++;
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
                    c++;
                    if (c >= 100)
                    {
                        c = 0;
                        s++;
                    }
                    if (s >= 60)
                    {
                        s = 0;
                        m++;
                    }
                }
                mapTimer.Value = string.Format("{0:00}:{1:00}:{2:00}", m, s, c);
                yield return new WaitForSeconds(.01f);
            }
        }
    }
}
