using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewObject", menuName = "Props/NewProp")]
public class ScruptableObjects : ScriptableObject
{
    public GameObject _prop;
    public Sprite _img;
}