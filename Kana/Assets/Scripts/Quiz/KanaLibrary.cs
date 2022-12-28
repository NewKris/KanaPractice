using System;
using UnityEngine;

namespace VD.Quiz
{
	[CreateAssetMenu(menuName = "Kana Library")]
	public class KanaLibrary : ScriptableObject
	{
		public Kana[] kana;

		public int Count => kana.Length;
		public Kana this[int i] => kana[i];

	}

	[Serializable]
	public struct Kana
	{
		public string symbol;
		public string answer;
	}
	
}