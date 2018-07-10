using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    private bool mMenuOn;

    private Canvas mCanvas;

    private void Start()
    {
        mCanvas = GetComponent<Canvas>();
        MenuOff();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TriggerMenu();
        }
    }


    public void Quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
		Application.Quit();
        #endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerMenu()
    {
        if (mMenuOn)
        {
            MenuOff();
        }
        else
        {
            MenuOn();
        }
    }

    private void MenuOn()
    {
        mMenuOn = true;
        mCanvas.enabled = true;
        Time.timeScale = 0f;
    }

    private void MenuOff()
    {
        mMenuOn = false;
        mCanvas.enabled = false;
        Time.timeScale = 1f;
    }
}
