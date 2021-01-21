using UnityEngine;

namespace Exodrifter.Aural
{
	public abstract class AudioPlayer : MonoBehaviour
	{
		[SerializeField]
		protected TransientParams param = TransientParams.Variable3D();
	}
}
