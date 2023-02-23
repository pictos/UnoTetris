using System;
using System.Collections.Concurrent;

namespace UnoTetris.Services;
public static class DependencyService
{
	static readonly ConcurrentDictionary<Type, object> dependencyImplementations = new();

	public static void RegisterSingleton<T, TImpl>(TImpl instance)
			where T : class
			where TImpl : class, T
	{
		var type = typeof(T);

		if (dependencyImplementations.ContainsKey(type))
			return;

		dependencyImplementations.TryAdd(type, instance);
	}

	public static T? Get<T>()
		where T : class
	{
		if (dependencyImplementations.TryGetValue(typeof(T), out var instance))
		{
			return (T)instance;
		}

		return null;
	}

	class DependencyData
	{
		public object GlobalInstance { get; set; }

		public Type ImplementorType { get; set; }
	}

}

