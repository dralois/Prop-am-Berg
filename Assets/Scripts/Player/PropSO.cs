using UnityEngine;

[CreateAssetMenu(fileName = "NewObject", menuName = "Props/NewProp")]
public class PropSO : ScriptableObject
{
	public GameObject Prop;
	public Sprite Icon;
}