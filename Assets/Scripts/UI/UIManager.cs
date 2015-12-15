using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class UIManager: MonoBehaviour
{
    private RectTransform _canvasTrans;

    public Image lifeimg;
    public RectTransform LifeContainer;
    private List<Image> _lifesprites;

    public Text PlayerScore;
    public Text HighScore;

    public Text LevelMessage;

    public FloatingText textPref;
    public float floatingTextSpeed;

    void Start ()
    {
        _canvasTrans = this.GetComponent<RectTransform>();
        _lifesprites = new List<Image>();
        GameManager.Instance.OnCurrentScoreChange += OnCurrentScoreHandler;
        GameManager.Instance.OnLevelMessage += OnLevelMessageHandler;
        GameManager.Instance.OnPlayerLifeChange += OnPlayerLifeHandler;
        GameManager.Instance.OnFloatingMessage += OnFloatingTextCreator;
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
                Image img = Instantiate(lifeimg);
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
}
