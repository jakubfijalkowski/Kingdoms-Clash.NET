using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Collections
{
	/// <summary>
	/// Lista thread-safe.
	/// </summary>
	/// <remarks>
	/// operator[] nie powinien być thread-safe(nie powinien lockować kolekcji).
	/// Metody <see cref="ISafeList`1.At"/> powinny być thread-safe.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public interface ISafeList<T>
		: ISafeCollection<T>, IList<T>
	{
		/// <summary>
		/// Zwraca obiekt na wskazanym miejscu Z zabezpieczaniem przed odczytem.
		/// </summary>
		/// <param name="idx">Indeks.</param>
		/// <returns>Obiekt.</returns>
		T At(int idx);

		/// <summary>
		/// Zmienia obiekt na wskazanej pozycji Z zabezpieczaniem przed zapisem.
		/// </summary>
		/// <param name="idx">Indeks.</param>
		/// <param name="obj">Obiekt do ustawienia.</param>
		/// <returns>Nowy obiekt.</returns>
		T At(int idx, T obj);
	}
}
