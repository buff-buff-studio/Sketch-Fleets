using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets.LanguageSystem
{
    /// <summary>
    /// Holds a language entry
    /// </summary>
    public class LanguageEntry
    {
        #region Private Fields
        private string value;
        private int weight = 0;
        #endregion

        #region Property
        public int Weight{get{ return weight;} set{weight = value;}}
        #endregion

        #region Constructor
        /// <summary>
        /// Create new entry
        /// </summary>
        /// <param name="value"></param>
        /// <param name="weight"></param>
        public LanguageEntry(string value,int weight)
        {
            this.weight = weight;
            this.value = value;
        }
        #endregion

        #region Util
        /// <summary>
        /// Formatted to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return weight + ": " + value;
        }
        #endregion

        public static explicit operator string(LanguageEntry a)
        {
            return a.value;
        }
    }

    /// <summary>
    /// Holds language full data
    /// </summary>
    public class Language
    {
        #region Contants
        public static readonly string MissingEntry = "<missing_entry>";
        #endregion

        #region Private Fields
        private string code;
        private string content;
        private bool baked = false;
        private Dictionary<string,LanguageEntry[]> entries = new Dictionary<string, LanguageEntry[]>();
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new language with file code and all baked entries
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entries"></param>
        public Language(string code,string content)
        {
            this.code = code;
            this.content = content;
        }
        #endregion

        #region Bake
        /// <summary>
        /// Bake entries
        /// </summary>
        public void Bake()
        {
            if(baked)
                return;
            baked = true;
            string key = null;
            Dictionary<string,List<LanguageEntry>> notBakedEntries = new Dictionary<string, List<LanguageEntry>>();
            entries.Clear();

            System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo) System.Globalization.CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            //Populate entries
            foreach(string ln in System.Text.RegularExpressions.Regex.Split(content,"\r\n|\r|\n"))
            {
                if(ln.StartsWith("#"))
                    continue;
                if(ln.Trim().Length == 0)
                    continue;

                string line = ln.TrimStart();

                string[] s = line.Split(new char[] { '=' }, 2);

                if(s.Length == 2)
                {
                    if(s[0].Length == 0)
                    {
                        //Use last key
                        if(key != null)
                        {
                            //Add value
                            if(!notBakedEntries.ContainsKey(key))
                                notBakedEntries[key] = new List<LanguageEntry>();
                            notBakedEntries[key].Add(new LanguageEntry(s[1],0));
                        }
                        else
                            throw new System.NullReferenceException("A language key cannot be null!");
                    }
                    else
                    {
                        if(float.TryParse(s[0],out _))
                        {
                            int weight = (int) (float.Parse(s[0],System.Globalization.NumberStyles.Any,ci) * 100);
                            if(key != null)
                            {
                                //Add value
                                if(!notBakedEntries.ContainsKey(key))
                                    notBakedEntries[key] = new List<LanguageEntry>();
                                notBakedEntries[key].Add(new LanguageEntry(s[1],weight));
                            }
                            else
                                throw new System.NullReferenceException("A language key cannot be null!");

                        }
                        else
                        {
                            //Add normally
                            key = s[0];

                            //Add value
                            if(!notBakedEntries.ContainsKey(key))
                                notBakedEntries[key] = new List<LanguageEntry>();
                            notBakedEntries[key].Add(new LanguageEntry(s[1],0));
                        }
                    }
                }
                else
                {
                    //New key only
                    key = s[0];
                }
            }
        
            //Bake entries
            foreach(KeyValuePair<string,List<LanguageEntry>> pair in notBakedEntries)
            {
                int sum = 100;
                int needed = 0;
                
                //Entries
                foreach(LanguageEntry entry in pair.Value)
                {
                    sum -= entry.Weight;
                    if(entry.Weight == 0)
                        needed ++;
                }

                //Remaning
                int remaining = sum - (sum/needed) * needed;

                //Bake entries
                int i = 0;
                foreach(LanguageEntry entry in pair.Value)
                {
                    if(entry.Weight == 0)
                    {
                        entry.Weight = (sum/needed + (i == 0 ? remaining : 0));
                    }

                    i ++;
                }

                entries[pair.Key] = pair.Value.ToArray();
            }
        }

        /// <summary>
        /// Unbake language
        /// </summary>
        public void Unbake()
        {
            baked = false;
            entries.Clear();
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Get language code
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            return code;
        }

        /// <summary>
        /// Check if contains entry
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return entries[key] != null;
        }

        /// <summary>
        /// Enumerate all keys
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetAllKeys()
        {
            return entries.Keys;
        }

        /// <summary>
        /// Localize a string without arguments
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Localize(string key)
        {
            if(entries[key] == null)
                return MissingEntry;

            return GetRandom(entries[key]);
        }

        /// <summary>
        /// Localize a string with arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Localize(string key,params string[] args)
        {
            if(entries[key] == null)
                return MissingEntry;
            
            string s = GetRandom(entries[key]);

            for(int i = 0; i < args.Length; i ++)
                s = System.Text.RegularExpressions.Regex.Replace(s, @"\{([^}]+)\}", m => args[int.Parse(m.Groups[1].Value)]);
            return s;
        }

        /// <summary>
        /// Get random string from an array of language entries
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private string GetRandom(LanguageEntry[] h)
        {
            if(h.Length == 0)
                return (string) h[0];

            int i = Random.Range(0,100);

            int s = 0;
            foreach(LanguageEntry e in h)
                if(s + e.Weight > i)
                    return (string) e;
                else
                    s += e.Weight;

            return (string) h[0];
        }
        #endregion

        #region Utils
        /// <summary>
        /// Return formatted to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            foreach(KeyValuePair<string,LanguageEntry[]> entry in entries)
            {
                s += entry.Key + "\n";
                foreach(LanguageEntry e in entry.Value)
                    s += "  " + e.ToString() + "\n";
            }
            return s;
        }
        #endregion
    }
}
