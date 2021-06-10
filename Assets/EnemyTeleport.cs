using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class EnemyTeleport : MonoBehaviour
    {
        [SerializeField]
        private Vector2 xDist;
        private Transform colTransform;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                col.TryGetComponent(out colTransform);
                col.transform.position = new Vector2(
                    colTransform.position.x + Random.Range(xDist.x, xDist.y),
                    Random.Range(-10, 10));
            }
        }
    }
}
