using Singleton;
using UnityEngine;

public class ApplicationSettings : Singleton<ApplicationSettings>
{
    [SerializeField, Header("Monitor")]
    public int frameRate;
    public int vSync;

    [SerializeField, Header("Client")] 
    public bool isRunInBackground;
    
    [SerializeField, Header("Cursor lock mode")]
    public CursorLockMode cursorLockMode;
    
    [SerializeField]
    public bool isVisibleCursor;
    
    protected override void OnAwake()
    {
        UpdateField();
    }

    public void UpdateField()
    {
        Application.targetFrameRate = frameRate;
        Application.runInBackground = isRunInBackground;
        QualitySettings.vSyncCount = vSync;

        Cursor.lockState = cursorLockMode;
        Cursor.visible = isVisibleCursor;
    }
}