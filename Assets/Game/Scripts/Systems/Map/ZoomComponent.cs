using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//Based On: https://answers.unity.com/questions/1280592/pinch-and-zoom-functionality-on-canvas-ui-images.html
/// <summary>
/// ScrollRect panel with zoom
/// </summary>
public class ZoomComponent : ScrollRect
{
    #region Private Fields
    private float _minZoom = 2f; //Auto calculated by size of map and screen size
    private float _maxZoom = 5f;
    [SerializeField] 
    private float _zoomLerpSpeed = 4f;
    private float _currentZoom = 1;
    private bool _isPinching = false;
    private float _startPinchDist;
    private float _startPinchZoom;
    private Vector2 _startPinchCenterPosition;
    private Vector2 _startPinchScreenPosition;
    private float _mouseWheelSensitivity = 1;
    private bool blockPan = false;
    #endregion

    #region Public Fields
    //Used to control input state
    public bool inputEnabled = false;
    #endregion

    #region Unity Callbacks
    /// <summary>
    /// Init panel
    /// </summary>
    protected override void Awake()
    {
        Input.multiTouchEnabled = true;
    }

    /// <summary>
    /// Update zoom lerp and inputs
    /// </summary>
    private void Update()
    {
        if (inputEnabled)
        {
            if (Input.touchCount == 2)
            {
                if (!_isPinching)
                {
                    _isPinching = true;
                    OnPinchStart();
                }
                OnPinch();
            }
            else
            {
                _isPinching = false;
                if (Input.touchCount == 0)
                {
                    blockPan = false;
                }
            }
            //pc input
#if !PLATFORM_ANDROID
            float scrollWheelInput = Mouse.current.scroll.ReadValue().y/512;
            if (Mathf.Abs(scrollWheelInput) > float.Epsilon)
            {
                SimulateScroll(scrollWheelInput, Mouse.current.position.ReadValue());
            }
#endif
        }

        //Update animation
        if (Mathf.Abs(content.localScale.x - _currentZoom) > 0.001f)
            content.localScale = Vector3.Lerp(content.localScale, Vector3.one * _currentZoom, _zoomLerpSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Update anchored position
    /// </summary>
    /// <param name="position"></param>
    protected override void SetContentAnchoredPosition(Vector2 position)
    {
        if (_isPinching || blockPan) return;
        base.SetContentAnchoredPosition(position);
    }

    /// <summary>
    /// On pinch start (zoom start)
    /// </summary>
    void OnPinchStart()
    {
        Vector2 pos1 = Input.touches[0].position;
        Vector2 pos2 = Input.touches[1].position;

        _startPinchDist = Distance(pos1, pos2) * content.localScale.x;
        _startPinchZoom = _currentZoom;
        _startPinchScreenPosition = (pos1 + pos2) / 2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, _startPinchScreenPosition, null, out _startPinchCenterPosition);

        Vector2 pivotPosition = new Vector3(content.pivot.x * content.rect.size.x, content.pivot.y * content.rect.size.y);
        Vector2 posFromBottomLeft = pivotPosition + _startPinchCenterPosition;

        SetPivot(content, new Vector2(posFromBottomLeft.x / content.rect.width, posFromBottomLeft.y / content.rect.height));
        blockPan = true;
    }

    /// <summary>
    /// On pinch change (zoom change)
    /// </summary>
    void OnPinch()
    {
        float currentPinchDist = Distance(Input.touches[0].position, Input.touches[1].position) * content.localScale.x;
        _currentZoom = (currentPinchDist / _startPinchDist) * _startPinchZoom;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Set minimum zoom limit
    /// </summary>
    /// <param name="zoom"></param>
    public void SetMinZoom(float zoom)
    {
        _minZoom = zoom;
    }

    /// <summary>
    /// Get current mim zoom limit
    /// </summary>
    /// <returns></returns>
    public float GetMinZoom()
    {
        return _minZoom;
    }

    /// <summary>
    /// Get current target zoom
    /// </summary>
    /// <returns></returns>
    public float GetCurrentZoom()
    {
        return _currentZoom;
    }

    /// <summary>
    /// Set current target zoom
    /// </summary>
    /// <param name="zoom"></param>
    public void SetCurrentZoom(float zoom)
    {
        _currentZoom = zoom;
    }

    /// <summary>
    /// Set target zoom instantly
    /// </summary>
    /// <param name="zoom"></param>
    public void SetZoomInstantly(float zoom)
    {
         _currentZoom = zoom;

         content.localScale = Vector3.one * _currentZoom;
    }

    /// <summary>
    /// Simulate scroll (mouse wheel)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pos"></param>
    public void SimulateScroll(float value, Vector2 pos)
    {
        _currentZoom *= 1 + value * _mouseWheelSensitivity;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
        _startPinchScreenPosition = pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, _startPinchScreenPosition, null, out _startPinchCenterPosition);
        Vector2 pivotPosition = new Vector3(content.pivot.x * content.rect.size.x, content.pivot.y * content.rect.size.y);
        Vector2 posFromBottomLeft = pivotPosition + _startPinchCenterPosition;
        SetPivot(content, new Vector2(posFromBottomLeft.x / content.rect.width, posFromBottomLeft.y / content.rect.height));
    }
    #endregion 

    #region Private Methods
    /// <summary>
    /// Calculate distance between two vectors onto zoom panel
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    private float Distance(Vector2 pos1, Vector2 pos2)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, pos1, null, out pos1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, pos2, null, out pos2);
        return Vector2.Distance(pos1, pos2);
    }

    /// <summary>
    /// Set zoom center pivot
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="pivot"></param>
    private static void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        if (rectTransform == null) return;

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y) * rectTransform.localScale.x;
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }
    #endregion
}