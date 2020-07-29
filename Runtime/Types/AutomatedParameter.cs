using System;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// An audio parameter which can change its value over time.
	/// </summary>
	[Serializable]
	public class AutomatedParameter
	{
		/// <summary>
		/// The current value of this parameter.
		/// </summary>
		public float CurrentValue
		{
			get { return currentValue; }
		}
		[SerializeField]
		private float currentValue;

		/// <summary>
		/// The desired value of this parameter
		/// </summary>
		public float DesiredValue
		{
			get { return desiredValue; }
			set { desiredValue = value; }
		}
		[SerializeField]
		private float desiredValue;

		/// <summary>
		/// The rate at which this value changes in units per second.
		/// </summary>
		[SerializeField]
		private float transitionSpeed;

		/// <summary>
		/// Creates a new automated parameter.
		/// </summary>
		/// <param name="value">
		/// The initial value for this parameter.
		/// </param>
		/// <param name="speed">
		/// The rate in units per second at which this parameter changes.
		/// </param>
		public AutomatedParameter(float value, float speed)
		{
			currentValue = value;
			desiredValue = value;
			transitionSpeed = speed;
		}

		/// <summary>
		/// Updates the value of this parameter and returns the new value.
		/// </summary>
		/// <param name="time">
		/// The amount of time that has passed since the last update in seconds.
		/// </param>
		public float Update(float delta)
		{
			// The current and desired values are the same
			if (Mathf.Abs(currentValue - desiredValue) < float.Epsilon)
			{
				currentValue = desiredValue;
				return desiredValue;
			}
			// The current value needs to increase
			else if (currentValue < desiredValue)
			{
				currentValue += transitionSpeed * delta;
				if (currentValue > desiredValue)
				{
					currentValue = desiredValue;
				}
				return currentValue;
			}
			// The current value needs to decrease
			else
			{
				currentValue -= transitionSpeed * delta;
				if (currentValue < desiredValue)
				{
					currentValue = desiredValue;
				}
				return currentValue;
			}
		}
	}
}
