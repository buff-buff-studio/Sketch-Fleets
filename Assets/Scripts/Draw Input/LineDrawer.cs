using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject inputTrail;
    public Image inputSprite;

    public float spriteSize;

    [SerializeField]
    private float linePointsMinDist;
    [SerializeField]
    private float lineWidht;
    [SerializeField]
    private float simplifyTolerance;

    private CreateLine currentLine;
    private Camera cam;
    private PlayerControl playerControl;

    [HideInInspector]
    public List<GameObject> lines = new List<GameObject>();

    [Space(15)]
    public List<ShapeType> Shapes;
    
    [Space(15f)]
    public UnityEvent EndEvent;

    private Vector2 mousePos
    {
        get
        {
            Vector2 pos;
#if PLATFORM_ANDROID
            pos = cam.ScreenToWorldPoint(playerControl.Draw.Draw.ReadValue<Vector2>());
#endif
#if !PLATFORM_ANDROID
            pos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
#endif
            return pos;
        }
    }

    private int FormSelect;
    
    private void Awake()
    {
        playerControl = new PlayerControl();
        playerControl.Enable();
        cam = Camera.main;
        playerControl.Draw.StartDraw.canceled += EndDraw;
    }

    private void OnDisable()
    {
        playerControl.Draw.StartDraw.canceled -= EndDraw;
    }

    void Update()
    {
        if(currentLine != null)
            Draw();
    }

    public void DrawCallBack(InputAction.CallbackContext context)
    {
        if(context.started)
            BeginDraw();
        else if(context.canceled)
            EndDraw(context);
    }

    void BeginDraw()
    {
        if(currentLine != null && SceneManager.GetActiveScene().name == "Game")
            Destroy(currentLine.gameObject);
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<CreateLine>();
        
        inputTrail.transform.position = mousePos;
        currentLine.SetPointsMinDistance(linePointsMinDist);
        currentLine.SetLineWidht(lineWidht);
        inputTrail.SetActive(true);
    }

    void Draw()
    {
        inputTrail.transform.position = mousePos;
        if (currentLine.pointCount >= 1 && Vector2.Distance(mousePos, currentLine.GetLastPoint()) < linePointsMinDist)
            return;
        
        currentLine.AddPoint(mousePos);
    }

    void EndDraw(InputAction.CallbackContext context)
    {
        if (currentLine != null)
        {
            currentLine.lineRenderer.Simplify(simplifyTolerance);
            
            if (currentLine.lineRenderer.positionCount <= 3)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                currentLine.lineRenderer.SetPosition(currentLine.lineRenderer.positionCount-1, currentLine.lineRenderer.GetPosition(0));
                lines.Add(currentLine.gameObject);
                FormSelect = FormDetector.Detector(currentLine.lineRenderer, Shapes);
                currentLine.transform.name = Shapes[FormSelect].shapeName + " - " + currentLine.lineRenderer.positionCount;
                Destroy(currentLine.gameObject);
                ShapeShow();
                currentLine = null;
                EndEvent.Invoke();
            }
        }
        
        if(SceneManager.GetActiveScene().name == "Game")
            inputTrail.SetActive(false);
        EndEvent.Invoke();
        transform.parent.gameObject.SetActive(false);
    }
    
    private void ShapeShow()
    {
        /*
        Tuple<Vector3, float> shapeInfo = currentLine.GetLineCenter(Shapes[FormSelect].shapeName);        
        
        Vector2 ViewportPosition = cam.WorldToScreenPoint(shapeInfo.Item1);

        inputSprite.rectTransform.localScale = Vector2.one*(shapeInfo.Item2 / spriteSize);
        inputSprite.rectTransform.anchoredPosition = transform.InverseTransformPoint(ViewportPosition);
        inputSprite.sprite = Shapes[FormSelect].shapeSprite;
        inputSprite.color = Shapes[FormSelect].shapeColor;
        inputSprite.GetComponent<Animator>().Play("showHide");
        */
        
        Shapes[FormSelect].shapeEvent.Invoke();
    }
}

[System.Serializable]
public struct ShapeType
{
    public string shapeName;
    public int shapeMaxVertices;
    public Color shapeColor;
    public Sprite shapeSprite;
    public UnityEvent shapeEvent;
}
