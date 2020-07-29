using UnityEngine;

namespace Exodrifter.Aural
{
	public class SoundbankHitPlayer : AudioPlayer
	{
		public Soundbank Soundbank { get { return soundbank; } }
		[SerializeField]
		private Soundbank soundbank = null;

		private void Start()
		{
			Audio.Hit(soundbank, param, transform.position);
		}
	}
}
