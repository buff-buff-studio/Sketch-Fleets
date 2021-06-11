using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.Data;
using UnityEngine.Serialization;

namespace SketchFleets
{
    public class ParallaxActive : MonoBehaviour
    {
        [FormerlySerializedAs("difficulty")] [SerializeField]
        private MapAttributes map;
        void Start()
        {
            transform.GetChild(map.Difficulty - 1).gameObject.SetActive(true);
        }
    }
}
