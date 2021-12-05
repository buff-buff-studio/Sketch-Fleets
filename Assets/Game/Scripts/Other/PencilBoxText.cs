using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.ProfileSystem;
using TMPro;

namespace SketchFleets
{
    public class PencilBoxText : MonoBehaviour
    {
        public static int AddedAmount = 0;
        public float animTime = 0;

        public AudioSource increaseSoundEffect;
        
        private TMP_Text txtMeshPro;
        private void Start()
        {
            TryGetComponent(out txtMeshPro);
        }

        float ls = 0;

        float delay = 0;

        private void OnEnable()
        {
            delay = 1.25f;
        }

        int lscount = 0;
        void Update()
        {
            if(delay > 0)
            {
                txtMeshPro.text = (Profile.Data.TotalCoins - ProfileData.ConvertCoinsToTotalCoins(AddedAmount)).ToString();
                delay -= Time.unscaledDeltaTime;
                animTime = Time.unscaledTime;
                return;
            }

            float time = Time.unscaledTime - animTime;

            int c = ProfileData.ConvertCoinsToTotalCoins(AddedAmount);
            int count = c - (int) Mathf.Max(0,(time - 1) * (c < 30 ? 20 : (c < 100 ? 50 : 100)));
            
            if(lscount != count && time - ls > 0.25f && time > 0.75f)
            {
                lscount = count;
                if(lscount >= 0)
                {
                    increaseSoundEffect.Play();
                }
                ls = time;
            }
            
            txtMeshPro.text = (Profile.Data.TotalCoins - Mathf.Max(0,count)) + (count > 0 ? (" +" + count) : string.Empty);
        }


    }
}
