using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

public class UIManager: MonoBehaviour
{
    public GameObject StartScreen;
    private RectTransform _canvasTrans;

    public Image LifeImg;
    public Image CherryImg;

    public RectTransform LifeContainer;
    private List<Image> _lifesprites;

    public RectTransform BonusContainer;
    private List<Image> _bonusSprites;

    public Text PlayerScore;
    public Text HighScore;

    public Text LevelMessage;

    public FloatingText textPref;
    public float floatingTextSpeed;

    void Awake ()
    {
        _canvasTrans = this.GetComponent<RectTransform>();
        _lifesprites = new List<Image>();
        _bonusSprites = new List<Image>();
        GameManager.Instance.OnCurrentScoreChange += OnCurrentScoreHandler;
        GameManager.Instance.OnLevelMessage += OnLevelMessageHandler;
        GameManager.Instance.OnPlayerLifeChange += OnPlayerLifeHandler;
        GameManager.Instance.OnFloatingMessage += OnFloatingTextCreator;
        GameManager.Instance.OnBonusPlaced += OnBonusPlaceHandler;
        GameManager.Instance.OnStartGame += OnStartGameHandler;
    }
	
    private void OnStartGameHandler(object sender, EventArgs args)
    {
        StartScreen.SetActive(false);
    }

    private void OnCurrentScoreHandler(object sender, ScoreArgs args)
    {
        PlayerScore.text = args.Score.ToString();
    }
    private void OnLevelMessageHandler(object sender, MessageArgs args)
    {
        LevelMessage.rectTransform.position = args.Position;
        LevelMessage.text = args.Msg;
    }
    private void OnPlayerLifeHandler(object sender, ScoreArgs args)
    {
        int count = args.Score;
        int lifecount = _lifesprites.Count;

        if (count > _lifesprites.Count)
        {
            for (int i = 0; i < count - lifecount; i++)
            {
                Image img = Instantiate(LifeImg);
                img.rectTransform.SetParent(LifeContainer);
                img.rectTransform.localScale = new Vector3(1, 1, 1);
                _lifesprites.Add(img);
            }
        }
        else if (_lifesprites.Count > 0 && count < lifecount)
        {
            for (int i = 0; i < lifecount - count; i++)
            {
                Image img = _lifesprites[0];
                Destroy(img.gameObject);
                _lifesprites.RemoveAt(0);
            }
        }
    }
    private void OnFloatingTextCreator(object sender, MessageArgs args)
    {
        FloatingText go = Instantiate(textPref, args.Position, Quaternion.identity) as FloatingText;
        go.transform.SetParent(_canvasTrans);
        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        go.Initialize(floatingTextSpeed, new Vector3(0,1,0), 3);
        go.GetComponent<Text>().text = args.Msg;
        go.GetComponent<Text>().color = args.MsgColor;
    }
    private void OnBonusPlaceHandler(object sender, CellEventArgs args)
    {
        foreach (Image img in _bonusSprites) { Destroy(img.gameObject); }
        _bonusSprites.Clear();
        for (int i = 0; i < args.BonusType.Count; i++)
        {
            CellType bonus = args.BonusType[i];
            Image img = null;
            switch (bonus)
            {
                case CellType.Cherry: img = Instantiate(CherryImg); break;
                //add some other bonus
            }
            if (img != null)
            {
                img.rectTransform.SetParent(BonusContainer);
                img.rectTransform.localScale = new Vector3(1, 1, 1);
                _bonusSprites.Add(img);
            }
        }
    }
}
