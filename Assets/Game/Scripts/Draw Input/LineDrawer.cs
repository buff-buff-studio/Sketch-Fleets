using System;
using System.Collections.Generic;
using ManyTools.Variables;
using SketchFleets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject inputTrail;
    public Image inputSprite;

    public float spriteSize;

    public Gradient trailGradient;
    private GradientColorKey[] colorKey = new GradientColorKey[2];
    private GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
    
    [SerializeField]
    private ColorsInventory colorsInventory;

    [SerializeField]
    private float linePointsMinDist;

    [SerializeField]
    private float lineWidht;

    [SerializeField]
    private float simplifyTolerance;

    private CreateLine currentLine;
    private Camera cam;
    private IAA_SketchFleetsInputs playerControl;
    
    private int FormSelect;

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
            cam ??= Camera.main;

            return cam.ScreenToWorldPoint(playerControl.InGame.TouchOne.ReadValue<Vector2>());
        }
    }

    private void OnEnable()
    {
        playerControl = new IAA_SketchFleetsInputs();
        playerControl.Enable();
        
        Debug.Log(colorsInventory.drawColor.ToString());
    }

    private void OnDisable()
    {
        playerControl.Disable();
        playerControl = null;
    }

    private void Update()
    {
        if (Touch.activeTouches.Count == 1)
            Draw();
    }

    public void BulletTime(float time)
    {
        Time.timeScale = time;
    }

    public void DrawCallBack(InputAction.CallbackContext context)
    {
        if(!gameObject.activeSelf) return;

        if (context.started)
        {
            BeginDraw();
        }
        else if (context.canceled)
        {
            EndDraw();
        }
    }

    private void BeginDraw()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }
        
        currentLine = Instantiate(linePrefab, transform).GetComponent<CreateLine>();
        currentLine.SetPointsMinDistance(linePointsMinDist);
        currentLine.SetLineWidht(lineWidht);
        SetTrailColor();
    }

    private void Draw()
    {
        if (currentLine == null) return;
            
        inputTrail.transform.position = mousePos;

        if (currentLine.pointCount >= 1 && Vector2.Distance(mousePos, currentLine.GetLastPoint()) < linePointsMinDist)
        {
            return;
        }

        currentLine.AddPoint(mousePos);
    }

    private void EndDraw()
    {
        inputTrail.SetActive(false);

        if (!IsLineValid())
        {
            RestoreUI();
            return;
        }
            
        currentLine.lineRenderer.Simplify(simplifyTolerance);

        if (currentLine.lineRenderer.positionCount <= 3)
        {
            Destroy(currentLine.gameObject);
        }
        else
        {
            currentLine.lineRenderer.SetPosition(currentLine.lineRenderer.positionCount - 1,
                currentLine.lineRenderer.GetPosition(0));
                
            lines.Add(currentLine.gameObject);
                
            FormSelect = FormDetector.Detector(currentLine.lineRenderer, Shapes);

            currentLine.transform.name =
                Shapes[FormSelect].shapeName + " - " + currentLine.lineRenderer.positionCount;
                
            Destroy(currentLine.gameObject);
            ShapeShow();
            currentLine = null;
        }
        
        RestoreUI();
    }
    
    private void SetTrailColor()
    {
        inputTrail.transform.position = mousePos;
        
        colorKey[0].color = colorsInventory.drawColor;
        colorKey[1].color = colorsInventory.drawColor;
        colorKey[0].time = 0f;
        colorKey[1].time = 1f;
        alphaKeys[0].alpha = 1f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].time = 1f;
        
        trailGradient.SetKeys(colorKey, alphaKeys);

        inputTrail.GetComponent<TrailRenderer>().colorGradient = trailGradient;
        currentLine.SetLineColor(trailGradient);
        inputTrail.SetActive(true);
    }

    private void RestoreUI()
    {
        Time.timeScale = 1;
        EndEvent.Invoke();
        gameObject.SetActive(false);
    }

    private bool IsLineValid()
    {
        return currentLine != null;
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

[Serializable]
public struct ShapeType
{
    public string shapeName;
    public int shapeMaxVertices;
    public Color shapeColor;
    public Sprite shapeSprite;
    public UnityEvent shapeEvent;
}