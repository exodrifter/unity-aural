using System;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// Helper for playing hits with an audio clip.
	/// </summary>
	[AddComponentMenu("")] // Don't show in component menu
	[Serializable]
	internal class SoundbankHit : Channel
	{
		[SerializeField]
		private Soundbank soundbank;

		#region Channel

		protected override bool DestroyWhenNoVoicesExist { get { return true; } }

		internal override void Trigger(TransientParams param)
		{
			var source = SpawnTransient();
			var clip = soundbank.GetRandomClip();
			source.Play(clip, false, param);
		}

		#endregion

		#region Constructors

		internal static SoundbankHit New(Soundbank soundbank, Vector3? position)
		{
			var helper = New<SoundbankHit>(soundbank);
			if (position.HasValue)
			{
				helper.transform.position = position.Value;
			}

			helper.soundbank = soundbank;
			return helper;
		}

		#endregion
	}
}
