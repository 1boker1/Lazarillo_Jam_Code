using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public List<Transform> checkPoints = new List<Transform>();
    public int actualCheckPoint = 0;
    public float speed = 10;

    public ParticleSystem particles;

    public CircleCollider2D ballCollider;
    public SpriteRenderer ballSprite;

    public GameObject sliderSprite;

    public float alertPerSecond;
    public float failDistance = 2f;

    public bool started = false;
    public bool finished = false;

    public bool inside = true;

    public OpenBag openBag;
    public ClosedBag closedBag;



    void Update()
    {
        if (started && !finished)
        {
            MoveBall();

            if (Input.GetMouseButton(0))
            {
                CheckMousePoisiton();
            }
        }
    }

    private void OnMouseDown()
    {
        if (openBag != null && openBag.isSliding) return;
        if (closedBag != null && closedBag.isSliding) return;

        started = true;
        if (openBag != null) openBag.isSliding = true;
        if (closedBag != null) closedBag.isSliding = true;

        sliderSprite.SetActive(true);
    }

    private void OnMouseUp()
    {
        if (started && !finished)
        {
            if (TutorialScript.instance != null) //Only On Tutorial
            {
                Debug.Log("kek");
                AlertIndicator.instance.actualAlert = 95;
                AlertIndicator.instance.ChangeColor(95);
                TutorialScript.instance.PassTutorial();

                return;
            }

            //Normal
            AlertIndicator.instance.actualAlert = 100;
        }
    }

    private void OnMouseExit()
    {
        inside = false;
    }

    private void OnMouseEnter()
    {
        inside = true;
    }

    public void CheckMousePoisiton()
    {
        Vector2 mousePoisiton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ballPosition = new Vector2(ballSprite.transform.position.x, ballSprite.transform.position.y);

        if (inside)
        {
            particles.Play();
            return;
        }

        particles.Stop();

        if ((mousePoisiton - ballPosition).magnitude - ballSprite.size.x / 2 < failDistance)
        {
            Vector2 direction = (mousePoisiton - ballPosition).normalized;

            Vector2 firstPoint = ballPosition + (direction * ballSprite.size.x / 2);
            Vector2 secondPoint = ballPosition + (direction * failDistance);

            float maxDistance = (secondPoint - firstPoint).magnitude;
            float actualDistance = (mousePoisiton - firstPoint).magnitude;

            float multiplier = maxDistance - actualDistance;

            float alert = (alertPerSecond * multiplier) * Time.deltaTime;

            AlertIndicator.instance.AddAlertPoints(alert);
        }
        else
        {
            AlertIndicator.instance.AddAlertPoints(100);
        }
    }

    public void MoveBall()
    {
        transform.position = Vector3.MoveTowards(transform.position, checkPoints[actualCheckPoint].position, speed * Time.deltaTime);
        particles.transform.forward = -checkPoints[actualCheckPoint].forward;
        if (transform.position == checkPoints[actualCheckPoint].position)
        {
            if (actualCheckPoint == checkPoints.Count - 1)
            {
                finished = true;
                if (openBag != null) openBag.isSliding = false;
                if (closedBag != null) closedBag.isSliding = false;
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                actualCheckPoint++;
            }
        }
    }
}
