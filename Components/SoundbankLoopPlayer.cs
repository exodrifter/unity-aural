using UnityEngine;

namespace Exodrifter.Aural
{
	public class SoundbankLoopPlayer : AudioPlayer
	{
		[SerializeField]
		private Soundbank soundbank = null;

		private void Start()
		{
			Audio.Loop(soundbank, param, transform.position);
		}
	}
}
