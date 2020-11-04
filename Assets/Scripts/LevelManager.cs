using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _totalLevels;
    private int _current;

    private void Start()
    {
        _current = CurrentLevel();
        _totalLevels = SceneManager.sceneCountInBuildSettings - 1;
    }

    public void IncreaseLevel()
    {
        if (_current < _totalLevels)
        {
            _current++;
            SceneManager.LoadScene(_current);
        }
        else
        {
            SceneManager.LoadScene(_current);
        }
    }

    public void DecreaseLevel()
    {
        if (_current != 0)
        {
            _current--;
            SceneManager.LoadScene(_current);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public int CurrentLevel()
    {
        Scene current = SceneManager.GetActiveScene();
        return current.buildIndex;
    }


}
