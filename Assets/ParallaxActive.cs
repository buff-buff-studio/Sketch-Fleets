using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;

namespace SketchFleets
{
    public class ParallaxActive : MonoBehaviour
    {
        [SerializeField]
        private DifficultyAttributes difficulty;
        void Start()
        {
            transform.GetChild(difficulty.Difficulty - 1).gameObject.SetActive(true);
        }
    }
}
