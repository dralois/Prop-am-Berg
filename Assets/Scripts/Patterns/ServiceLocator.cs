using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Service Provider Interface
/// </summary>
/// <typeparam name="V">Datentyp (Struct)</typeparam>
public interface Service<V> where V : struct
{
	/// <summary>
	/// Setze neue Daten <paramref name="data"/> von Typ <typeparamref name="V"/> von aussen
	/// </summary>
	/// <param name="data">Zu setzende Daten</param>
	void SetData(V data);

	/// <summary>
	/// Hole Daten von Typ <typeparamref name="V"/> von aussen
	/// </summary>
	/// <returns>Daten von Typ <typeparamref name="V"/></returns>
	V GetData();
}

/// <summary>
/// Service Locator Implementierung
/// </summary>
/// <typeparam name="V">Datentyp (Struct)</typeparam>
/// <typeparam name="E">Identifier (Enum)</typeparam>
public static class ServiceLocator<V, E> where V : struct where E : System.Enum
{

	#region Fields

	private static Dictionary<E, Service<V>> _serviceContainer = new Dictionary<E, Service<V>>();
	private static readonly object _lock = new object();

	#endregion

	#region Events

	public static event System.Action<E> OnServiceAdded;
	public static event System.Action<E> OnServiceRemoved;

	#endregion

	#region Methods

	/// <summary>
	/// Findet Service Provider zu Identifier <paramref name="target"/>
	/// </summary>
	/// <param name="target">Identifier (Enum)</param>
	/// <returns>Service Provider oder null</returns>
	public static Service<V> GetService(E target)
	{
		Service<V> service;
		// Falls Service fuer Identifier verfuegbar ist, dann zurueckgeben
		if(_serviceContainer.TryGetValue(target, out service))
		{
			return service;
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// <paramref name="provider"/> bietet Service fuer Identifier <paramref name="target"/> an
	/// </summary>
	/// <param name="provider">Anbieter (Service, MonoBehaviour))</param>
	/// <param name="target">Identifier (Enum)</param>
	public static void ProvideService(Service<V> provider, E target)
	{
		// Falls bereits Service vorhanden
		if (_serviceContainer.ContainsKey(target))
		{
			// Threadsafe ueberschreiben
			lock (_lock)
			{
				Debug.LogWarningFormat("A service for data type {0} was already registered, " +
															 "it will be overwritten by {1}.", typeof(V), provider.GetType());
				_serviceContainer[target] = provider;
				OnServiceAdded?.Invoke(target);
			}
		}
		else
		{
			// Sonst threadsafe hinzufuegen
			lock (_lock)
			{
				_serviceContainer.Add(target, provider);
				OnServiceAdded?.Invoke(target);
			}
		}
	}

	/// <summary>
	/// Service fuer Identifier <paramref name="target"/> zurueckziehen
	/// </summary>
	/// <param name="target">Identifier (Enum)</param>
	public static void WithdrawService(E target)
	{
		// Threadsafe Service entfernen
		lock (_lock)
		{
			_serviceContainer.Remove(target);
			OnServiceRemoved?.Invoke(target);
		}
	}

	#endregion

}
