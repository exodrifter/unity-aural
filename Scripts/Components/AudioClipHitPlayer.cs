using UnityEngine;
using UnityEngine.Serialization;

namespace Exodrifter.Aural
{
	public class AudioClipHitPlayer : AudioPlayer
	{
		public AudioClip Clip { get { return clip; } }
		[SerializeField]
		private AudioClip clip = default;

		/// <summary>
		/// This hit is only triggered if the channel has less than or equal to
		/// this number of voices. If zero, there is no limit.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("voiceLimit")]
		private int limit = 0;

		private void Start()
		{
			if (limit > 0)
			{
				var channel = Audio.FindChannel<AudioClipHit>(clip);
				if ((channel?.TransientCount ?? 0) < limit)
				{
					Audio.Hit(clip, param, transform.position);
				}
			}
			else
			{
				Audio.Hit(clip, param, transform.position);
			}
		}
	}
}
