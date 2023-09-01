using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class gamecontroller : MonoBehaviour
{
    [Serializable]
    private class MenuPanel
    {
        public GameObject MenuScreen;
        public GameObject LoadingScreen;
    }

    [Serializable]
    private class GamePanel
    {
        public GameObject GameScreen;
        public GameObject letsStartScreen;

        public TMP_Text ScoreText;
        public TMP_Text MovesText;
        public CardManager[] CardsList;

    }

    [Serializable]
    private class GameOverPanel
    {
        public GameObject GameOverScreen;
    }

    [SerializeField]
    private MenuPanel Menu;
    [SerializeField]
    private GamePanel Game;
    [SerializeField]
    private GameOverPanel GameOver;
    [SerializeField]
    private Sprite[] CardFaces;

    public Image PreviousMove,CurrentMove;

    public int CardMoves;
    public int Score;
    
    private bool isGamePlay=false;

    public static gamecontroller Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance=this;
    }

    // Play Button Action
    public void GameStart()
    {
        Menu.MenuScreen.SetActive(false);
        Menu.LoadingScreen.SetActive(true);
        StartCoroutine(LoadingScreen());
    }

    //loading Corotine
    IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(2);
        Menu.LoadingScreen.SetActive(false);
        Game.GameScreen.SetActive(true);
        Game.letsStartScreen.SetActive(true);
        StartCoroutine(LoadCards());
        isGamePlay=true;
    }
    // Go back to menu
    public void GotoMenu()
    {
        Application.LoadLevel(0);
    }

    // Delay to load inscreen cards and coutdown
    IEnumerator LoadCards()
    {
        yield return new WaitForSeconds(2);
        CardManager[] AllCards=FindObjectsOfType<CardManager>();
        List<CardManager> cardfinals=new List<CardManager>();
        foreach (CardManager cards in AllCards)
        {
            if (cards.gameObject.activeInHierarchy)
            {
                cardfinals.Add(cards);
            }
        }
        if (cardfinals.Count % 2 != 0)
        {
            int lastIndex = cardfinals.Count - 1;
            GameObject Lastitem=cardfinals[lastIndex].gameObject;
            Lastitem.SetActive(false);
            cardfinals.RemoveAt(lastIndex);
        }
        ShuffleList(cardfinals);
        Game.CardsList=cardfinals.ToArray();
        AssignSprites(Game.CardsList);
        yield return new WaitForSeconds(1);
        Game.letsStartScreen.SetActive(false);
        PlayShuffleAnimation(Game.CardsList);

    }
    // shuffle all the cards
    System.Random rng = new System.Random();
    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Assign sprites to Objects
    void AssignSprites(CardManager[] Cards)
    {
        int counter=0;
        int RandomNo=0;
        foreach (CardManager card in Cards)
        {
            if(counter==2 || counter==0)
            {
                counter=0;
                RandomNo=UnityEngine.Random.Range(0,CardFaces.Length);
                card.faceSprite=CardFaces[RandomNo];
                counter+=1;
            }
            else
            {
                card.faceSprite=CardFaces[RandomNo];
                counter+=1;
            }
        }
    }
    // play shuffle animation 
    void PlayShuffleAnimation(CardManager[] Cards)
    {
        foreach (CardManager card in Cards)
        {
            StartCoroutine(card.RotateCardAnimation());
        }
    }

    // UpdateScore
    public void AddScore()
    {
        Score+=1;
        Game.ScoreText.text="Matches: "+Score;
    }
    //update moves
    public void AddMoves()
    {
        CardMoves+=1;
        Game.MovesText.text="Moves: "+CardMoves;
    }
    //Check for remaining cards
    public void CheckRemainingCards()
    {
        CardManager[] Cards=Game.CardsList;
        int remaingcards=0;
        foreach (CardManager card in Cards)
        {
            if(card.gameObject.activeInHierarchy)
            remaingcards+=1;
        }
        if(remaingcards==0)
        {
            GameOver.GameOverScreen.SetActive(true);
            StartCoroutine(GameOverSoundEffectDelay());
        }
    }
    // sound effect delay
    IEnumerator GameOverSoundEffectDelay()
    {
        yield return new WaitForSeconds(1f);
        soundmanager.Instance.PlaySoundEffect("over");
    }
}
