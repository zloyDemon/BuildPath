using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteHolder", menuName = "ScriptableObjects/Sprite Holder", order = 1)]
public class SpriteHolder : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;

    public Sprite GetSpriteByName(string name)
    {
        return sprites.FirstOrDefault(e => string.Equals(e.name, name));
    }
}
