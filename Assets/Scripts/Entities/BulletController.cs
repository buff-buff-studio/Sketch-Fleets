using UnityEngine;
using SketchFleets.Data;
using ManyTools.Variables;

/// <summary>
/// A class that controls a bullet and its behaviour
/// </summary>
public class BulletController : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private BulletAttributes attributes;
    [SerializeField]
    private FloatReference playerLife;
    [SerializeField]
    private bool enemyBullet;

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
        if (col.gameObject.CompareTag("EndMap"))
        {
            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("Enemy") && !enemyBullet)
        {
            Destroy(gameObject, 10);
            transform.localScale *= .75f;
        }

        if (col.gameObject.CompareTag("Player") && enemyBullet)
        {
            Destroy(gameObject, 10);
            playerLife.Value -= attributes.DirectDamage;
            transform.localScale *= .75f;
        }

    }
    
    #endregion
}
