using UnityEngine;

namespace Exodrifter.Aural
{
	public class AudioClipLoopPlayer : AudioPlayer
	{
		public AudioClip Clip { get { return clip; } }
		[SerializeField]
		private AudioClip clip = default;

		private void Start()
		{
			Audio.Loop(clip, param, transform.position);
		}
	}
}
