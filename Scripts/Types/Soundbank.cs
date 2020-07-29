using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exodrifter.Aural
{
	[CreateAssetMenu(fileName = "New Soundbank", menuName = "Aural/Soundbank")]
	public class Soundbank : ScriptableObject
	{
		[SerializeField]
		private List<AudioClip> clips = default;

		public float MaxLength
		{
			get { return clips.Select(x => x.length).Max(); }
		}

		public AudioClip GetRandomClip()
		{
			if (clips == null || clips.Count == 0)
			{
				return null;
			}

			return clips[Random.Range(0, clips.Count)];
		}
	}
}
