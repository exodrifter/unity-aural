using System;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// Helper for playing loops with an audio clip.
	/// </summary>
	[AddComponentMenu("")] // Don't show in component menu
	[Serializable]
	internal class AudioClipLoop : Channel
	{
		[SerializeField]
		private AudioClip clip;

		private Voice source;

		#region Channel

		protected override bool DestroyWhenNoVoicesExist { get { return true; } }

		internal override void Trigger(VoiceParams param)
		{
			// Start playback of the audio
			if (source == null)
			{
				source = SpawnVoice();
				source.Play(clip, true, param);
			}
			// Apply the new parameters
			else
			{
				source.Apply(param);
			}
		}

		#endregion

		#region Constructors

		internal static AudioClipLoop New(AudioClip clip, Vector3? position)
		{
			var helper = New<AudioClipLoop>(clip);
			if (position.HasValue)
			{
				helper.transform.position = position.Value;
			}

			helper.clip = clip;
			return helper;
		}

		#endregion
	}
}
