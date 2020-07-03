using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Exodrifter.Aural
{
	/// <summary>
	/// A collection of parameters that can be applied to a <see cref="Voice"/>.
	/// </summary>
	[Serializable]
	public class VoiceParams
	{
		[SerializeField]
		private FloatVariance volume = FloatVariance.Range(0.8f, 1.0f);
		[SerializeField]
		private FloatVariance pitch = FloatVariance.Variance(1.0f, 0.1f);
		[SerializeField]
		public FloatVariance normalizedTime = FloatVariance.Const(0.0f);
		[SerializeField]
		private FloatVariance leftSilencePadding = FloatVariance.Const(0.0f);
		[SerializeField]
		private FloatVariance panStereo = FloatVariance.Const(0.0f);
		[SerializeField]
		private FloatVariance spatialBlend = FloatVariance.Const(0.0f);
		[SerializeField]
		private AudioMixerGroup mixerGroup = default;

		private VoiceParams(Factory f)
		{
			volume = f.volume;
			pitch = f.pitch;
			normalizedTime = f.normalizedTime;
			leftSilencePadding = f.leftSilencePadding;
			panStereo = f.panStereo;
			spatialBlend = f.spatialBlend;
			mixerGroup = f.mixerGroup;
		}

		public float GetVolume()
		{
			return volume.GenerateValue();
		}

		public float GetPitch()
		{
			return pitch.GenerateValue();
		}

		public float GetTime()
		{
			// Wrap the generated value to [0,1)
			return Mathf.Clamp01(normalizedTime.GenerateValue() % 1);
		}

		public float GetLeftSilenceLength()
		{
			return leftSilencePadding.GenerateValue();
		}

		public float GetPanStereo()
		{
			return panStereo.GenerateValue();
		}

		public float GetSpatialBlend()
		{
			return spatialBlend.GenerateValue();
		}

		public AudioMixerGroup GetMixerGroup()
		{
			return mixerGroup;
		}

		#region Factory

		/// <summary>
		/// A utility class used to create a new AudioParams without having
		/// to specify all of the properties by choosing sane defaults.
		/// </summary>
		public class Factory
		{
			internal FloatVariance volume;
			internal FloatVariance pitch;
			internal FloatVariance normalizedTime;
			internal FloatVariance leftSilencePadding;
			internal FloatVariance panStereo;
			internal FloatVariance spatialBlend;
			internal AudioMixerGroup mixerGroup;

			internal Factory(VoiceParams p = null)
			{
				volume = p?.volume ?? FloatVariance.Range(0.8f, 1.0f);
				pitch = p?.pitch ?? FloatVariance.Variance(1.0f, 0.1f);
				normalizedTime = p?.normalizedTime ?? FloatVariance.Const(0.0f);
				leftSilencePadding = p?.leftSilencePadding ?? FloatVariance.Const(0.0f);
				panStereo = p?.panStereo ?? FloatVariance.Const(0.0f);
				spatialBlend = p?.spatialBlend ?? FloatVariance.Const(0.0f);
				mixerGroup = p?.mixerGroup;
			}

			/// <summary>
			/// The volume of the voice. 1 is the default volume of the audio
			/// and 0 is muted.
			/// </summary>
			/// <param name="volume">The volume.</param>
			/// <returns>The factory, for chaining.</returns>
			public Factory Volume(FloatVariance volume)
			{
				this.volume = volume;
				return this;
			}

			/// <summary>
			/// The pitch of the voice.
			/// TODO: How do changes in the pitch value map to semitone changes?
			/// </summary>
			/// <param name="time">The pitch.</param>
			/// <returns>The factory, for chaining.</returns>
			public Factory Pitch(FloatVariance pitch)
			{
				this.pitch = pitch;
				return this;
			}

			/// <summary>
			/// The normalized, initial playback position for the voice. 0 is
			/// the beginning of the audio and 1 is the end of the audio.
			/// Generated values will be wrapped to the range [0, 1). If the
			/// voice has already started playing, this parameter will have no
			/// effect.
			/// </summary>
			/// <param name="normalizedTime">
			/// The normalized playback position.
			/// </param>
			/// <returns>The factory, for chaining.</returns>
			public Factory NormalizedTime(FloatVariance normalizedTime)
			{
				this.normalizedTime = normalizedTime;
				return this;
			}

			/// <summary>
			/// The amount of silence in seconds that should be added to the
			/// beginning of a voice before it actually generates sound.
			/// </summary>
			/// <param name="leftSilencePadding">
			/// The amount of silence in seconds.
			/// </param>
			/// <returns>The factory, for chaining.</returns>
			public Factory LeftSilencePadding(FloatVariance leftSilencePadding)
			{
				this.leftSilencePadding = leftSilencePadding;
				return this;
			}

			/// <summary>
			/// Pans a playing sound in a stereo way between the left and right
			/// speakers. This only affects mono and stereo sounds. Stereo
			/// panning affects the left-right balance of the sound before it is
			/// spatialised in 3D.
			/// </summary>
			/// <param name="panStereo">
			/// The panning of the voice.
			/// </param>
			/// <returns>The factory, for chaining.</returns>
			public Factory PanStereo(FloatVariance panStereo)
			{
				this.panStereo = panStereo;
				return this;
			}

			/// <summary>
			/// Sets how much the voice is affected by 3D spatialisation
			/// calculations. 0 makes the audio full 2D, 1 makes it full 3D.
			/// </summary>
			/// <param name="spatialBlend">
			/// The spatial blend of the voice.
			/// </param>
			/// <returns>The factory, for chaining.</returns>
			public Factory SpatialBlend(FloatVariance spatialBlend)
			{
				this.spatialBlend = spatialBlend;
				return this;
			}

			/// <summary>
			/// Sets which mixer group the voice's output will be routed to.
			/// </summary>
			/// <param name="mixerGroup">
			/// The mixer group to route output to.
			/// </param>
			/// <returns>The factory, for chaining.</returns>
			public Factory MixerGroup(AudioMixerGroup mixerGroup)
			{
				this.mixerGroup = mixerGroup;
				return this;
			}

			/// <summary>
			/// Converts the factory into an <see cref="VoiceParams"/>.
			/// </summary>
			/// <param name="factory">The factory to convert.</param>
			public static implicit operator VoiceParams(Factory factory)
			{
				return new VoiceParams(factory);
			}
		}

		/// <summary>
		/// Creates a new factory initialized with either the default values or
		/// with the values copied from another <see cref="VoiceParams"/>.
		/// </summary>
		/// <param name="audioParams">
		/// The <see cref="VoiceParams"/> to copy the initial state from.
		/// </param>
		/// <returns>The factory to modify.</returns>
		public static Factory Clone(VoiceParams audioParams)
		{
			return new Factory(audioParams);
		}

		/// <summary>
		/// Creates a new factory initialized with variable volume and pitch for
		/// a 2D voice.
		/// </summary>
		/// <param name="volume">
		/// The volume of the audio. A variation of 0.1f will be applied to it.
		/// </param>
		/// <param name="pitch">
		/// The pitch of the audio. A variation of 0.1f will be applied to it.
		/// </param>
		/// <returns>The factory to modify.</returns>
		public static Factory Variable2D(float volume = 0.9f, float pitch = 1)
		{
			return new Factory()
				.Volume(FloatVariance.Variance(volume, 0.1f))
				.Pitch(FloatVariance.Variance(pitch, 0.1f))
				.SpatialBlend(FloatVariance.Const(0));
		}

		/// <summary>
		/// Creates a new factory initialized with constant volume and pitch for
		/// a 2D voice.
		/// </summary>
		/// <param name="volume">The volume of the audio.</param>
		/// <param name="pitch">The pitch of the audio.</param>
		/// <returns>The factory to modify.</returns>
		public static Factory Const2D(float volume = 1, float pitch = 1)
		{
			return new Factory()
				.Volume(FloatVariance.Const(volume))
				.Pitch(FloatVariance.Const(pitch))
				.SpatialBlend(FloatVariance.Const(0));
		}

		/// <summary>
		/// Creates a new factory initialized with variable volume and pitch for
		/// a 3D voice.
		/// </summary>
		/// <param name="volume">
		/// The volume of the audio. A variation of 0.1f will be applied to it.
		/// </param>
		/// <param name="pitch">
		/// The pitch of the audio. A variation of 0.1f will be applied to it.
		/// </param>
		/// <returns>The factory to modify.</returns>
		public static Factory Variable3D(float volume = 0.9f, float pitch = 1)
		{
			return new Factory()
				.Volume(FloatVariance.Variance(volume, 0.1f))
				.Pitch(FloatVariance.Variance(pitch, 0.1f))
				.SpatialBlend(FloatVariance.Const(1));
		}

		/// <summary>
		/// Creates a new factory initialized with constant volume and pitch for
		/// a 3D voice.
		/// </summary>
		/// <param name="volume">The volume of the audio.</param>
		/// <param name="pitch">The pitch of the audio.</param>
		/// <returns>The factory to modify.</returns>
		public static Factory Const3D(float volume = 1, float pitch = 1)
		{
			return new Factory()
				.Volume(FloatVariance.Const(volume))
				.Pitch(FloatVariance.Const(pitch))
				.SpatialBlend(FloatVariance.Const(1));
		}

		#endregion
	}
}
