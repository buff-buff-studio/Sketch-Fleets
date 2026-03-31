using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SketchFleets.Data;
using SketchFleets.Interaction;

/// <summary>
/// Holds a constelation of stars and all path between stars
/// </summary>
public class Constelation : IEnumerable
{
    #region Private Fields
    private List<List<Star>> stars = new List<List<Star>>();
    private List<Star> allStars = new List<Star>();
    private ConstelationMap map;
    private MapLevelInteraction interaction;
    #endregion 

    #region Properties
    /// <summary>
    /// Return total star count
    /// </summary>
    /// <value></value>
    public int Count
    {
        get { return allStars.Count;}
        set {}
    }

    /// <summary>
    /// Return number of columns
    /// </summary>
    /// <value></value>
    public int Columns
    {
        get { return stars.Count; }
        set {}
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Start to construct a new constelation from a map
    /// </summary>
    /// <param name="map"></param>
    /// <param name="interaction"></param>
    public Constelation(ConstelationMap map,MapLevelInteraction interaction)
    {
        this.map = map;
        this.interaction = interaction;
    }
    
    /// <summary>
    /// Create new column
    /// </summary>
    public void NewColumn()
    {
        stars.Add(new List<Star>());
    }

    /// <summary>
    /// Get a star from id
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public Star GetStar(int star)
    {
        return allStars[star];
    }

    /// <summary>
    /// Add a star to current constelation column
    /// </summary>
    /// <param name="star"></param>
    public void AddStar(Star star)
    {
        int id = allStars.Count;
        stars[stars.Count - 1].Add(star);
        allStars.Add(star);
        star.Id = id;

        //Register on click
        star.Object.GetComponent<Button>().onClick.AddListener(() => map.OnClickStar(id));
    }

    /// <summary>
    /// Get count of stars of a column
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public int GetColumnStarCount(int column)
    {     
        return stars[column].Count;
    }

    /// <summary>
    /// Get a star from constelation
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public Star GetStar(int column,int row)
    {
        return stars[column][row];
    }
    #endregion

    #region IEnumerator
    public IEnumerator GetEnumerator()
    {
        foreach(Star s in allStars)
            yield return s;
    }
    #endregion

    /// <summary>
    /// Represents a star in constelation
    /// </summary>
    public class Star
    {
        #region Private Fields
        private int id = -1;
        private int difficulty = 0;
        private readonly ConstelationMap ownerMap;
        #endregion

        #region Public Fields
        public float scale = 0;
        public Vector2 position;
        public GameObject Object;
        public List<StarJunction> fromJunctions = new List<StarJunction>();
        public List<StarJunction> toJunctions = new List<StarJunction>();
        #endregion
        
        #region Properties
        public int Id
        {
            get{
                return id;
            }
            set {
                if(id == -1)
                    id = value;
            }
        }

        public int Difficulty
        {
            get{
                return difficulty;
            }

            set{
                difficulty = value;

                PlanetAttributes planet = ownerMap != null ? ownerMap.GetPlanetAttributesForStarType(difficulty) : null;
                float tierPart = planet != null ? TierPartForPlanet(planet) : (difficulty == 0 ? 0.5f : difficulty * 0.15f);
                this.scale = tierPart + Random.Range(0.8f, 1.75f);
                Object.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50) * scale;

                ApplyPlanetVisualsFromAttributes();
            }
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Create new star from GameObject
        /// </summary>
        /// <param name="ownerMap">Used to resolve <see cref="PlanetAttributes"/> for this star type (sprite / map color).</param>
        public Star(GameObject Object, int difficulty, ConstelationMap ownerMap)
        {
            this.Object = Object;
            this.ownerMap = ownerMap;
            this.Difficulty = difficulty;
            this.position = Object.GetComponent<RectTransform>().anchoredPosition;
            SetEnabled(false);
        }

        private static float TierPartForPlanet(PlanetAttributes planet)
        {
            switch (planet.PlanetDifficulty)
            {
                case PlanetDifficulty.Store: return 0.5f;
                case PlanetDifficulty.Easy: return 0.15f;
                case PlanetDifficulty.Medium: return 0.30f;
                case PlanetDifficulty.Hard: return 0.45f;
                case PlanetDifficulty.Extreme: return 0.60f;
                case PlanetDifficulty.Boss: return 0.75f;
                default: return 0.5f;
            }
        }

        private void ApplyPlanetVisualsFromAttributes()
        {
            if (ownerMap == null)
                return;

            PlanetAttributes planet = ownerMap.GetPlanetAttributesForStarType(difficulty);
            if (planet == null)
                return;

            Image img = Object.GetComponent<Image>();
            if (img == null)
                return;

            if (planet.PlanetSprite != null)
                img.sprite = planet.PlanetSprite;
            else
            {
                Color c = planet.PlanetColor;
                c.a = img.color.a;
                img.color = c;
            }
        }
        
        /// <summary>
        /// Set if level is enabled to interact or not
        /// </summary>
        /// <param name="enabled"></param>
        public void SetEnabled(bool enabled)
        {
            try
            {
                Object.GetComponent<Button>().interactable = enabled;
            }
            catch(System.Exception)
            {

            }
        }

        /// <summary>
        /// Change current star mode
        /// </summary>
        /// <param name="mode"></param>
        public void SetMode(StarMode mode)
        {
            switch (mode)
            {
                case StarMode.PASSED_THROUGH_SELECTED:
                    Object.transform.GetChild(2).gameObject.SetActive(true);
                    break;

                case StarMode.PASSED_THROUGH_NOT_SELECTED:
                    Object.transform.GetChild(1).gameObject.SetActive(true);
                    break;

                case StarMode.DEFAULT:
                    Object.transform.GetChild(1).gameObject.SetActive(false);
                    Object.transform.GetChild(2).gameObject.SetActive(false);
                    break;
            }         
        }
        #endregion    
    }

    

    /// <summary>
    /// Represents a junction between two stars
    /// </summary>
    public class StarJunction
    {
        #region Public Fields
        public Constelation.Star starA;
        public Constelation.Star starB;
        public GameObject junction;
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Create star junction between two stars
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="junction"></param>
        public StarJunction(Constelation.Star a,Constelation.Star b,GameObject junction)
        {
            starA = a;
            starB = b;
            this.junction = junction;
        }
        #endregion
    }

    public enum StarMode
    {
        PASSED_THROUGH_SELECTED,
        PASSED_THROUGH_NOT_SELECTED,
        DEFAULT
    }
}
