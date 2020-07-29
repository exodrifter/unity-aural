using UnityEngine;

namespace Exodrifter.Aural
{
	public abstract class AudioPlayer : MonoBehaviour
	{
		[SerializeField]
		protected VoiceParams param = VoiceParams.Variable3D();
	}
}
