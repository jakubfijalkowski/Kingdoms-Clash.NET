namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Serializer - klasa pomocnicza do serializowania danych binarnych do formatu używanego przez moduł sieciowy.
	/// </summary>
	public interface IBinarySerializer
	{
		#region Serialization
		/// <summary>
		/// Serializuje obiekty do nowej tablicy.
		/// </summary>
		/// <param name="objs">Obiekty.</param>
		/// <returns></returns>
		byte[] Serialize(params object[] objs);

		/// <summary>
		/// Serializuje obiekty do istniejącej tablicy.
		/// 
		/// Tablica musi mieć odpowiedni rozmiar i nie może być nullem!
		/// </summary>
		/// <param name="output">Tablica.</param>
		/// <param name="objs">Obiekty.</param>
		void Serialize(byte[] output, params object[] objs);
		#endregion

		#region Deserialization
		bool GetBool();
		byte GetByte();
		sbyte GetSByte();
		char GetChar();
		short GetInt16();
		int GetInt32();
		long GetInt64();
		ushort GetUInt16();
		uint GetUInt32();
		ulong GetUInt64();
		double GetDouble();
		float GetFloat();
		string GetString();
		#endregion
	}
}
