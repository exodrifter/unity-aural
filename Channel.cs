using System;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// A channel is a collection of voices for one audio asset.
	/// </summary>
	[AddComponentMenu("")]
	[Serializable]
	internal abstract class Channel : MonoBehaviour
	{
		#region Automated Parameters

		/// <summary>
		/// The current volume of the channel.
		/// </summary>
		internal float Volume
		{
			get { return volume.CurrentValue; }
			set { volume.DesiredValue = value; }
		}
		[SerializeField]
		private AutomatedParameter volume;

		#endregion

		#region Internal

		internal int VoiceCount { get { return transform.childCount; } }

		/// <summary>
		/// True if the channel should be eagerly destroyed when no voices are
		/// remaining in the channel.
		/// </summary>
		protected abstract bool DestroyWhenNoVoicesExist { get; }

		internal static GameObject Parent
		{
			get
			{
				if (parent == null)
				{
					parent = new GameObject("Audio");
					DontDestroyOnLoad(parent);
				}

				return parent;
			}
		}
		private static GameObject parent;

		/// <summary>
		/// Used by implementing channels to spawn new voices.
		/// </summary>
		private protected Voice SpawnVoice()
		{
			var go = new GameObject("Voice");
			go.transform.parent = transform;
			go.transform.localPosition = Vector3.zero;

			return go.AddComponent<Voice>();
		}

		/// <summary>
		/// Used by implementing channels to spawn themselves under the Audio
		/// GameObject.
		/// </summary>
		/// <typeparam name="T">The type of channel to make.</typeparam>
		/// <param name="obj">
		/// The audio asset that will be used with this channel.
		/// </param>
		/// <returns></returns>
		protected static T New<T>(UnityEngine.Object obj) where T : Channel
		{
			var typeName = typeof(T).GetHashCode();
			var instanceId = obj.GetInstanceID().ToString();

			var go = new GameObject(typeName + " " + instanceId);
			go.transform.parent = Parent.transform;

			var helper = go.AddComponent<T>();
			helper.volume = new AutomatedParameter(1, 0.25f);
			return helper;
		}

		#endregion

		/// <summary>
		/// Starts, restarts, or continues playback of the sound. What action
		/// the helper takes exactly is up to the individual helper.
		/// </summary>
		/// <param name="param">
		/// The new parameters to use for the sound.
		/// </param>
		internal abstract void Trigger(VoiceParams param);

		internal void Update()
		{
			volume.Update(Time.deltaTime);

			// If this channel is no longer audible, destroy it.
			if (volume.CurrentValue == 0)
			{
				Destroy(gameObject);
			}
			// If this channel has no more voices, destroy it.
			else if (DestroyWhenNoVoicesExist && transform.childCount == 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
