using UnityEngine;

public class PokeBallSpawner : MonoBehaviour
{
    public GameObject[] pokeBalls;
    public Color[] colors;
    private GameObject selectedPokeBall;
    private Color selectedColor;
    private float lastTapTime = 0;
    public float doubleTapThreshold = 0.3f;
    private GameObject draggedObject = null;
    private Vector2 swipeStartPos;
    private bool isSwiping = false;

    public GameObject trailEffectPrefab;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    swipeStartPos = touchPosition;
                    isSwiping = false;

                    if (Time.time - lastTapTime < doubleTapThreshold)
                    {
                        DeleteObject(touchPosition);
                    }
                    else
                    {
                        if (selectedPokeBall != null)
                        {
                            SpawnPokeBall(touchPosition);
                        }

                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane)), Vector2.zero);
                        if (hit.collider != null)
                        {
                            draggedObject = hit.collider.gameObject;
                        }

                        lastTapTime = Time.time;
                    }
                    break;

                case TouchPhase.Moved:
                    if (draggedObject != null)
                    {
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
                        worldPosition.z = 0;
                        draggedObject.transform.position = worldPosition;
                    }

                    // Detectar el swipe
                    if (Vector2.Distance(touchPosition, swipeStartPos) > 20) 
                    {
                        isSwiping = true;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        CreateTrailEffect(touchPosition);
                        DeleteAllObjects();
                    }

                    draggedObject = null;
                    break;
            }
        }
    }

    public void SelectPokeBall(int index)
    {
        selectedPokeBall = pokeBalls[index];
    }

    public void SelectColor(int index)
    {
        selectedColor = colors[index];
    }

    void SpawnPokeBall(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject newPokeBall = Instantiate(selectedPokeBall, worldPosition, Quaternion.identity);
        newPokeBall.GetComponent<SpriteRenderer>().color = selectedColor;
    }

    public void DeleteObject(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }
    }

    void DeleteAllObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PokeBall")) 
        {
            Destroy(obj);
        }
    }

    void CreateTrailEffect(Vector2 swipeEndPos)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(swipeEndPos.x, swipeEndPos.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        if (trailEffectPrefab != null)
        {
            GameObject trailEffect = Instantiate(trailEffectPrefab, worldPosition, Quaternion.identity);
            TrailRenderer trailRenderer = trailEffect.GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.startColor = selectedColor;
                trailRenderer.endColor = selectedColor;
            }

            Destroy(trailEffect, 1f); 
        }
    }
}
