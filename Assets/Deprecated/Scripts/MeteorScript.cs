// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using SketchFleets.Entities;
// using SketchFleets.Enemies;
//
// public class MeteorScript : MonoBehaviour
// {
//     #region Private Fields
//     private int damage;
//     #endregion
//
//     #region Unity Callbacks
//     void Start()
//     {
//         float size = Random.Range(0, .75f);
//         transform.localScale += new Vector3(size, size, size);
//         GetComponent<Rigidbody2D>().AddForce(-transform.right * 55, ForceMode2D.Impulse);
//         damage = GetComponent<Obstacle>().damage;
//         damage = (int)(damage + (float)damage * size);
//     }
//     #endregion
//
//     #region Collider
//     private void OnTriggerEnter2D(Collider2D col)
//     {
//         if (col.gameObject.CompareTag("Player"))
//         {
//             col.GetComponent<Mothership>().CurrentHealth.Value -= damage;
//         }
//         else if (col.gameObject.CompareTag("Enemy"))
//         {
//             col.GetComponent<EnemyShip>().CurrentHealth.Value -= damage;
//         }
//         else if (col.name == "collider_back")
//         {
//             Destroy(gameObject, 1);
//         }
//     }
//     #endregion
// }
