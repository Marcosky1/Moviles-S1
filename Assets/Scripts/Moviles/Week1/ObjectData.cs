using System;
using UnityEngine;

namespace Moviles.Week1
{
    [CreateAssetMenu(fileName = "Object Data", menuName = "Scriptable Objects/Moviles/Object Data")]
    public class ObjectData : ScriptableObject
    {
        [SerializeField] private Color currentColor;
        [SerializeField] private Sprite currentSprite;

        public Action<Color> OnColorUpdate;
        public Action<Sprite> OnSpriteUpdate;

        public void UpdateColor(ColorData newColor)
        {
            currentColor = newColor.ColorValue;

            OnColorUpdate?.Invoke(currentColor);
        }

        public void UpdateColor2(Color newColor)
        {
            currentColor = newColor;

            OnColorUpdate?.Invoke(currentColor);
        }

        public void UpdateSprite(Sprite newSprite)
        {
            currentSprite = newSprite;

            OnSpriteUpdate?.Invoke(currentSprite);
        }
    }
}