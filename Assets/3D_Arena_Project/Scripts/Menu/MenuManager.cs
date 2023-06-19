using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerModel _playerModel;

    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private GameObject _panelEndGame;

    private void OnEnable()
    {
        _playerModel.EndGame += EndGame;
    }

    private void OnDisable()
    {
        _playerModel.EndGame -= EndGame;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        _panelMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void EndGame()
    {
        _panelEndGame.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _panelMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
