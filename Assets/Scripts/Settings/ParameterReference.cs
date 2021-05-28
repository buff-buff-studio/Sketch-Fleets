namespace SketchFleets.SettingsSystem
{
    public class ParameterReference<T>
    {
        #region Private Fields
        private T _value;
        private string _key;
        #endregion

        #region Properties 
        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                Settings.Set<T>(_key,value);
            }
        }
        #endregion
        
        #region Constructor
        public ParameterReference(string key)
        {
            this._key = key;

            _value = Settings.Get<T>(_key);
            Settings.AddHandler(_key,() => {
                _value = Settings.Get<T>(_key);
            });
        }
        #endregion
    }
}