using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] LeftWalk;
    public Sprite[] DownWalk;
    public Sprite[] UpWalk;
    public Sprite[] RightWalk;
    int c = 0;
    public Vector3 lastMove;
    public string lastDir;
    SpriteRenderer sr;
    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(changer());
    }
    bool reversing = false;
    IEnumerator changer()
    {
        while (true)
        {
            if (lastMove.magnitude >= 0.01)
            {
                if (!reversing)
                {
                    ++c;
                }
                else
                {
                    --c;
                }
                if (c >= LeftWalk.Length)
                {
                    reversing = true;
                    --c;
                    --c;
                }
                if (c < 0)
                {
                    reversing = false;
                    ++c;
                    ++c;
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                c = 0;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    void FixedUpdate()
    {
        if (lastMove.x < 0 && -lastMove.x >= Mathf.Abs(lastMove.y))
        {
            sr.sprite = LeftWalk[c];
            lastDir = "Left";
        }
        else if (lastMove.x > 0 && lastMove.x >= Mathf.Abs(lastMove.y))
        {
            sr.sprite = RightWalk[c];
            lastDir = "Right";
        }
        else if (lastMove.y > 0 && lastMove.y >= Mathf.Abs(lastMove.x))
        {
            sr.sprite = UpWalk[c];
            lastDir = "Up";
        }
        else if (lastMove.y < 0 && -lastMove.y >= Mathf.Abs(lastMove.x))
        {
            sr.sprite = DownWalk[c];
            lastDir = "Down";
        }
    }
}
