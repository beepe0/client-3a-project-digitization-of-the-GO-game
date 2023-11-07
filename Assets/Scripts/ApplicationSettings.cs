using Player;
using Singleton;
using UnityEngine;

public class ApplicationSettings : Singleton<ApplicationSettings>
{
    [SerializeField, Header("Target frame rate")]
    public int frameRate;

    [SerializeField, Header("vSync")]
    public int vSync;

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
        Application.runInBackground = true;
        QualitySettings.vSyncCount = vSync;

        Cursor.lockState = cursorLockMode;
        Cursor.visible = isVisibleCursor;
    }
}