using UnityEngine;

namespace Exodrifter.Aural
{
	public class AudioClipHitPlayer : AudioPlayer
	{
		[SerializeField]
		private AudioClip clip = default;

		/// <summary>
		/// This hit is only triggered if the channel has less than or equal to
		/// this number of voices. If zero, there is no limit.
		/// </summary>
		[SerializeField]
		private int voiceLimit = 0;

		private void Start()
		{
			if (voiceLimit > 0)
			{
				var channel = Audio.FindChannel<AudioClipHit>(clip);
				if ((channel?.VoiceCount ?? 0) < voiceLimit)
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
