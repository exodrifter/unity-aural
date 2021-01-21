using System;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// Helper for playing hits with an audio clip.
	/// </summary>
	[AddComponentMenu("")] // Don't show in component menu
	[Serializable]
	internal class AudioClipHit : Channel
	{
		[SerializeField]
		private AudioClip clip;

		#region Channel

		protected override bool DestroyWhenNoVoicesExist { get { return true; } }

		internal override void Trigger(TransientParams param)
		{
			var source = SpawnTransient();
			source.Play(clip, false, param);
		}

		#endregion

		#region Constructors

		internal static AudioClipHit New(AudioClip clip, TransientParams param, Vector3? position)
		{
			var helper = New<AudioClipHit>(clip);
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
