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

    #region Properties

    public BulletAttributes Attributes
    {
        get => attributes;
        set => attributes = value;
    }

    #endregion
    
    #region Unity Callbacks

    private void Start()
    {
        // Replace by pooling call
        Destroy(gameObject, 10f);
        GetComponent<Rigidbody2D>().AddForce(transform.up * attributes.Speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        Debug.Log(attributes.Speed);
        //transform.Translate(Vector3.forward * Time.deltaTime * Attributes.Speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Enemy")) return;
        
        Destroy(gameObject, 10);
        transform.localScale *= .75f;
    }
    
    #endregion
}
