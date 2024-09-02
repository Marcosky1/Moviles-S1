using UnityEngine;
using UnityEngine.Events;

public class PokeBallManager : MonoBehaviour
{
    public PokeBallConfig[] pokeBallConfigs;
    public ColorConfig[] colorConfigs;
    public UnityEvent<Vector2> OnPokeBallSpawn;
    public UnityEvent OnAllPokeBallsDeleted;

    private PokeBallConfig selectedConfig;
    private Color selectedColor;

    public void SelectPokeBall(int index)
    {
        if (index >= 0 && index < pokeBallConfigs.Length)
        {
            selectedConfig = pokeBallConfigs[index];
        }
    }

    public void SelectColor(int index)
    {
        if (index >= 0 && index < colorConfigs.Length)
        {
            selectedColor = colorConfigs[index].color;
        }
    }

    public void SpawnPokeBall(Vector2 position)
    {
        if (selectedConfig != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
            worldPosition.z = 0;

            GameObject pokeBall = Instantiate(selectedConfig.pokeBallPrefab, worldPosition, Quaternion.identity);
            pokeBall.GetComponent<SpriteRenderer>().color = selectedColor;
            OnPokeBallSpawn?.Invoke(position);
        }
    }

    public void DeleteAllPokeBalls()
    {
        OnAllPokeBallsDeleted?.Invoke();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PokeBall"))
        {
            Destroy(obj);
        }
    }
}



