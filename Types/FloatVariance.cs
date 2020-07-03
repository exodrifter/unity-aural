using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// A class that represents a range of random numbers.
	/// </summary>
	[System.Serializable]
	public class FloatVariance
	{
		[SerializeField, Tooltip("The base value")]
		private float number;
		[SerializeField, Tooltip("The maximum delta from the base value.")]
		private float variance;

		private FloatVariance(float number, float variance)
		{
			this.number = number;
			this.variance = variance;
		}

		public static FloatVariance Const(float value)
		{
			return new FloatVariance(value, 0);
		}

		public static FloatVariance Range(float min, float max)
		{
			var mid = (min + max) / 2;
			var var = Mathf.Abs(max - mid);
			return new FloatVariance(mid, var);
		}

		public static FloatVariance Variance(float number, float variance)
		{
			return new FloatVariance(number, variance);
		}

		public float GenerateValue()
		{
			return Random.Range(number - variance, number + variance);
		}

		public override string ToString()
		{
			return "{ number=" + number + ", variance=" + variance + " }";
		}
	}
}