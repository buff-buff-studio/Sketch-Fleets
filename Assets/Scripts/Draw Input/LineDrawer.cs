using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Events;
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

    [HideInInspector]
    public List<GameObject> lines = new List<GameObject>();

    public List<ShapeType> Shapes;
    
    private Vector2 mousePos => cam.ScreenToWorldPoint(Input.mousePosition);

    private int FormSelect;
    
    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            BeginDraw();
        
        if(currentLine != null)
            Draw();
        
        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }

    void BeginDraw()
    {
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

    void EndDraw()
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
                Destroy(currentLine.gameObject, 1f);
                ShapeShow();
                currentLine = null;
            }
        }
        
        inputTrail.SetActive(false);
    }
    
    private void ShapeShow()
    {
        Tuple<Vector3, float> shapeInfo = currentLine.GetLineCenter(Shapes[FormSelect].shapeName);
        Vector2 ViewportPosition = cam.WorldToScreenPoint(shapeInfo.Item1);

        /*
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
