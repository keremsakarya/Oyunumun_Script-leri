using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathPanelController : MonoBehaviour
{
    public GameObject deathPanel;
    public MonoBehaviour playerMovement;
    public AudioSource[] audioSources;
    public Button restartButton;

    void OnEnable()
    {
        PlayerHealth.PlayerDied += OnPlayerDied;
    }

    void OnDisable()
    {
        PlayerHealth.PlayerDied -= OnPlayerDied;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (deathPanel != null) deathPanel.SetActive(false);
        if (restartButton != null) restartButton.onClick.AddListener(RestartScene);
    }

    void OnPlayerDied()
    {
        if (playerMovement != null) playerMovement.enabled = false;

        foreach (var source in audioSources)
        {
            source.Stop();
        }

        if (deathPanel != null) deathPanel.SetActive(true);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
