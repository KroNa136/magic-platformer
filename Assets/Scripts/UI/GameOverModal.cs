using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverModal : Modal
{
    public void Respawn()
    {
        Deactivate();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void ToMenu()
    {
        Deactivate();
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }

    protected override void OnActivate() { }
    protected override void OnDeactivate() { }
}
