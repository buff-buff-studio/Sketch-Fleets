using System.Collections.Generic;
using SketchFleets.ProfileSystem;

/// <summary>
/// Holds a state of constelation with unlocked path and other infos
/// </summary>
[System.Serializable]
public class ConstelationState
{   
    #region Private Fields
    public Constelation constelation;
    private List<int> openQueue = new List<int>();
    #endregion

    #region Public Methods
    /// <summary>
    /// Creates a new constelation state
    /// </summary>
    /// <param name="constelation"></param>
    public ConstelationState(Constelation constelation)
    {
        this.constelation = constelation;
    }
    
    /// <summary>
    /// Set current star
    /// </summary>
    /// <param name="currentStar"></param>
    public void SetCurrentStar(int currentStar)
    {
        Profile.GetData().Map.currentStar = currentStar;
    }

    /// <summary>
    /// Get current star
    /// </summary>
    /// <returns></returns>
    public int GetCurrentStar()
    {
        return Profile.GetData().Map.currentStar;
    }

    /// <summary>
    /// Check if star is open
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public bool IsOpen(int star)
    {
        return Profile.GetData().Map.openPath.Contains(star) && !openQueue.Contains(star);
    }

    /// <summary>
    /// Set current constelation
    /// </summary>
    /// <param name="constelation"></param>
    public void SetConstelation(Constelation constelation)
    {
        this.constelation = constelation;
    }

    /// <summary>
    /// Open a constelation star
    /// </summary>
    /// <param name="star"></param>
    public void Open(int star)
    {
        if(!IsOpen(star))
        {
            //Remove from open queue
            if(openQueue.Contains(star))
                openQueue.Remove(star);

            Profile.GetData().Map.openPath.Add(star);
            _Open(star);
        }
    }

    /// <summary>
    /// Add start to open queue to save
    /// </summary>
    /// <param name="star"></param>
    public void AddToOpenQueue(int star)
    {
        if(!openQueue.Contains(star) && !Profile.GetData().Map.openPath.Contains(star))
        {
            openQueue.Add(star);
            Profile.GetData().Map.openPath.Add(star);
        }
        //if(!Profile.GetData().Map.openQueue.Contains(star) && !Profile.GetData().Map.openPath.Contains(star))
            //Profile.GetData().Map.openQueue.Add(star);
    }

    /// <summary>
    /// Check if star is choosen
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public bool IsChoosen(int star)
    {
        return Profile.GetData().Map.choosen.Contains(star);
    }
    
    /// <summary>
    /// Choose a star
    /// </summary>
    /// <param name="star"></param>
    public void Choose(int star)
    {   
        if(!IsChoosen(star))
        {
            Profile.GetData().Map.currentStar = star;
            Profile.GetData().Map.choosen.Add(star);
            _Choose(star);
        }
    }

    /// <summary>
    /// Update all starts
    /// </summary>
    public void Init()
    {
        foreach(int i in Profile.GetData().Map.openPath)
            _Open(i);
        
        foreach(int i in Profile.GetData().Map.choosen)
            _Choose(i);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Internal open star
    /// </summary>
    /// <param name="star"></param>
    private void _Open(int star)
    {
        //Get star
        Constelation.Star s = constelation.GetStar(star);

        s.SetEnabled(true);

        //Get parents
        foreach(Constelation.StarJunction jr in s.fromJunctions)
        {
            if(IsChoosen(jr.starA.Id))
            {
                jr.starA.SetMode(Constelation.StarMode.PASSED_THROUGH_SELECTED);
            }
        }
    }

    /// <summary>
    /// Internal choose start
    /// </summary>
    /// <param name="star"></param>
    private void _Choose(int star)
    {   
        //Get star
        Constelation.Star s = constelation.GetStar(star);

        //Get parents
        foreach(Constelation.StarJunction jr in s.fromJunctions)
        {
            if(IsChoosen(jr.starA.Id))
            {
                foreach(Constelation.StarJunction jrb in jr.starA.toJunctions)
                {
                    if(jrb.starB.Id == star)
                        continue;
                    jrb.starB.SetEnabled(false);
                    jrb.starB.SetMode(Constelation.StarMode.PASSED_THROUGH_NOT_SELECTED);
                }

                jr.starA.SetMode(Constelation.StarMode.PASSED_THROUGH_SELECTED);
            }
        }

        //Update this      
        bool childOpen = false;
        foreach(Constelation.StarJunction jr in s.toJunctions) 
            if(IsOpen(jr.starB.Id))
            {
                childOpen = true;
                break;
            }
        if(!childOpen)
            s.SetMode(Constelation.StarMode.DEFAULT);
            
        s.SetEnabled(true);

        //Check if must disable 
        foreach(Constelation.StarJunction jr in s.toJunctions)
        {
            if(IsOpen(jr.starB.Id))
            {
                s.SetEnabled(false);
                break;
            }
        }    
    }
    #endregion

    /// <summary>
    /// To string helper
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string s = "openPath(";

        for(int i = 0; i < Profile.GetData().Map.openPath.Count; i ++)
        {
            if(i > 0)
                s += ",";
            s += Profile.GetData().Map.openPath[i].ToString();
        }

        s += "),choosen(";

        for(int i = 0; i < Profile.GetData().Map.choosen.Count; i ++)
        {
            if(i > 0)
                s += ",";
            s += Profile.GetData().Map.choosen[i].ToString();
        }

        return s + "),currentStar=" + Profile.GetData().Map.currentStar + ",seed=" + Profile.GetData().Map.seed;
    }
}