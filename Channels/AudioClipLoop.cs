using System;
using System.Collections;
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
		[SerializeField]
		private VoiceParams currentParams;
		[SerializeField]
		private Coroutine loop;

		private Voice source;

		#region Channel

		protected override bool DestroyWhenNoVoicesExist { get { return false; } }

		internal override void Trigger(VoiceParams param)
		{
			currentParams = param;

			// Start playback of the audio
			if (source == null)
			{
				if (param.HasSilencePadding())
				{
					if (loop == null)
					{
						loop = StartCoroutine(Loop());
					}
				}
				else
				{
					source = SpawnVoice();
					source.Play(clip, true, currentParams);
				}
			}
			// Apply the new parameters
			else
			{
				source.Apply(currentParams);
			}
		}

		private IEnumerator Loop()
		{
			while (true)
			{
				source = SpawnVoice();
				source.Play(clip, false, currentParams);
				yield return new WaitForSeconds(source.RemainingTime);
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
