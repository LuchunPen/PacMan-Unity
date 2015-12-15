using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class UIManager: MonoBehaviour
{
    public Image lifeimg;

    public RectTransform LifeContainer;
    private List<Image> _lifesprites;

    public Text PlayerScore;
    public Text HighScore;

    public Text LevelMessage;

	void Start ()
    {
        _lifesprites = new List<Image>();
        GameManager.Instance.OnCurrentScoreChange += OnCurrentScoreHandler;
        GameManager.Instance.OnLevelMessage += OnLevelMessageHandler;
        GameManager.Instance.OnPlayerLifeChange += OnPlayerLifeHandler;
    }
	
	void Update ()
    {
	
	}

    private void OnCurrentScoreHandler(object sender, ScoreArgs args)
    {
        PlayerScore.text = args.Score.ToString();
    }

    private void OnLevelMessageHandler(object sender, LevelMsgArgs args)
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
}
