using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseModal : Modal
{
    public void Resume()
        => Deactivate();

    public void ToMenu()
    {
        Deactivate();
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }

    protected override void OnActivate() { }
    protected override void OnDeactivate() { }
}
