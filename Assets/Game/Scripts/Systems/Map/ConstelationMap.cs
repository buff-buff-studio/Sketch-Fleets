using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Events;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Interaction;
using Random = UnityEngine.Random;

/// <summary>
/// Renders the constellation map UI, builds the procedural node graph, and drives zoom / focus / path animations.
/// </summary>
public class ConstelationMap : MonoBehaviour
{
    #region Visual constants

    private const int JunctionPathDiscardPixels = 10;
    private const float StarIconBaseSize = 50f;
    private const float BossIconScaleMultiplier = 3f;

    #endregion

    #region Private Fields

    [Header("Events")]
    [SerializeField]
    private GameEvent generationAnimationOver;
    [SerializeField]
    private GameEvent zoomAnimationOver;

    private bool inputEnabled = true;
    private float mapWidth;
    private float mapHeight;
    private Constelation constelation;

    #endregion

    #region Public Fields

    [Header("Map Parameters")]
    public MapLevelInteraction interaction;
    public GameObject mapPrefab;
    public GameObject pathPrefab;
    public GameObject pathHolder;
    public RectTransform mapView;
    public MapScrollRect scrollRect;
    public ZoomComponent zoom;
    public GameObject mapDisabler;
    public AnimationCurve curve;
    public AnimationCurve focusCurve;
    public AnimationCurve focusCurveScale;
    public AnimationCurve focusCurveScaleProgress;

    public static Action onMapLoad;

    public MapAttributes currentMap;
    public IntReference currentLevel;
    public IntReference currentLevelDifficulty;
    public IntReference currentSeed;

    [Range(0f, 1000f)]
    public float parallaxSpeed = 5f;

    #endregion

    #region Properties

    public bool InputEnabled
    {
        get => inputEnabled;
        set
        {
            if (inputEnabled == value)
                return;

            zoom.inputEnabled = value;
            zoom.horizontal = value;
            zoom.vertical = value;

            if (mapDisabler != null)
                mapDisabler.SetActive(!value);
            inputEnabled = value;
        }
    }

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        MapLevelInteraction.map = this;
    }

    private void Start()
    {
        constelation = new Constelation(this, interaction);
        MapLevelInteraction.state.SetConstelation(constelation);

        int seed = SketchFleets.ProfileSystem.Profile.GetData().Map.seed == -1
            ? (int)DateTime.Now.Ticks
            : SketchFleets.ProfileSystem.Profile.GetData().Map.seed;
        Random.InitState(seed);
        SketchFleets.ProfileSystem.Profile.GetData().Map.seed = seed;

        MapLevelInteraction.map = this;

        GenerateConstellationLayout();

        scrollRect.verticalNormalizedPosition = 0.5f;
        scrollRect.horizontalNormalizedPosition = 0f;

        MapLevelInteraction.state.Open(MapLevelInteraction.state.GetCurrentStar());
        MapLevelInteraction.state.Init();

        onMapLoad?.Invoke();
    }

    /// <summary>
    /// Parallax camera drift, zoom bounds, node idle motion, and path line updates.
    /// </summary>
    private void Update()
    {
        Camera.main.transform.position += new Vector3(parallaxSpeed, 0, 0) * Time.deltaTime;

        RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
        float fitZoom = scrollRectTransform.rect.width / mapWidth;
        float z = Mathf.Min(fitZoom, 1f);
        zoom.SetMinZoom(z);

        float cz = z;
        if (mapHeight * cz >= scrollRectTransform.rect.height)
            mapView.sizeDelta = new Vector2(mapView.sizeDelta.x, mapHeight);
        else
            mapView.sizeDelta = new Vector2(mapView.sizeDelta.x,
                mapHeight * (scrollRectTransform.rect.height / (mapHeight * cz)));

        foreach (Constelation.Star star in constelation)
        {
            RectTransform rt = star.Object.GetComponent<RectTransform>();
            float phase = Time.time * Mathf.Deg2Rad * 90f + ((star.Id - 5) % 10) * 10f;
            float wobble = (Mathf.Sin(phase) + 0.75f) * 0.1f;

            PlanetAttributes planetDef = GetPlanetAttributesForStarType(star.Difficulty);
            if (planetDef != null && planetDef.PlanetDifficulty == PlanetDifficulty.Boss)
                rt.sizeDelta = BossIconScaleMultiplier * new Vector2(StarIconBaseSize, StarIconBaseSize) *
                    (star.scale + wobble);
            else
                rt.sizeDelta = new Vector2(StarIconBaseSize, StarIconBaseSize) * (star.scale +
                    (Mathf.Sin(Time.time * Mathf.Deg2Rad * 60f + ((star.Id - 5) % 10) * 10f) + 0.75f) * 0.1f);

            int seedMix = SketchFleets.ProfileSystem.Profile.GetData().Map.seed;
            float drift = (((seedMix * star.Id) << (star.Id % 7)) % 10) - 5;
            rt.anchoredPosition = star.position + new Vector2(
                Mathf.Sin(Time.time * Mathf.Deg2Rad * 90f + ((star.Id - 5) % 10) * 10f),
                Mathf.Cos(Time.time * Mathf.Deg2Rad * 45f + ((star.Id - 5) % 10) * 10f)) * drift;

            foreach (Constelation.StarJunction j in star.toJunctions)
                UpdateLine(j.junction, j.starB.Object, j.starA.Object);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>Planet asset at <paramref name="planetIndex"/> in <see cref="MapAttributes.Planets"/> (same as <see cref="Constelation.Star.Difficulty"/>).</summary>
    public PlanetAttributes GetPlanetAttributesForStarType(int planetIndex)
    {
        PlanetAttributes[] planets = currentMap != null ? currentMap.Planets : null;
        if (planets == null || planetIndex < 0 || planetIndex >= planets.Length)
            return null;
        return planets[planetIndex];
    }

    public void OnClickStar(int starNumber)
    {
        if (!InputEnabled)
            return;

        Constelation.Star star = constelation.GetStar(starNumber);
        MapLevelInteraction.state.Choose(star.Id);
        interaction.OnClickOnMapStar(starNumber);
    }

    public void UnlockNextLevel()
    {
        UnlockNextLevel(MapLevelInteraction.state.GetCurrentStar());
    }

    public void UnlockNextLevel(int starNumber)
    {
        Constelation.Star star = constelation.GetStar(starNumber);
        star.SetMode(Constelation.StarMode.PASSED_THROUGH_SELECTED);

        foreach (Constelation.StarJunction j in star.toJunctions)
            MapLevelInteraction.state.AddToOpenQueue(j.starB.Id);

        var objects = new List<GameObject>();
        foreach (Constelation.StarJunction jc in star.toJunctions)
        {
            if (jc.starA.Object != star.Object)
                objects.Add(jc.starA.Object);
            else
                objects.Add(jc.starB.Object);
        }

        star.SetEnabled(false);
        objects.Add(star.Object);

        FocusInto(objects.ToArray(), 2f, 1f, true, () => OpenStarPaths(starNumber));
    }

    public void OpenInstantly()
    {
        InputEnabled = true;
        for (int i = 0; i < constelation.Count; i++)
        {
            Constelation.Star s = constelation.GetStar(i);
            s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

            foreach (Constelation.StarJunction j in s.toJunctions)
            {
                if (MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) &&
                    MapLevelInteraction.state.IsChoosen(j.starA.Id))
                {
                    RectTransform back = j.junction.GetComponent<RectTransform>();
                    RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();
                    ApplyJunctionProgress(back, prog, 1f);
                }
            }
        }

        Constelation.Star star = constelation.GetStar(MapLevelInteraction.state.GetCurrentStar());
        var objects = new List<GameObject>();
        foreach (Constelation.StarJunction jc in star.toJunctions)
        {
            if (jc.starA.Object != star.Object)
                objects.Add(jc.starA.Object);
            else
                objects.Add(jc.starB.Object);
        }

        objects.Add(star.Object);

        zoom.SetZoomInstantly(1f);
        FocusIntoInstantly(objects.ToArray());
        InputEnabled = true;
    }

    public void OpenStarPaths(int star)
    {
        StartCoroutine(_OpenStarPaths(star));
    }

    public void OpenAnimation(Action callback)
    {
        StartCoroutine(_OpenAnimation(callback));
    }

    public void CloseAnimation(Action callback)
    {
        StartCoroutine(_CloseAnimation(callback));
    }

    public void FocusInto(GameObject target, float time, float targetZoom, bool smooth, Action callback)
    {
        StartCoroutine(_FocusInto(new[] { target }, time, targetZoom, smooth, callback));
    }

    public void FocusInto(GameObject[] target, float time, float targetZoom, bool smooth, Action callback)
    {
        StartCoroutine(_FocusInto(target, time, targetZoom, smooth, callback));
    }

    public void FocusIntoInstantly(GameObject[] target)
    {
        mapView.anchoredPosition = _GetAnchoredPosition(target);
    }

    public void FocusIntoLerp(GameObject star, float t)
    {
        mapView.anchoredPosition = Vector2.Lerp(mapView.anchoredPosition,
            (Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(star.transform.position), t);
    }

    public void FocusIntoLerp(GameObject star, Vector2 start, float t)
    {
        Vector2 target = (Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(star.transform.position);
        mapView.anchoredPosition = start + (target - start) * t;
    }

    public void FocusLerpTarget(Vector2 target, Vector2 start, float t)
    {
        mapView.anchoredPosition = start + (target - start) * t;
    }

    /// <summary>Same as <see cref="OpenStarPaths"/> but without coroutine delay.</summary>
    public void OpenStarPathsInstantly(int star)
    {
        Constelation.Star s = constelation.GetStar(star);
        InputEnabled = false;
        MapLevelInteraction.state.Open(s.Id);

        foreach (Constelation.StarJunction j in s.toJunctions)
        {
            if (!MapLevelInteraction.state.IsChoosen(j.starA.Id))
                continue;

            MapLevelInteraction.state.Open(j.starA.Id);
            MapLevelInteraction.state.Open(j.starB.Id);

            RectTransform back = j.junction.GetComponent<RectTransform>();
            RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();
            ApplyJunctionProgress(back, prog, 1f);
        }

        InputEnabled = true;
    }

    public void ReturnToMenu()
    {
        interaction.ReturnToMenu(this);
    }

    #endregion

    #region Constellation generation

    /// <summary>Builds columns from <see cref="MapAttributes.Planets"/> using spawn ranges; wires paths; fits scroll content.</summary>
    private void GenerateConstellationLayout()
    {
        // Validação inicial do asset
        if (currentMap == null)
        {
            Debug.LogError("ConstelationMap: assign a MapAttributes asset on currentMap.");
            return;
        }

        // --- Configurações locais (extraídas para clareza) ---
        ConstellationMapGenerationConfig generation = currentMap.ConstellationGeneration;
        PlanetAttributes[] planets = currentMap.Planets;

        // Entradas obrigatórias no array de planetas
        PlanetAttributes easy = ConstellationMapPlanetPicker.FindFirst(planets, PlanetDifficulty.Easy);
        PlanetAttributes boss = ConstellationMapPlanetPicker.FindFirst(planets, PlanetDifficulty.Boss);
        PlanetAttributes store = ConstellationMapPlanetPicker.FindFirst(planets, PlanetDifficulty.Store);
        PlanetAttributes mediumFallback = ConstellationMapPlanetPicker.FindFirst(planets, PlanetDifficulty.Medium);

        if (planets == null || planets.Length == 0 || easy == null || boss == null || store == null)
        {
            Debug.LogError("ConstelationMap: MapAttributes.Planets must define at least Easy, Store, and Boss entries.");
            return;
        }

        // Índice do easy (usado como fallback em casos inesperados)
        int easyIndex = ConstellationMapPlanetPicker.IndexOf(planets, easy);
        if (mediumFallback == null)
            mediumFallback = easy; // garante fallback

        // Extração de variáveis de geração para legibilidade
        int columns = generation.columns;
        float spaceBetweenY = generation.spaceBetweenY;
        float spaceBetweenX = generation.spaceBetweenX;
        float xMaxRandom = generation.randomOffsetX;
        float yMaxRandom = generation.randomOffsetY;
        float maxMultiplier = generation.columnWidthMultiplierMax;
        float minMultiplier = generation.columnWidthMultiplierMin;
        int endWithMax = generation.endColumnNodes;
        int startWith = generation.startColumnNodes;
        float margin = generation.margin;
        float itemHalfSize = generation.itemHalfSize;

        // Estado temporário para ajustar layout
        var lastLineStars = new List<Constelation.Star>();
        float sizeX = 0f;
        float minY = 0f;
        float maxY = 0f;

        // Garantia: pelo menos uma loja antes da coluna do boss
        bool storePlacedInBossPreColumn = false;

        // --- Loop por colunas ---
        for (int col = 0; col < columns; col++)
        {
            constelation.NewColumn();
            var currentLineStars = new List<Constelation.Star>();

            // Contador de quantas vezes cada planeta foi usado nesta coluna (limite de duplicatas)
            var usedCountsInColumn = new Dictionary<PlanetAttributes, int>();

            // Progressão horizontal do mapa em % (0..100)
            float mapPercent = ConstellationMapPlanetPicker.ColumnToMapPercent(col, columns);

            // Determina quantos nós (estrelas) terá esta coluna
            int count;
            if (col == 0)
            {
                // Coluna inicial: sempre conter pelo menos startWith nós
                count = Mathf.Max(1, startWith);
            }
            else if (col == columns - 1)
            {
                // Coluna final: apenas o boss
                count = 1;
            }
            else
            {
                // Heurística: largura da coluna baseada na coluna anterior com alguma aleatoriedade
                float estimate = (col <= columns / 2 + endWithMax)
                    ? lastLineStars.Count * (Random.value * (maxMultiplier - minMultiplier) + minMultiplier)
                    : lastLineStars.Count / (Random.value * (maxMultiplier - minMultiplier) + minMultiplier);

                estimate = Mathf.Min(generation.maxNodesPerColumn, estimate);
                if (col > 0)
                    estimate = Mathf.Max(Mathf.Min(estimate, lastLineStars.Count + 1), 1);

                count = (Random.value * 10f < 5f) ? Mathf.RoundToInt(estimate) : Mathf.CeilToInt(estimate);
                count = Mathf.Max(count, 1);
            }

            // Decide se haverá uma loja nesta coluna (exclui primeira e última coluna)
            int storeRow = -1;
            // Only attempt to place a store in this column if the store asset allows spawning here
            float columnMapPercent = ConstellationMapPlanetPicker.ColumnToMapPercent(col, columns);
            bool storeAllowedHere = ConstellationMapPlanetPicker.IsPercentInSpawnRange(columnMapPercent, store.SpawnOnMapRangePercent);
            if (col > 0 && col < columns - 1 && storeAllowedHere)
            {
                // Chance base para loja (pode ser ajustada ou exposta)
                const float baseStoreChance = 0.15f;

                // Se já não garantimos uma loja e estamos na penúltima coluna, aumentamos chance
                float effectiveChance = (col == columns - 2 && !storePlacedInBossPreColumn)
                    ? 0.7f // torna muito provável
                    : baseStoreChance;

                if (Random.value < effectiveChance)
                {
                    storeRow = Random.Range(0, count);
                    if (col == columns - 2)
                        storePlacedInBossPreColumn = true;
                }
            }

            // Altura para centralizar os nós da coluna
            float height = (count - 1) * spaceBetweenY;

            // --- Loop por linhas (nós) na coluna ---
            for (int row = 0; row < count; row++)
            {
                // Pequeno jitter para posição (exceto colunas de início/fim)
                Vector2 jitter = (col == 0 || col == columns - 1)
                    ? Vector2.zero
                    : new Vector2(Random.value * xMaxRandom - xMaxRandom / 2f,
                        Random.value * yMaxRandom - yMaxRandom / 2f);

                Vector2 pos = new Vector2(itemHalfSize + margin / 2f, -margin / 2f)
                    + new Vector2(col * spaceBetweenX, row * spaceBetweenY)
                    + jitter
                    - new Vector2(0f, height / 2f);

                GameObject o = CreatePoint(pos);

                // Escolha do planeta (atributo) para este nó
                PlanetAttributes chosen;
                if (col == 0)
                {
                    // Sempre easy na primeira coluna
                    chosen = easy;
                }
                else if (col == columns - 1)
                {
                    // Última coluna: boss
                    chosen = boss;
                }
                else if (row == storeRow)
                {
                    // Loja escolhida explicitamente para esta célula
                    chosen = store;
                }
                else
                {
                    // Seleção procedimental respeitando spawn ranges, pesos e duplicatas
                    chosen = ConstellationMapPlanetPicker.PickCombatForColumn(
                        planets,
                        mapPercent,
                        mediumFallback,
                        col,
                        columns,
                        currentMap,
                        count,
                        usedCountsInColumn
                    );

                    // Nota: a lógica de Special (chance de spawn) foi centralizada em
                    // ConstellationMapPlanetPicker.PickCombatForColumn para evitar
                    // verificações duplicadas aqui.
                }

                // Resolve índice (usado no visual e no armazenamento do star)
                int planetIndex = ConstellationMapPlanetPicker.IndexOf(planets, chosen);
                if (planetIndex < 0)
                    planetIndex = easyIndex;

                // Exibe índice (debug / visualização do mapa)
                o.transform.GetChild(0).GetComponent<Text>().text = planetIndex.ToString();

                // Cria a estrela na constelação
                var star = new Constelation.Star(o, planetIndex, this);
                currentLineStars.Add(star);
                constelation.AddStar(star);

                // Conta uso para limitar duplicatas (máx ~ 2/3 do número de nós da coluna)
                if (chosen != null)
                {
                    usedCountsInColumn[chosen] = usedCountsInColumn.ContainsKey(chosen)
                        ? usedCountsInColumn[chosen] + 1
                        : 1;
                }

                // Atualiza bounds do mapa
                RectTransform curRt = o.GetComponent<RectTransform>();
                if (curRt.anchoredPosition.x > sizeX)
                    sizeX = curRt.anchoredPosition.x;
                if (curRt.anchoredPosition.y < minY)
                    minY = curRt.anchoredPosition.y;
                if (curRt.anchoredPosition.y > maxY)
                    maxY = curRt.anchoredPosition.y;
            }

            // Conecta coluna atual com a anterior (cria junções)
            if (col > 0)
                ConnectColumnToPrevious(col, columns, currentLineStars, lastLineStars);

            // Prepara para próxima iteração
            lastLineStars.Clear();
            lastLineStars.AddRange(currentLineStars);
        }

        // Se ainda não garantimos uma loja antes do boss, tentamos forçar uma numa coluna
        // que respeite o range de spawn do store. Procuramos preferencialmente na penúltima
        // coluna, caso contrário buscarmos da direita para a esquerda (excluindo a coluna 0).
        if (!storePlacedInBossPreColumn && columns > 1)
        {
            int penultimate = columns - 2;
            int chosenColumn = -1;

            // Helper local para checar se o store permite spawn na coluna
            bool StoreAllowsColumn(int columnIndex)
            {
                float mp = ConstellationMapPlanetPicker.ColumnToMapPercent(columnIndex, columns);
                return ConstellationMapPlanetPicker.IsPercentInSpawnRange(mp, store.SpawnOnMapRangePercent);
            }

            if (penultimate >= 0 && StoreAllowsColumn(penultimate) && constelation.GetColumnStarCount(penultimate) > 0)
            {
                chosenColumn = penultimate;
            }
            else
            {
                for (int c = penultimate; c >= 1; c--)
                {
                    if (!StoreAllowsColumn(c))
                        continue;
                    if (constelation.GetColumnStarCount(c) > 0)
                    {
                        chosenColumn = c;
                        break;
                    }
                }
            }

            if (chosenColumn >= 0)
            {
                Constelation.Star starToChange = constelation.GetStar(chosenColumn, Random.Range(0, constelation.GetColumnStarCount(chosenColumn)));
                starToChange.Difficulty = ConstellationMapPlanetPicker.IndexOf(planets, store);
                starToChange.Object.transform.GetChild(0).GetComponent<Text>().text = starToChange.Difficulty.ToString();
            }
        }

        // Ajusta tamanho do view e offsets finais
        float h = (maxY - minY) + itemHalfSize * 2f;
        foreach (Transform t in mapView)
            t.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, -itemHalfSize);

        mapView.sizeDelta = new Vector2(sizeX + itemHalfSize + margin, mapHeight = h + margin);
        mapWidth = sizeX + itemHalfSize + margin;
    }

    private void ConnectColumnToPrevious(int columnIndex, int columns, List<Constelation.Star> currentLineStars,
        List<Constelation.Star> lastLineStars)
    {
        for (int j = 0; j < currentLineStars.Count; j++)
        {
            int a = j;
            int b = j - 1;
            int c = j + 1;
            if (b < 0)
                b = j + 2;

            Constelation.Star starA = currentLineStars[j];

            if (a >= 0 && a < lastLineStars.Count)
                AddJunction(starA, lastLineStars[a], columnIndex, columns);
            if (c >= 0 && c < lastLineStars.Count)
                AddJunction(starA, lastLineStars[c], columnIndex, columns);
            if (b >= 0 && b < lastLineStars.Count)
                AddJunction(starA, lastLineStars[b], columnIndex, columns);
        }
    }

    private void AddJunction(Constelation.Star starA, Constelation.Star starB, int columnIndex, int columns)
    {
        GameObject junction = CreateLine(starA.Object, starB.Object);
        var junc = new Constelation.StarJunction(starB, starA, junction);
        starA.fromJunctions.Add(junc);
        starB.toJunctions.Add(junc);

        // Removed the logic that replaces store planets with combat planets.
        // Store placement is now handled exclusively in GenerateConstellationLayout.
    }

    #endregion

    #region Private Methods

    private static void ApplyJunctionProgress(RectTransform back, RectTransform prog, float t)
    {
        float p = JunctionPathDiscardPixels + (back.sizeDelta.x - JunctionPathDiscardPixels * 2f) * t;
        prog.sizeDelta = new Vector2(p, prog.sizeDelta.y);
    }

    private IEnumerator _OpenStarPaths(int star)
    {
        Constelation.Star s = constelation.GetStar(star);
        InputEnabled = false;
        MapLevelInteraction.state.Open(s.Id);
        s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

        float time = Time.time;
        while (true)
        {
            float progTime = (Time.time - time) * 2f;
            if (progTime >= 1f)
                break;

            foreach (Constelation.StarJunction j in s.toJunctions)
            {
                if (!MapLevelInteraction.state.IsChoosen(j.starA.Id))
                    continue;

                RectTransform back = j.junction.GetComponent<RectTransform>();
                RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();
                ApplyJunctionProgress(back, prog, progTime);
            }

            yield return new WaitForEndOfFrame();
        }

        foreach (Constelation.StarJunction j in s.toJunctions)
        {
            if (!MapLevelInteraction.state.IsChoosen(j.starA.Id))
                continue;

            MapLevelInteraction.state.Open(j.starA.Id);
            MapLevelInteraction.state.Open(j.starB.Id);

            RectTransform back = j.junction.GetComponent<RectTransform>();
            RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();
            ApplyJunctionProgress(back, prog, 1f);
        }

        InputEnabled = true;
    }

    private Vector2 _GetAnchoredPosition(GameObject[] star)
    {
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < star.Length; i++)
        {
            sum += (Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(star[i].transform.position);
        }

        return sum / star.Length;
    }

    private IEnumerator _OpenAnimation(Action callback)
    {
        float time = Time.time;
        int lastcolumn = 0;
        InputEnabled = false;

        while (true)
        {
            float progTime = (Time.time - time) * 4f;
            int column = Mathf.FloorToInt(progTime);
            float curProgress = progTime - Mathf.FloorToInt(progTime);

            if (lastcolumn != column)
            {
                for (int i = 0; i < constelation.GetColumnStarCount(lastcolumn); i++)
                {
                    Constelation.Star s = constelation.GetStar(lastcolumn, i);
                    s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

                    foreach (Constelation.StarJunction j in s.toJunctions)
                    {
                        if (MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) &&
                            MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        {
                            RectTransform back = j.junction.GetComponent<RectTransform>();
                            RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();
                            ApplyJunctionProgress(back, prog, 1f);
                        }
                    }
                }
            }

            lastcolumn = column;

            if (column < constelation.Columns)
            {
                for (int i = 0; i < constelation.GetColumnStarCount(column); i++)
                {
                    Constelation.Star s = constelation.GetStar(column, i);
                    s.Object.transform.localScale = Vector3.one * curve.Evaluate(curProgress);

                    foreach (Constelation.StarJunction j in s.toJunctions)
                    {
                        if (MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) &&
                            MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        {
                            RectTransform back = j.junction.GetComponent<RectTransform>();
                            RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();
                            ApplyJunctionProgress(back, prog, curProgress);
                        }
                    }
                }

                float progress = progTime / constelation.Columns;
                float needed = scrollRect.GetComponent<RectTransform>().rect.width / mapWidth;
                zoom.SetCurrentZoom(progress == 0
                    ? 1
                    : Mathf.Min(1f, needed * 1 / Mathf.Clamp01(progress + 0.1f / (mapWidth / 2000f))));
            }
            else
                break;

            yield return new WaitForEndOfFrame();
        }

        generationAnimationOver.Invoke();
        yield return new WaitForSeconds(0.5f);
        FocusInto(constelation.GetStar(MapLevelInteraction.state.GetCurrentStar()).Object, 2f, 2f, false, null);
        callback?.Invoke();
    }

    private IEnumerator _CloseAnimation(Action callback)
    {
        float time = Time.time;
        float startZoom = zoom.GetCurrentZoom();
        var imagesToFade = new List<Image>();

        for (int i = 0; i < constelation.Count; i++)
        {
            Constelation.Star s = constelation.GetStar(i);
            imagesToFade.Add(s.Object.GetComponent<Image>());
            GameObject sa = s.Object.transform.GetChild(1).gameObject;
            GameObject sb = s.Object.transform.GetChild(2).gameObject;

            if (sa.activeInHierarchy)
                imagesToFade.Add(sa.GetComponent<Image>());
            if (sb.activeInHierarchy)
                imagesToFade.Add(sb.GetComponent<Image>());

            foreach (Constelation.StarJunction j in s.toJunctions)
            {
                if (MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) &&
                    MapLevelInteraction.state.IsChoosen(j.starA.Id))
                    imagesToFade.Add(j.junction.transform.GetChild(0).GetComponent<Image>());
            }
        }

        InputEnabled = false;
        while (true)
        {
            float progT = Mathf.Clamp01(Time.time - time);
            Color c = new Color(1, 1, 1, 1 - progT);
            foreach (Image img in imagesToFade)
                img.color = c;

            zoom.SetZoomInstantly(Mathf.Lerp(startZoom, zoom.GetMinZoom(), Mathf.Clamp01(progT * 0.15f)));

            if (progT >= 1f)
                break;
            yield return new WaitForEndOfFrame();
        }

        InputEnabled = true;
        callback?.Invoke();
    }

    private IEnumerator _FocusInto(GameObject[] target, float time, float targetZoom, bool smooth, Action callback)
    {
        Vector2 cur = mapView.anchoredPosition;
        float f = Time.time;
        float startZoomLevel = zoom.GetCurrentZoom();
        float tm = Mathf.Sqrt(time);

        zoom.SetZoomInstantly(targetZoom);
        FocusIntoInstantly(target);
        scrollRect.PublicUpdateBounds();

        Vector2 targetPos = mapView.anchoredPosition;
        zoom.SetZoomInstantly(startZoomLevel);
        mapView.anchoredPosition = cur;
        scrollRect.PublicUpdateBounds();

        float distance = Vector2.Distance(targetPos / targetZoom, cur / startZoomLevel);
        float scaleFactor = smooth ? Mathf.Clamp01((distance - 200f) / 200f) : 0;

        if (Vector2.Distance(cur, _GetAnchoredPosition(target)) < 5 && Mathf.Abs(startZoomLevel - targetZoom) < 0.05f)
            yield return new WaitForSeconds(1f);
        else
        {
            while (true)
            {
                float pog = Mathf.Clamp01((Time.time - f) / time);
                float rs = 1f - (time - (pog * tm) * (pog * tm)) / time;

                FocusLerpTarget(targetPos, cur, smooth ? focusCurve.Evaluate(rs) : rs);
                zoom.SetZoomInstantly(startZoomLevel +
                    (targetZoom - startZoomLevel - focusCurveScale.Evaluate(rs) * targetZoom / 2f * scaleFactor) *
                    focusCurveScaleProgress.Evaluate(rs));

                InputEnabled = false;
                yield return new WaitForEndOfFrame();
                if (Time.time - f > time)
                    break;
            }
        }

        zoomAnimationOver.Invoke();
        InputEnabled = true;
        callback?.Invoke();
    }

    private GameObject CreatePoint(Vector2 position)
    {
        GameObject cube = Instantiate(mapPrefab);
        cube.transform.SetParent(mapView.transform);
        cube.GetComponent<RectTransform>().anchoredPosition = new Vector3(position.x, position.y, 0);
        cube.SetActive(true);
        return cube;
    }

    private GameObject CreateLine(GameObject pointA, GameObject pointB)
    {
        GameObject path = Instantiate(pathPrefab);
        path.transform.SetParent(mapView.transform);
        path.transform.localPosition = (pointA.transform.localPosition + pointB.transform.localPosition) / 2f;
        path.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Vector2.Distance(pointA.transform.localPosition, pointB.transform.localPosition), 10);
        path.transform.localEulerAngles = new Vector3(0, 0, GetAngleBetween(pointA, pointB));
        path.transform.SetParent(pathHolder.transform);
        path.SetActive(true);
        return path;
    }

    private void UpdateLine(GameObject path, GameObject pointA, GameObject pointB)
    {
        path.transform.localPosition =
            (pointA.transform.localPosition + pointB.transform.localPosition) / 2f - pathHolder.transform.localPosition;
        path.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Vector2.Distance(pointA.transform.localPosition, pointB.transform.localPosition), 10);
        path.transform.localEulerAngles = new Vector3(0, 0, GetAngleBetween(pointA, pointB));
    }

    private static float GetAngleBetween(GameObject a, GameObject b)
    {
        Vector3 dir = b.transform.position - a.transform.position;
        dir = b.transform.InverseTransformDirection(dir);
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    #endregion
}
