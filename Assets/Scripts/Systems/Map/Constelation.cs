using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds a constelation of stars and all path between stars
/// </summary>
public class Constelation
{
    #region Private Fields
    private List<List<Star>> stars = new List<List<Star>>();
    private List<Star> allStars = new List<Star>();
    private ConstelationMap map;
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
    public Constelation(ConstelationMap map)
    {
        this.map = map;
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

    /// <summary>
    /// Represents a star in constelation
    /// </summary>
    public class Star
    {
        #region Private Fields
        private int id = -1;
        private int difficulty = 0;
        #endregion

        #region Public Fields
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
                if(difficulty == 0)
                    difficulty = value;
            }
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Create new star from GameObject
        /// </summary>
        /// <param name="Object"></param>
        public Star(GameObject Object,int difficulty)
        {
            this.Object = Object;
            this.difficulty = difficulty;
            SetEnabled(false);
        }
        
        /// <summary>
        /// Set if level is enabled to interact or not
        /// </summary>
        /// <param name="enabled"></param>
        public void SetEnabled(bool enabled)
        {
            Object.GetComponent<Button>().interactable = enabled;
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
                    Object.GetComponent<Image>().color = Color.green;
                    break;

                case StarMode.PASSED_THROUGH_NOT_SELECTED:
                    Object.GetComponent<Image>().color = Color.red;
                    break;

                case StarMode.DEFAULT:
                    Object.GetComponent<Image>().color = Color.white;
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
