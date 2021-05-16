using System.Collections.Generic;

/// <summary>
/// Holds a state of constelation with unlocked path and other infos
/// </summary>
[System.Serializable]
public class ConstelationState
{   
    #region Private Fields
    public Constelation constelation;
    #endregion

    #region Public Fields
    public int currentStar = 0;
    public List<int> openPath = new List<int>();
    private List<int> openQueue = new List<int>();
    public List<int> choosen = new List<int>();
    public int seed = 0; //0 = any random seed
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
        this.currentStar = currentStar;
    }

    /// <summary>
    /// Get current star
    /// </summary>
    /// <returns></returns>
    public int GetCurrentStar()
    {
        return currentStar;
    }

    /// <summary>
    /// Check if star is open
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public bool IsOpen(int star)
    {
        return openPath.Contains(star);
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

            openPath.Add(star);
            _Open(star);
        }
    }

    /// <summary>
    /// Add start to open queue to save
    /// </summary>
    /// <param name="star"></param>
    public void AddToOpenQueue(int star)
    {
        if(!openQueue.Contains(star) && !openPath.Contains(star))
            openQueue.Add(star);
    }

    /// <summary>
    /// Check if star is choosen
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public bool IsChoosen(int star)
    {
        return choosen.Contains(star);
    }
    
    /// <summary>
    /// Choose a star
    /// </summary>
    /// <param name="star"></param>
    public void Choose(int star)
    {   
        if(!IsChoosen(star))
        {
            currentStar = star;
            choosen.Add(star);
            _Choose(star);
        }
    }

    /// <summary>
    /// Update all starts
    /// </summary>
    public void Init()
    {
        foreach(int i in openPath)
            _Open(i);
        
        foreach(int i in choosen)
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
        constelation.GetStar(star).SetEnabled(true);
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
            }
        }

        //Update this      
        s.SetMode(Constelation.StarMode.PASSED_THROUGH_SELECTED);
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
    /// Load data of constelation state from bytes
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public ConstelationState LoadData(byte[] data)
    {
        //Clear all current data
        openPath.Clear();
        choosen.Clear();
        openQueue.Clear();

        //Current byte section
        int section = 0;

        //Hold seed bytes
        List<byte> seedHolder = new List<byte>();

        //Iterate over all bytes
        for(int i = 0; i < data.Length; i ++)
        {
            if(data[i] == 255)
            {
                section ++;
                continue;
            }

            switch (section)
            {
                case 0:
                    openPath.Add(data[i]);
                    break;
                case 1:
                    choosen.Add(data[i]);
                    break;
                case 2:
                    currentStar = data[i];
                    break;
                case 3:
                    seedHolder.Add(data[i]);
                    break;
            }
        }
        
        //Convert all bytes to seed
        byte[] bytes = seedHolder.ToArray();
        if (System.BitConverter.IsLittleEndian)
            System.Array.Reverse(bytes);
        seed = System.BitConverter.ToInt32(bytes, 0);

        //Update
        return this;
    }

    /// <summary>
    /// Get bytes from ConstelationState to save
    /// </summary>
    /// <returns></returns>
    public byte[] ToData()
    {
        List<int> fullOpen = new List<int>();
        fullOpen.AddRange(openPath);
        fullOpen.AddRange(openQueue);

        byte[] data = new byte[fullOpen.Count + 1 + choosen.Count + 7];
        
        //Set open path data
        for(int i = 0; i < fullOpen.Count; i ++)
        {
            int v = fullOpen[i];
            byte a = (byte) (v);
            data[i] = a;
        }

        //Seperator
        data[fullOpen.Count] = 255;

        //Set choosen count
        for(int i = 0; i < choosen.Count; i ++)
        {
            int v = choosen[i];
            byte a = (byte) (v);
            data[fullOpen.Count + 1 + i] = a;
        }

        //Seperator
        data[fullOpen.Count + 1 + choosen.Count] = 255;

        //Set current choosen
        data[fullOpen.Count + 1 + choosen.Count + 1] = (byte) currentStar;

        //Seperator
        data[fullOpen.Count + 1 + choosen.Count + 2] = 255;

        //Set current seed
        byte[] bt = System.BitConverter.GetBytes(seed);
        
        if (System.BitConverter.IsLittleEndian)
            System.Array.Reverse(bt);

        data[fullOpen.Count + 1 + choosen.Count + 3] = bt[0];
        data[fullOpen.Count + 1 + choosen.Count + 4] = bt[1];
        data[fullOpen.Count + 1 + choosen.Count + 5] = bt[2];
        data[fullOpen.Count + 1 + choosen.Count + 6] = bt[3];

        return data;
    }

    /// <summary>
    /// To string helper
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string s = "openPath(";

        for(int i = 0; i < openPath.Count; i ++)
        {
            if(i > 0)
                s += ",";
            s += openPath[i].ToString();
        }

        s += "),choosen(";

        for(int i = 0; i < choosen.Count; i ++)
        {
            if(i > 0)
                s += ",";
            s += choosen[i].ToString();
        }

        return s + "),currentStar=" + currentStar + ",seed=" + seed;
    }
}