using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    Renderer myRenderer = null;
    bool armed = true;

    [SerializeField]
    float maxTimeUntilDetonation = 0.0f;
    float timeUntilDetonation = 0.0f;
    [SerializeField]
    float dangerTime = 0.0f;
    //Will tick down from maxTime, when it reaches below dangerTime it will start a coroutine to flash the bomb

    bool hasEnteredDangerState = false;
    Material myMaterial = null;
    [SerializeField]
    Material dangerMaterial = null;

    [SerializeField]
    float moveSpeed = 0.0f;

    Rigidbody myRB = null;

    private void Awake()
    {
        myRenderer = transform.Find("Graphics").GetComponent<Renderer>();
        timeUntilDetonation = maxTimeUntilDetonation;
        myMaterial = myRenderer.material;
        myRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(armed)
        {
            if((timeUntilDetonation -= Time.deltaTime) <= dangerTime && !hasEnteredDangerState)
            {
                hasEnteredDangerState = true;
                StartCoroutine(StartDangerState());
            }
            else if(timeUntilDetonation <= 0.0f)
            {
                DetonateWithTime();
            }

        }
    }

    public bool IsArmed()
    {
        return armed;
    }

    Renderer GetMyRenderer()
    {
        return myRenderer;
    }

    public void SetBombMaterial(Material mat)
    {
        if (myRenderer != null)
            myRenderer.material = mat;
    }

    public void SetMyMaterial(Material mat)
    {
        myMaterial = mat;
    }

    IEnumerator StartDangerState()
    {
        int time = 0;
        int mul = 5;
        while(true)
        {
            if(time != (int)(timeUntilDetonation*mul))
            {
                time = (int)(timeUntilDetonation * mul);
                SetBombMaterial(myRenderer.material.name.Contains(myMaterial.name)? dangerMaterial : myMaterial);
            }
            yield return null;
        }
    }

    public Color GetMaterialColor()
    {
        return myRenderer.material.color;
    }

    void DetonateWithWrongLocation()
    {
        //NEEDS TO DETONATE MORE THAN JUST HIMSELF
        //TRIGGER END GAME
        //WILL USE BOMB MANAGER FOR THIS MOST LIKELY
        ScoreKeeper.EndGame();
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bomb in bombs)
        {
            Bomb b = bomb.GetComponent<Bomb>();
            if (b.IsArmed() && armed)
            {
                Destroy(bomb);
            }
            else if (!b.IsArmed() && b.GetMaterialColor() != GetMaterialColor())
            {
                ScoreKeeper.AddToScore(-1);
                Destroy(bomb);
            }
        }
        Detonate();
    }

    void DetonateWithTime()
    {
        //NEEDS TO DETONATE MORE THAN JUST HIMSELF
        //TRIGGER END GAME
        //WILL USE BOMB MANAGER FOR THIS MOST LIKELY
        ScoreKeeper.EndGame();
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach(GameObject bomb in bombs)
        {
            Bomb b = bomb.GetComponent<Bomb>();
            if (b.IsArmed() && armed)
            {
                Destroy(bomb);
            }
        }
        Detonate();
    }

    void Detonate()
    {
        Destroy(gameObject);
    }

    public void StartMoving()
    {
        transform.Rotate(new Vector3(0.0f, Random.Range(-45.0f, 45.0f), 0.0f));
        myRB.velocity = transform.forward * moveSpeed;
    }

    public void ToggleGravity()
    {
        myRB.useGravity = !myRB.useGravity;
    }

    public void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                Landed();
                break;
            case "Defuse":
                Landed();
                if(collision.gameObject.GetComponent<Renderer>().material.color == myMaterial.color)
                {
                    HandleDefuse();
                }
                else
                {
                    DetonateWithWrongLocation();
                }
                break;
            default:
                break;
        }
    }

    void Landed()
    {
        myRB.velocity = new Vector3(myRB.velocity.x, 0.0f, myRB.velocity.z);
        //set random x and z velocity instead?
    }

    void HandleDefuse()
    {
        if (armed)
        {
            armed = false;
            SetBombMaterial(myMaterial);
            StopCoroutine(StartDangerState());
            ScoreKeeper.AddToScore(1);
        }
    }
}
