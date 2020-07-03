using UnityEngine;

namespace Exodrifter.Aural
{
	public class SoundbankLoopPlayer : AudioPlayer
	{
		public Soundbank Soundbank { get { return soundbank; } }
		[SerializeField]
		private Soundbank soundbank = null;

		private void Start()
		{
			Audio.Loop(soundbank, param, transform.position);
		}
	}
}
