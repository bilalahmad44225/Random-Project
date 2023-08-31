using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private Image rend;

    [SerializeField]
    public Sprite faceSprite, backSprite;
    [SerializeField]
    private bool coroutineAllowed, facedUp;

    private RectTransform parentCanvas,imageToCheck;

    // Start is called before the first frame update
    void Start()
    {
        parentCanvas = transform.parent.GetComponent<RectTransform>();
        imageToCheck=this.GetComponent<RectTransform>();
        if (IsImageCompletelyInCanvas())
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        rend = GetComponent<Image>();
        rend.sprite = backSprite;
        coroutineAllowed = true;
        facedUp = false;
    }

    private void OnMouseDown() {
        if (coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }
    // rotate card call from button
    public void TurnOverCard()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    private IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        if (!facedUp)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = faceSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            facedUp = !facedUp;
            CheckMove();
        }

        else if (facedUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            facedUp = !facedUp;
        }

        coroutineAllowed = true;

        // facedUp = !facedUp;
    }

    // check if card is in canvas view or not
    private bool IsImageCompletelyInCanvas()
    {
        Rect parentRect = parentCanvas.rect;
        Rect imageRect = imageToCheck.rect;

        Vector3[] imageCorners = new Vector3[4];
        imageToCheck.GetWorldCorners(imageCorners);

        bool fullyContained = true;

        foreach (Vector3 corner in imageCorners)
        {
            Vector3 localCorner = parentCanvas.InverseTransformPoint(corner);
            if (!parentRect.Contains(localCorner))
            {
                fullyContained = false;
                break;
            }
        }

        return fullyContained;
    }

    // card animation at start
    public IEnumerator RotateCardAnimation()
    {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = faceSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1.5f);
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

// Check the move if correct or wrong
    void CheckMove()
    {
        //if(gamecontroller.Instance.PreviousMove==null)
        {
            if(gamecontroller.Instance.CurrentMove!=null && gamecontroller.Instance.CurrentMove.gameObject!=this.gameObject)
            {
                gamecontroller.Instance.PreviousMove=gamecontroller.Instance.CurrentMove;
                gamecontroller.Instance.CurrentMove=this.gameObject.GetComponent<Image>();
            }
            else if(gamecontroller.Instance.CurrentMove==null)
            {
                gamecontroller.Instance.PreviousMove=gamecontroller.Instance.CurrentMove;
                gamecontroller.Instance.CurrentMove=this.gameObject.GetComponent<Image>();
            }
            if(gamecontroller.Instance.PreviousMove!=null && gamecontroller.Instance.CurrentMove.sprite==gamecontroller.Instance.PreviousMove.sprite && gamecontroller.Instance.CurrentMove.gameObject!=gamecontroller.Instance.PreviousMove.gameObject)
            {
                gamecontroller.Instance.AddScore();
                gamecontroller.Instance.AddMoves();
                gamecontroller.Instance.CurrentMove.gameObject.SetActive(false);
                gamecontroller.Instance.PreviousMove.gameObject.SetActive(false);
                gamecontroller.Instance.CurrentMove=null;
                gamecontroller.Instance.PreviousMove=null;
                gamecontroller.Instance.CheckRemainingCards();
            }
            else if(gamecontroller.Instance.PreviousMove!=null && gamecontroller.Instance.CurrentMove.sprite!=gamecontroller.Instance.PreviousMove.sprite && gamecontroller.Instance.CurrentMove.gameObject!=gamecontroller.Instance.PreviousMove.gameObject)
            {
                // gamecontroller.Instance.AddScore();
                gamecontroller.Instance.AddMoves();
                StartCoroutine(gamecontroller.Instance.PreviousMove.gameObject.GetComponent<CardManager>().RotateCardBack());
                StartCoroutine(RotateCardBack());
                gamecontroller.Instance.CurrentMove=null;
                gamecontroller.Instance.PreviousMove=null;
            }
        }
    }

    //Rotateback if wrong attempt for previous card
    public IEnumerator RotateCardBack()
    {
        coroutineAllowed = false;
        if (facedUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            facedUp = !facedUp;
        }
        coroutineAllowed = true;   
    }
}