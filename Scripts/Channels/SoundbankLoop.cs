using System;
using System.Collections;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// Helper for playing random sounds from a soundbank in a loop.
	/// </summary>
	[AddComponentMenu("")] // Don't show in component menu
	[Serializable]
	internal class SoundbankLoop : Channel
	{
		[SerializeField]
		private Soundbank soundbank;

		[SerializeField]
		private TransientParams currentParams;
		[SerializeField]
		private Coroutine loop;

		#region Channel

		protected override bool DestroyWhenNoVoicesExist { get { return false; } }

		internal override void Trigger(TransientParams param)
		{
			currentParams = param;

			if (loop == null)
			{
				loop = StartCoroutine(Loop());
			}
		}

		private IEnumerator Loop()
		{
			while (true)
			{
				var source = SpawnTransient();
				source.Play(soundbank.GetRandomClip(), false, currentParams);
				yield return new WaitForSeconds(source.RemainingTime);
			}
		}

		#endregion

		#region Constructors

		internal static SoundbankLoop New(Soundbank soundbank, Vector3? position)
		{
			var helper = New<SoundbankLoop>(soundbank);
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
