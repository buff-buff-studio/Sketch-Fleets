using System.Text;
using TMPro;
using UnityEngine;

public class FrameMeter : MonoBehaviour
{
    #region Private Fields

    private static readonly string FPS = "FPS ";
    private static readonly int STRING_SIZE = FPS.Length + 2;
    
    private TMP_Text textField;
    
    [SerializeField]
    private float refreshInterval = 0.1f;
    
    private int frames;
    private float remainingTime;
    private float accumulatedTime;

    private StringBuilder stringBuilder = new StringBuilder(STRING_SIZE);
    
    #endregion


    #region Private Methods

    private void Awake()
    {
        textField = GetComponent<TMP_Text>();
        remainingTime = refreshInterval;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        accumulatedTime += Time.timeScale / Time.deltaTime;
        ++frames;

        int fps = (int)accumulatedTime / frames;

        if (remainingTime <= 0f)
        {
            stringBuilder.Remove(0, stringBuilder.Length);

            stringBuilder.Append(fps);
            stringBuilder.Append(FPS);

            textField.text = stringBuilder.ToString();

            remainingTime = refreshInterval;
            accumulatedTime = 0f;
            frames = 0;
        }
    }
    
    #endregion
}
