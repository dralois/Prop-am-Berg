using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> : System.IDisposable where T : Component
{

	#region Fields

	private Queue<T> _pool;
	private List<T> _allCreated;

	private T[] _prefabs;
	private bool _exactPool;
	private int _addCapacity;
	private bool _hideInInspector;

	#endregion

	#region Methods

	/// <summary>
	/// Gibt gepooltes Objekt (erstellt ggf. Neue)
	/// </summary>
	public T GetObject()
	{
		// Bei einem exakten Pool kann ggf. nichts dequeued werden
		if (_exactPool)
		{
			if(_pool.Count == 0)
			{
				return null;
			}
			else
			{
				return _pool.Dequeue();
			}
		}
		else
		{
			// ggf. neue Objekte erstellen
			if(_pool.Count == 0)
			{
				X_CreateObjects(_addCapacity);
			}
			// Naechstes zurueck
			return _pool.Dequeue();
		}
	}

	/// <summary>
	/// Gibt zufaelliges gepooltes Objekt
	/// </summary>
	public T GetRandomObject()
	{
		// Pool shuffeln falls mindestens zwei Elemente
		if(_pool.Count > 1)
		{
			for(int i = 0; i < Random.Range(0, _pool.Count); i++)
			{
				_pool.Enqueue(_pool.Dequeue());
			}
		}
		// Erstes zurueck
		return GetObject();
	}

	/// <summary>
	/// Gibt <paramref name="returnToPool"/> Objekt zurueck
	/// </summary>
	/// <param name="returnToPool"></param>
	public void ReturnToPool(T returnToPool)
	{
		// Deaktivieren und in den Pool
		returnToPool.gameObject.SetActive(false);
		_pool.Enqueue(returnToPool);
	}

	/// <summary>
	/// Erstellt <paramref name="count"/> neue Objekte
	/// </summary>
	/// <param name="count"></param>
	private void X_CreateObjects(int count)
	{
		// Neue Objekte erstellen
		for (int i = 0; i < count; i++)
		{
			T newObject;
			// Prefab oder neues GameObjekt erstellen
			if (_prefabs != null)
			{
				// Falls exakter Pool dann alle Prefabs instantiieren
				if (_exactPool)
				{
					newObject = Object.Instantiate(_prefabs[i]);
				}
				else
				{
					newObject = Object.Instantiate(_prefabs[Random.Range(0, _prefabs.Length)]);
				}
			}
			else
			{
				newObject = new GameObject(nameof(T) + "_pooled").AddComponent<T>();
			}
			// Deaktivieren und in die Queue
			newObject.gameObject.SetActive(false);
			newObject.gameObject.hideFlags = _hideInInspector ? HideFlags.HideAndDontSave : HideFlags.DontSave;
			_pool.Enqueue(newObject);
			_allCreated.Add(newObject);
		}
	}

	/// <summary>
	/// Entfernt alle Objekte des Pools
	/// </summary>
	public void Dispose()
	{
		// Liste durchlaufen und loeschen
		for(int i = _allCreated.Count - 1; i >= 0; i--)
		{
			T curr = _allCreated[i];
			_allCreated.RemoveAt(i);
			Object.DestroyImmediate(curr.gameObject);
		}
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Neuer GameObject Pool mit Component <typeparamref name="T"/>
	/// </summary>
	/// <param name="prefabs">Zu poolende Prefabs</param>
	/// <param name="exactPool">ggf. ob Pool exakt diese Prefabs enthalten soll</param>
	/// <param name="hideInInspector">ggf. im Editor verstecken</param>
	/// <param name="initialCapacity">ggf. mit bestimmter Kapazitaet starten</param>
	/// <param name="addCapacity">ggf. wieviele Objekte neu erzeugt werden</param>
	public ObjectPool(T[] prefabs, bool exactPool = false, bool hideInInspector = false, int initialCapacity = 1, int addCapacity = 1)
	{
		// Cachen
		_prefabs = prefabs;
		_exactPool = exactPool;
		_addCapacity = _exactPool ? 0 : addCapacity;
		_hideInInspector = hideInInspector;
		// Pool erstellen
		_pool = new Queue<T>();
		_allCreated = new List<T>();
		// Pool aufwaermen
		X_CreateObjects(_exactPool ? _prefabs.Length : initialCapacity);
	}

	/// <summary>
	/// Neuer GameObject Pool mit Component <typeparamref name="T"/>
	/// </summary>
	/// <param name="prefab">ggf. zu poolendes Prefab</param>
	/// <param name="hideInInspector">ggf. im Editor verstecken</param>
	/// <param name="initialCapacity">ggf. mit bestimmter Kapazitaet starten</param>
	/// <param name="addCapacity">ggf. wieviele Objekte neu erzeugt werden</param>
	public ObjectPool(T prefab = null, bool hideInInspector = false, int initialCapacity = 1, int addCapacity = 1)
	{
		// Cachen
		_prefabs = prefab == null? null : new T[] { prefab };
		_addCapacity = addCapacity;
		_hideInInspector = hideInInspector;
		// Pool erstellen
		_pool = new Queue<T>();
		_allCreated = new List<T>();
		// Pool aufwaermen
		X_CreateObjects(initialCapacity);
	}


	#endregion

}
