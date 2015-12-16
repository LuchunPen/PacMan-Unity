using UnityEngine;
using System;

public class StartMenu : MonoBehaviour
{
    private int _activeMenu;
    public GameObject KeyBoardScreen;

    public RectTransform cursor;
    private Action _activeAction;

	public void OnUpdate()
    {
        if (KeyBoardScreen.activeInHierarchy)
        {
            _activeAction = ReturnToMainMenu;
        }
        else
        {
            if (_activeMenu == 0)
            {
                cursor.anchoredPosition = new Vector2(0, -20);
                _activeAction = StartGame;
            }

            else if (_activeMenu == 1)
            {
                cursor.anchoredPosition = new Vector2(0, -60);
                _activeAction = OpenKeyBoardScreen;
            }

            if (_activeMenu == 2)
            {
                cursor.anchoredPosition = new Vector2(0, -100);
                _activeAction = Exit;
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_activeMenu == 2) { _activeMenu = 0; }
                else { _activeMenu++; }
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_activeMenu == 0) { _activeMenu = 2; }
                else { _activeMenu--; }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_activeAction != null) { _activeAction(); }
        }
    }

    private void StartGame()
    {
        GameManager.Instance.PrepareGame();
        this.gameObject.SetActive(false);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void OpenKeyBoardScreen()
    {
        KeyBoardScreen.SetActive(true);
        _activeAction = ReturnToMainMenu;
    }

    private void ReturnToMainMenu()
    {
        KeyBoardScreen.SetActive(false);
    }
}
