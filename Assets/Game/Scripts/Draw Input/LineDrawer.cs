using System;
using System.Collections.Generic;
using SketchFleets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public sealed class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject inputTrail;

    public Gradient trailGradient;
    private GradientColorKey[] colorKey = new GradientColorKey[2];
    private GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

    [SerializeField]
    private Image backgroundOverlay;

    [SerializeField]
    private GameObject HUD;
    
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

    private int requiredShapeIndex = -1;

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
    }

    private void OnDisable()
    {
        playerControl.Disable();
        playerControl = null;
    }

    private void Update()
    {
        if (Touch.activeTouches.Count == 1) Draw();
    }

    /// <summary>
    /// Prevents the draw UI from closing until the player has drawn the required shape.
    /// </summary>
    public void RequireOutcome(int shapeIndex)
    {
        requiredShapeIndex = shapeIndex;
    }

    /// <summary>
    /// Forces the drawing to begin
    /// </summary>
    public void ForceBeginDraw()
    {
        HUD.SetActive(false);
        gameObject.SetActive(true);

        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
        }

        currentLine = Instantiate(linePrefab, transform).GetComponent<CreateLine>();
        currentLine.SetPointsMinDistance(linePointsMinDist);
        currentLine.SetLineWidht(lineWidht);
        backgroundOverlay.gameObject.SetActive(true);
        SetTrailColor();
    }

    public static void BulletTime(float time)
    {
        Time.timeScale = time;
    }

    public void DrawCallBack(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf) return;

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

        if (currentLine == null) return;
        currentLine.AddPoint(mousePos);
    }

    private void EndDraw()
    {
        inputTrail.SetActive(false);

        if (!IsLineValid())
        {
            if (requiredShapeIndex != -1)
            {
                BeginDraw();
                return;
            }
            else
            {
                RestoreUI();
                return;
            }
        }

        currentLine.lineRenderer.Simplify(simplifyTolerance);

        if (currentLine.lineRenderer.positionCount <= 3)
        {
            Destroy(currentLine.gameObject);
            
            if (requiredShapeIndex != -1)
            {
                BeginDraw();
                return;
            }
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

            if (ShapeMeetsRequirement())
            {
                Shapes[FormSelect].shapeEvent.Invoke();
                ClearShapeRequirement();
            }

            currentLine.lineRenderer.positionCount = 0;
            currentLine = null;
        }

        if (ShapeMeetsRequirement())
        {
            RestoreUI();
        }
        else
        {
            BeginDraw();
        }
    }

    /// <summary>
    /// Checks whether the currently selected shape is the required shape.
    /// </summary>
    /// <returns>Whether the currently selected shape is the required shape.</returns>
    private bool ShapeMeetsRequirement()
    {
        return requiredShapeIndex == -1 || FormSelect == requiredShapeIndex;
    }

    /// <summary>
    /// Clears any shape requirements
    /// </summary>
    private void ClearShapeRequirement()
    {
        requiredShapeIndex = -1;
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

    private void RestoreUI(bool restoreTimeScale = true)
    {
        if (restoreTimeScale)
        {
            Time.timeScale = 1;
        }

        EndEvent.Invoke();
        HUD.SetActive(true);
        gameObject.SetActive(false);
        backgroundOverlay.gameObject.SetActive(false);
    }

    private bool IsLineValid()
    {
        return currentLine != null;
    }
}

[Serializable]
public struct ShapeType
{
    public string shapeName;
    public int shapeMaxVertices;
    public UnityEvent shapeEvent;
}