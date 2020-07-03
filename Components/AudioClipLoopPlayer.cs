using UnityEngine;

namespace Exodrifter.Aural
{
	public class AudioClipLoopPlayer : AudioPlayer
	{
		[SerializeField]
		private AudioClip clip = default;

		private void Start()
		{
			Audio.Loop(clip, param, transform.position);
		}
	}
}
