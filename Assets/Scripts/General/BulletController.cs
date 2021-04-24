using UnityEngine;
using SketchFleets.Data;

/// <summary>
/// A class that controls a bullet and its behaviour
/// </summary>
public class BulletController : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private BulletAttributes attributes;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // Replace by pooling call
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * attributes.Speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Enemy")) return;
        
        Destroy(gameObject, 10);
        transform.localScale *= .75f;
    }
    
    #endregion
}
