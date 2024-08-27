using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGameController1 : MonoBehaviour
{
    public GameObject[] shapes;
    public Color[] colors;
    public float swipeThreshold = 50f;
    public TrailRenderer swipeTrail;

    private GameObject selectedShape;
    private Color selectedColor;
    private Vector2 startPosition;
    private bool isDragging = false;

    void Start()
    {
        selectedShape = shapes[0];
        selectedColor = colors[0];
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegan(touch);
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        DragObject(touch);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                    }
                    else
                    {
                        DetectSwipe(touch);
                    }
                    break;
            }
        }
    }

    void HandleTouchBegan(Touch touch)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

        if (touch.tapCount == 1)
        {
            if (hitCollider == null)
            {
                CreateShape(touchPosition);
            }
        }
        else if (touch.tapCount == 2)
        {
            if (hitCollider != null)
            {
                Destroy(hitCollider.gameObject);
            }
        }
        else if (hitCollider != null)
        {
            isDragging = true;
        }

        startPosition = touch.position;
    }

    void CreateShape(Vector2 position)
    {
        GameObject newShape = Instantiate(selectedShape, position, Quaternion.identity);
        newShape.GetComponent<SpriteRenderer>().color = selectedColor;
    }

    void DragObject(Touch touch)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        Collider2D hitCollider = Physics2D.OverlapPoint(startPosition);

        if (hitCollider != null)
        {
            hitCollider.transform.position = touchPosition;
        }
    }

    void DetectSwipe(Touch touch)
    {
        float swipeDistance = (touch.position - startPosition).magnitude;

        if (swipeDistance >= swipeThreshold)
        {
            swipeTrail.transform.position = Camera.main.ScreenToWorldPoint(touch.position);
            swipeTrail.emitting = true;
            Invoke("ClearAllShapes", 0.5f);
        }
        else
        {
            swipeTrail.emitting = false;
        }
    }

    void ClearAllShapes()
    {
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("Shape"))
        {
            Destroy(shape);
        }
        swipeTrail.emitting = false;
    }

    public void SelectShape(int index)
    {
        selectedShape = shapes[index];
    }

    public void SelectColor(int index)
    {
        selectedColor = colors[index];
    }
}
