using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteHolder", menuName = "ScriptableObjects/Sprite Holder", order = 1)]
public class SpriteHolder : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;
}
