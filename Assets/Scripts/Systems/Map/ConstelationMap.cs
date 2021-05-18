using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Variables;
using SketchFleets.Data;

/// <summary>
/// Main map class (Display Constelation | MapLevelInteraction.state)
/// </summary>
public class ConstelationMap : MonoBehaviour
{
    #region Private Fields
    //Internal input enabled
    private bool inputEnabled = true;
    //Holds generate map size
    private float mapWidth = 0;
    private float mapHeight = 0;
    //Holds all stars
    private Constelation constelation;
    #endregion

    #region Public Fields
    //Prefabs
    public GameObject mapPrefab;
    public GameObject pathPrefab;
    //Objects
    public GameObject pathHolder;
    public RectTransform mapView;
    public MapScrollRect scrollRect;
    public ZoomComponent zoom;
    //Overlay input disable panel
    public GameObject mapDisabler;
    //Animaton curves
    public AnimationCurve curve;
    public AnimationCurve focusCurve;
    public AnimationCurve focusCurveScale;
    public AnimationCurve focusCurveScaleProgress;
    //Testing
    public static System.Action onMapLoad;
    //Reference
    public DifficultyAttributes currentMap;
    public IntReference currentLevel;
    public IntReference currentLevelDifficulty;
    public IntReference currentSeed;
    //Icons
    public Sprite[] planetIcons;
    //Parallax
    [Range(0f,1000f)]
    public float parallaxSpeed = 5f;
    #endregion

    #region Properties
    //Input enabled property
    public bool InputEnabled
    {
        get { return inputEnabled;}
        set {
            if(inputEnabled != value)
            {
                zoom.inputEnabled = value;
                zoom.horizontal = value;
                zoom.vertical = value;

                if(mapDisabler != null)
                    mapDisabler.SetActive(!value);
                inputEnabled = value;
            }
        }
    }
    #endregion

    #region Unity Callbacks
    /// <summary>
    /// On awake
    /// </summary>
    private void Awake()
    {
        //Set map
        MapLevelInteraction.map = this;
    }
    
    /// <summary>
    /// Generate map and play animation
    /// </summary>
    private void Start() 
    {
        //Create new constelation
        constelation = new Constelation(this);

        //Init constelation state
        MapLevelInteraction.state.SetConstelation(constelation);
        
        int seed = MapLevelInteraction.state.seed == 0 ? (int)System.DateTime.Now.Ticks : MapLevelInteraction.state.seed;
        //Current map system - Generate from seed
        Random.InitState(seed);
        MapLevelInteraction.state.seed = seed;
        
        //Set map
        MapLevelInteraction.map = this;
        
        //Map columns count
        int columns = 12;
        //Main levels per line
        int maxPerLine = 5;
        //Default space between levels
        float spaceBetweenY = 100;
        float spaceBetweenX = 180;
        //Map Randomizer
        float XMaxRandom = 50;
        float YMaxRandom = 50;
        //Map size randomizer
        float maxMultiplier = 1.5f;
        float minMultipler = 1.25f;
        //End with max
        int endWithMax = 1; //Always end with 1
        int startWith = 1;

        //Holds last line objects
        List<Constelation.Star> lastLineStars = new List<Constelation.Star>();

        //Map bounds
        float margin = 50;
        float itemHalfSize = 25;
        float sizeX = 0;
        float minY = 0;
        float maxY = 0;

        for(int i = 0; i < columns; i ++)
        {
            //Start new constelation column
            constelation.NewColumn();

            //Holds current line objects
            List<Constelation.Star> currentLineStars = new List<Constelation.Star>();

            //Levels in current column
            float current = (i == 0 ? startWith : Mathf.Min(maxPerLine,   
                    i <= (columns/2 + (endWithMax)) ? lastLineStars.Count * (Random.value * (maxMultiplier - minMultipler) + minMultipler):
                    lastLineStars.Count / (Random.value * (maxMultiplier - minMultipler) + minMultipler)
                ));

            //End with target amount
            if( i == columns - 1)
                current = Mathf.Min(current,endWithMax);

            //Limit current between [1,lastLineStars.Count + 1]
            if(i > 0)
                current = Mathf.Max(Mathf.Min(current,lastLineStars.Count + 1),1);

            //Count of current line and height
            int count = (Random.value * 10) < 5 ? Mathf.RoundToInt(current) : Mathf.CeilToInt(current);
            float height = (count - 1) * spaceBetweenY; 
 
            //Create constelation points
            for(int j = 0; j < count; j ++)
            {
                GameObject cur;
                GameObject o = (cur = CreatePoint(new Vector2(itemHalfSize + margin/2,-margin/2) + new Vector2(i * spaceBetweenX,j * spaceBetweenY) + ( i == 0 || i == columns - 1 ? Vector2.zero :
                new Vector2(
                    (Random.value * XMaxRandom) - XMaxRandom/2,
                    (Random.value * YMaxRandom) - YMaxRandom/2
                )) - new Vector2(0,height/2)));

                
                //Create start difficulty
                int difficulty = 0;

                if(i == columns - 1)
                    difficulty = 5; //Boss
                else if(i == columns - 2)
                    difficulty = 0; //Shop
                else
                {   
                    if(Random.Range(0,10) <= 8)
                        difficulty = Random.Range(Mathf.Max(1,i/3),Mathf.Max(2,Mathf.Min(5,i + 1))); //Random difficulty
                }

                //Temp fix kkkk
                if(difficulty < 1)
                    difficulty = 1;
                if(difficulty == 4)
                    difficulty = Random.Range(1,3);

                //o.transform.GetChild(0).GetComponent<Text>().text = constelation.Count + "";
                o.transform.GetChild(0).GetComponent<Text>().text = difficulty + "";

                //Add star
                Constelation.Star s = new Constelation.Star(o,difficulty,Random.Range(0.8f,1.25f));
                currentLineStars.Add(s);
                constelation.AddStar(s);

                //Update bounds
                if(cur.GetComponent<RectTransform>().anchoredPosition.x > sizeX)
                    sizeX = cur.GetComponent<RectTransform>().anchoredPosition.x;

                if(cur.GetComponent<RectTransform>().anchoredPosition.y < minY)
                    minY = cur.GetComponent<RectTransform>().anchoredPosition.y;

                if(cur.GetComponent<RectTransform>().anchoredPosition.y > maxY)
                    maxY = cur.GetComponent<RectTransform>().anchoredPosition.y;
            }

            if(i > 0)
            {
                //Create paths between stars
                for(int j = 0; j < currentLineStars.Count; j ++)
                {

                    int a = j;
                    int b = j - 1;
                    int c = j + 1;

                    Constelation.Star starA = currentLineStars[j];

                    if(a >= 0 && a < lastLineStars.Count)
                    {
                        Constelation.Star starB = lastLineStars[a];
                        GameObject junction = CreateLine(starA.Object,starB.Object);

                        Constelation.StarJunction junc = new Constelation.StarJunction(starB,starA,junction);

                        starA.fromJunctions.Add(junc);
                        starB.toJunctions.Add(junc);

                        //Change difficulty if two shops are conected
                        if(starB.Difficulty == 0 && starA.Difficulty == 0)
                        {
                            starB.Difficulty = Random.Range(1,Mathf.Max(2,Mathf.Min(5,i + 1)));
                        }
                    }
                    if(c >= 0 && c < lastLineStars.Count)
                    {
                        Constelation.Star starB = lastLineStars[c];
                        GameObject junction = CreateLine(starA.Object,starB.Object);

                        Constelation.StarJunction junc = new Constelation.StarJunction(starB,starA,junction);

                        starA.fromJunctions.Add(junc);
                        starB.toJunctions.Add(junc);

                        //Change difficulty if two shops are conected
                        if(starB.Difficulty == 0 && starA.Difficulty == 0)
                        {
                            starB.Difficulty = Random.Range(1,Mathf.Max(2,Mathf.Min(5,i + 1)));
                        }
                    }
                    if(b >= 0 && b < lastLineStars.Count)
                    {
                        Constelation.Star starB = lastLineStars[b];
                        GameObject junction = CreateLine(starA.Object,starB.Object);

                        Constelation.StarJunction junc = new Constelation.StarJunction(starB,starA,junction);

                        starA.fromJunctions.Add(junc);
                        starB.toJunctions.Add(junc);

                        //Change difficulty if two shops are conected
                        if(starB.Difficulty == 0 && starA.Difficulty == 0)
                        {
                            starB.Difficulty = Random.Range(1,Mathf.Max(2,Mathf.Min(5,i + 1)));
                        }
                    }
                }
            }

            //Clone stars
            lastLineStars.Clear();
            lastLineStars.AddRange(currentLineStars);
        }

        //Current map height
        float h = (maxY - minY) + itemHalfSize * 2f;

        //Centralize items
        foreach(Transform o in this.mapView)
        {
            o.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0,-itemHalfSize);
        }

        //Recalculate size
        mapView.sizeDelta = new Vector2(sizeX + itemHalfSize + margin,mapHeight = (h + margin));
        mapWidth = sizeX + itemHalfSize + margin;

        //Set view rect to the start
        scrollRect.verticalNormalizedPosition = 0.5f;
        scrollRect.horizontalNormalizedPosition = 0f;

        //Play Open Animation
        //OpenAnimation(null);
        //OpenInstantly();

        //Open first
        MapLevelInteraction.state.Open(MapLevelInteraction.state.GetCurrentStar());

        //Init current state
        MapLevelInteraction.state.Init();

        if(onMapLoad != null)
            onMapLoad();
    }

    /// <summary>
    /// Update map and animations
    /// </summary>
    private void Update() 
    {
        //Camera PARALLAX
        Camera.main.transform.position += new Vector3(parallaxSpeed,0,0) * Time.deltaTime;

        //Update minimum zoom possible
        RectTransform scrollRect = this.scrollRect.GetComponent<RectTransform>();     
        float zoom = scrollRect.rect.width/mapWidth;
        float z = Mathf.Min(zoom,1f);
        this.zoom.SetMinZoom(z);

        // float cz = Zoom.GetCurrentZoom(); //Adds an strange view behaviour
        float cz = z;
        
        //Update needed height for current zoom
        if(mapHeight * cz >= scrollRect.rect.height)
            mapView.sizeDelta = new Vector2(mapView.sizeDelta.x,mapHeight);
        else
            mapView.sizeDelta = new Vector2(mapView.sizeDelta.x,mapHeight * 
            (scrollRect.rect.height/(mapHeight * cz)));

        //Update scales
        foreach(Constelation.Star star in constelation)
        {
            star.Object.GetComponent<RectTransform>().sizeDelta = new Vector2(50,50) * (star.scale + (Mathf.Sin(Time.time * Mathf.Deg2Rad * 90 + ((star.Id - 5)%10) * 10) + 0.75f) * 0.2f);
        }
    }
    #endregion  

    #region Public Methods
    /// <summary>
    /// On click on a star (Temporary)
    /// </summary>
    /// <param name="starNumber">Get star from index</param>
    public void OnClickStar(int starNumber)
    {
        if(!InputEnabled)
            return;

        //Choose current star
        Constelation.Star star = constelation.GetStar(starNumber);
        MapLevelInteraction.state.Choose(star.Id); 
        MapLevelInteraction.OnClickOnMapStar(starNumber);
    }

    public void UnlockNextLevel()
    {
        UnlockNextLevel(MapLevelInteraction.state.GetCurrentStar());
    }

    public void UnlockNextLevel(int starNumber)
    {
        Constelation.Star star = constelation.GetStar(starNumber);

        //Add to open queue
        foreach(Constelation.StarJunction j in star.toJunctions)
        {
            MapLevelInteraction.state.AddToOpenQueue(j.starB.Id);
        }

        //Sum all positions
        List<GameObject> objects = new List<GameObject>();

        foreach(Constelation.StarJunction jc in star.toJunctions)
        {
            if(jc.starA.Object != star.Object)
                objects.Add(jc.starA.Object);
            else
                objects.Add(jc.starB.Object);
        }

        //Disable current
        star.SetEnabled(false);

        //Add star to sum
        objects.Add(star.Object);

        FocusInto(objects.ToArray(),2f,1f,true,() => {
            OpenStarPaths(starNumber);
        });
    }

    public void OpenInstantly()
    {
        InputEnabled = true;
        for(int i = 0; i < constelation.Count; i ++)
        {
            Constelation.Star s = constelation.GetStar(i);
            s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

            foreach(Constelation.StarJunction j in s.toJunctions)
            {
                if(MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) && MapLevelInteraction.state.IsChoosen(j.starA.Id))
                {
                    RectTransform back = j.junction.GetComponent<RectTransform>();
                    RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                    int discard = 10; //Amount to discard from each side

                    float p = discard + (back.sizeDelta.x - discard * 2) * 1;
                    prog.sizeDelta = new Vector2(p,prog.sizeDelta.y); 
                }         
            }
        }

        //Sum all positions
        Constelation.Star star = constelation.GetStar(MapLevelInteraction.state.GetCurrentStar());
        List<GameObject> objects = new List<GameObject>();

        foreach(Constelation.StarJunction jc in star.toJunctions)
        {
            if(jc.starA.Object != star.Object)
                objects.Add(jc.starA.Object);
            else
                objects.Add(jc.starB.Object);
        }

        //Add star to sum
        objects.Add(star.Object);

        //Zoom and focus star
        zoom.SetZoomInstantly(1f);
        FocusIntoInstantly(objects.ToArray());
        InputEnabled = true;
    }

    /// <summary>
    /// Play open star paths animation for a star
    /// </summary>
    /// <param name="star">Open paths of star</param>
    public void OpenStarPaths(int star)
    {   
        StartCoroutine(_OpenStarPaths(star));
    }

    /// <summary>
    /// Open map screen animation
    /// </summary>
    /// <param name="callback">On end callback</param>
    public void OpenAnimation(System.Action callback)
    {
        StartCoroutine(_OpenAnimation(callback));
    }

    
    /// <summary>
    /// Close map screen animation
    /// </summary>
    /// <param name="callback">On end callback</param>
    public void CloseAnimation(System.Action callback)
    {
        StartCoroutine(_CloseAnimation(callback));
    }

    /// <summary>
    /// Focus view into a single object
    /// </summary>
    /// <param name="target">Target object</param>
    /// <param name="time">Duration time</param>
    /// <param name="targetZoom">Zoom level target</param>
    /// <param name="smooth">Switch smooth curves option</param>
    /// <param name="callback">On end callback</param>
    public void FocusInto(GameObject target,float time,float targetZoom,bool smooth,System.Action callback)
    {
        StartCoroutine(_FocusInto(new GameObject[]{target},time,targetZoom,smooth,callback));
    }

    /// <summary>
    /// Focus view into center of a group of targets
    /// </summary>
    /// <param name="target">Multiple targets object to calculate center point</param>
    /// <param name="time">Duration time</param>
    /// <param name="targetZoom">Zoom level target</param>
    /// <param name="smooth">Switch smooth curves option</param>
    /// <param name="callback">On end callback</param>
    public void FocusInto(GameObject[] target,float time,float targetZoom,bool smooth,System.Action callback)
    {
        StartCoroutine(_FocusInto(target,time,targetZoom,smooth,callback));
    }

    /// <summary>
    /// Focus into objects instantly
    /// </summary>
    /// <param name="star">Objects to focus</param>
    public void FocusIntoInstantly(GameObject[] target)
    {
        Vector2 sum = Vector2.zero;
        for(int i = 0; i < target.Length; i ++)
        {
            sum += (Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target[i].transform.position);;
        }

        mapView.anchoredPosition = sum/target.Length;
    }

    /// <summary>
    /// Focus into object with easing
    /// </summary>
    /// <param name="star">Single object to focus</param>
    /// /// <param name="t">Lerp factor</param>
    public void FocusIntoLerp(GameObject star,float t)
    {
       mapView.anchoredPosition = Vector2.Lerp(mapView.anchoredPosition,(Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(star.transform.position),t);
            
    }

    /// <summary>
    /// Focus into object with fixed lerp
    /// </summary>
    /// <param name="star">Single object to focus</param>
    /// <param name="start">Starting anchored position</param>
    /// <param name="t">Lerp factor</param>
    public void FocusIntoLerp(GameObject star,Vector2 start,float t)
    {
        Vector2 target = (Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(star.transform.position);
        
       mapView.anchoredPosition = start + (target - start) * t;
    }

    /// <summary>
    /// Lerp to target anchored position
    /// </summary>
    /// <param name="target">Target anchored position</param>
    /// <param name="start">Starting anchored position</param>
    /// <param name="t">Lerp factor</param>
    public void FocusLerpTarget(Vector2 target,Vector2 start,float t)
    {
       mapView.anchoredPosition = start + (target - start) * t;
    }
    #endregion

    #region Private Method
    
    /// <summary>
    /// Internal play open star paths animation for a star
    /// </summary>
    /// <param name="star">Star index</param>
    /// <returns></returns>
    private IEnumerator _OpenStarPaths(int star)
    {
        Constelation.Star s = constelation.GetStar(star);
        {
            InputEnabled = false;

            //Guarantee
            MapLevelInteraction.state.Open(s.Id);

            s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

            float time = Time.time;

            while(true)
            {
                float progTime = (Time.time - time) * 2f;

                if(progTime >= 1f)
                    break;

                foreach(Constelation.StarJunction j in s.toJunctions)
                {       
                    if(!MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        continue;

                    RectTransform back = j.junction.GetComponent<RectTransform>();
                    RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                    int discard = 10; //Amount to discard from each side

                    float p = discard + (back.sizeDelta.x - discard * 2) * progTime;
                    prog.sizeDelta = new Vector2(p,prog.sizeDelta.y);         
                }
                
                yield return new WaitForEndOfFrame();
            }

            foreach(Constelation.StarJunction j in s.toJunctions)
            {
                if(!MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        continue;

                //Open linked starts
                MapLevelInteraction.state.Open(j.starA.Id);
                MapLevelInteraction.state.Open(j.starB.Id);
          
                RectTransform back = j.junction.GetComponent<RectTransform>();
                RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                int discard = 10; //Amount to discard from each side

                float p = discard + (back.sizeDelta.x - discard * 2) * 1;
                prog.sizeDelta = new Vector2(p,prog.sizeDelta.y);      
            }

            InputEnabled = true;
        }
    }

    /// <summary>
    /// Open start paths instantly
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public void OpenStarPathsIntantly(int star)
    {
        Constelation.Star s = constelation.GetStar(star);
        {
            InputEnabled = false;
            //Guarantee
            MapLevelInteraction.state.Open(s.Id);

            //s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

            foreach(Constelation.StarJunction j in s.toJunctions)
            {
                if(!MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        continue;

                //Open linked starts
                MapLevelInteraction.state.Open(j.starA.Id);
                MapLevelInteraction.state.Open(j.starB.Id);
          
                RectTransform back = j.junction.GetComponent<RectTransform>();
                RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                int discard = 10; //Amount to discard from each side

                float p = discard + (back.sizeDelta.x - discard * 2) * 1;
                prog.sizeDelta = new Vector2(p,prog.sizeDelta.y);      
            }

            InputEnabled = true;
        }
    }

    /// <summary>
    /// Get target anchored position for objects
    /// </summary>
    /// <param name="star">Objects to focus</param>
    /// <returns></returns>
    private Vector2 _GetAnchoredPosition(GameObject[] star)
    {
        Vector2 sum = Vector2.zero;
        for(int i = 0; i < star.Length; i ++)
        {
            sum += (Vector2)scrollRect.transform.InverseTransformPoint(mapView.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(star[i].transform.position);;
        }

        return sum/star.Length;
    }
    

    /// <summary>
    /// Internal open map screen animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator _OpenAnimation(System.Action callback)
    {
        float time = Time.time;

        int lastcolumn = 0;

        InputEnabled = false;
        while(true)
        {
            float progTime = (Time.time - time) * 4f;
            int column = Mathf.FloorToInt(progTime);
            float curProgress = progTime - Mathf.FloorToInt(progTime);
            
            if(lastcolumn != column)
            {
                for(int i = 0; i < constelation.GetColumnStarCount(lastcolumn); i ++)
                {
                    Constelation.Star s = constelation.GetStar(lastcolumn,i);
                    s.Object.transform.localScale = Vector3.one * curve.Evaluate(1);

                    foreach(Constelation.StarJunction j in s.toJunctions)
                    {
                        if(MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) && MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        {
                            RectTransform back = j.junction.GetComponent<RectTransform>();
                            RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                            int discard = 10; //Amount to discard from each side

                            float p = discard + (back.sizeDelta.x - discard * 2) * 1;
                            prog.sizeDelta = new Vector2(p,prog.sizeDelta.y); 
                        }         
                    }
                }
            }

            lastcolumn = column;

            if(column < constelation.Columns)
            {
                for(int i = 0; i < constelation.GetColumnStarCount(column); i ++)
                {
                    Constelation.Star s = constelation.GetStar(column,i);
                    s.Object.transform.localScale = Vector3.one * curve.Evaluate(curProgress);
                    
                    
                    foreach(Constelation.StarJunction j in s.toJunctions)
                    {
                        if(MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) && MapLevelInteraction.state.IsChoosen(j.starA.Id))
                        {
                            RectTransform back = j.junction.GetComponent<RectTransform>();
                            RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                            int discard = 10; //Amount to discard from each side

                            float p = discard + (back.sizeDelta.x - discard * 2) * curProgress;
                            prog.sizeDelta = new Vector2(p,prog.sizeDelta.y);    
                        }      
                    }
                    
                }

                float progress = progTime/constelation.Columns;
                //Remove zoom
                //zoom.SimulateScroll(-0.25f * Time.deltaTime,new Vector2(Screen.width,Screen.height)/2f);
                
                float needed = scrollRect.GetComponent<RectTransform>().rect.width/mapWidth;
                zoom.SetCurrentZoom(progress == 0 ? 1 : Mathf.Min(1f,needed * 1/Mathf.Clamp01(progress + 0.1f/(mapWidth/2000))));
            }
            else
                break;
            
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);

        //Focus into current level
        FocusInto(constelation.GetStar(MapLevelInteraction.state.GetCurrentStar()).Object,2f,2f,false,null);

        if(callback != null)
            callback.Invoke();
    }

    /// <summary>
    /// Internal close map screen animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator _CloseAnimation(System.Action callback)
    {
        float time = Time.time;

        float zoom = this.zoom.GetCurrentZoom();
        
        InputEnabled = false;
        while(true)
        {
            float progT = Mathf.Clamp01(Time.time - time);

            for(int i = 0; i < constelation.Count; i ++)
            {
                Constelation.Star s = constelation.GetStar(i);
                s.Object.transform.localScale = Vector3.one * (1 - progT)/*(1 - curve.Evaluate(progT))*/;

                foreach(Constelation.StarJunction j in s.toJunctions)
                {
                    if(MapLevelInteraction.state.IsOpen(j.starA.Id) && MapLevelInteraction.state.IsOpen(j.starB.Id) && MapLevelInteraction.state.IsChoosen(j.starA.Id))
                    {
                        RectTransform back = j.junction.GetComponent<RectTransform>();
                        RectTransform prog = j.junction.transform.GetChild(0).GetComponent<RectTransform>();

                        int discard = 0; //Amount to discard from each side

                        float p = discard + (back.sizeDelta.x - discard * 2) * (1 - progT);
                        prog.sizeDelta = new Vector2(p,prog.sizeDelta.y); 
                    }         
                }
            }
                
            //Zoom out
            this.zoom.SetZoomInstantly(Mathf.Lerp(zoom,this.zoom.GetMinZoom(),Mathf.Clamp01(progT * 1.5f)));

            if(progT >= 1f)
                break;

            yield return new WaitForEndOfFrame();
        }

        InputEnabled = true;

        if(callback != null)
            callback.Invoke();
    }


    /// <summary>
    /// Internal focus view into a map object
    /// </summary>
    /// <param name="target"></param>
    /// <param name="time"></param>
    /// <param name="targetZoom"></param>
    /// <param name="smooth"></param>
    /// <returns></returns>
    private IEnumerator _FocusInto(GameObject[] target,float time,float targetZoom,bool smooth,System.Action callback)
    {
        //Initial state
        Vector2 cur =  mapView.anchoredPosition;
        float f = Time.time;
        float zoom = this.zoom.GetCurrentZoom();

        //End target zoom
        float tm = Mathf.Sqrt(time);

        //Set to end
        this.zoom.SetZoomInstantly(targetZoom);
        FocusIntoInstantly(target);

        //Force update
        scrollRect.PublicUpdateBounds();

        //Get actual target
        Vector2 targetPos = mapView.anchoredPosition;

        //Reset
        this.zoom.SetZoomInstantly(zoom);
        mapView.anchoredPosition = cur;

        //Force update
        scrollRect.PublicUpdateBounds();

        //Get factor to smooth scale
        float distance = Vector2.Distance(targetPos/targetZoom,cur/zoom);
        float scaleFactor = smooth ? Mathf.Clamp01((distance - 200f)/200f) : 0;

       
        if(Vector2.Distance(cur,_GetAnchoredPosition(target)) < 5 && Mathf.Abs(zoom - targetZoom) < 0.05f)
        {
            yield return new WaitForSeconds(1f);
        }
        else while(true)
        {
            float pog = Mathf.Clamp01((Time.time - f)/time);
            float rs = 1f - ((time - (pog * tm)*(pog * tm)))/time;

            FocusLerpTarget(targetPos,cur,smooth ? focusCurve.Evaluate(rs) : rs);
            this.zoom.SetZoomInstantly(zoom + (targetZoom - zoom - focusCurveScale.Evaluate(rs) * targetZoom/2f * scaleFactor) * focusCurveScaleProgress.Evaluate(rs));

            InputEnabled = false;
            yield return new WaitForEndOfFrame();

            if(Time.time - f > time)
                break;
        }
        
        InputEnabled = true; 

        if(callback != null) 
            callback.Invoke();
    }

    /// <summary>
    /// Create a point in a 2D space
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private GameObject CreatePoint(Vector2 position)
    {
        GameObject cube = GameObject.Instantiate(mapPrefab);
        cube.transform.SetParent(mapView.transform);
        cube.GetComponent<RectTransform>().anchoredPosition = new Vector3(position.x,position.y,0);
        cube.SetActive(true);
        return cube;
    }

    /// <summary>
    /// Crate a line between two objects in a 2D space
    /// </summary>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    private GameObject CreateLine(GameObject pointA,GameObject pointB)
    {
        GameObject path = GameObject.Instantiate(pathPrefab);
        path.transform.SetParent(mapView.transform);
        path.transform.localPosition = (pointA.transform.localPosition + pointB.transform.localPosition)/2f;
        path.GetComponent<RectTransform>().sizeDelta = new Vector2(Vector2.Distance(pointA.transform.localPosition,pointB.transform.localPosition),10);
        path.transform.localEulerAngles = new Vector3(0,0,GetAngleBetween(pointA,pointB));
        
        //GetAngleBetween
        path.transform.SetParent(pathHolder.transform);
        path.SetActive(true);

        return path;
    }

    /// <summary>
    /// Get the angle between two objects in a 2D space
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static float GetAngleBetween(GameObject a,GameObject b)
    {
        Vector3 dir = b.transform.position - a.transform.position;
        dir = b.transform.InverseTransformDirection(dir);
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    #endregion
}
