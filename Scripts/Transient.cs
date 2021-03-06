﻿using System;
using UnityEngine;

namespace Exodrifter.Aural
{
	/// <summary>
	/// Responsible for playing an audio file and automatically destroying
	/// itself when the audio finishes playing.
	/// </summary>
	[AddComponentMenu("")]
	[Serializable]
	internal sealed class Transient : MonoBehaviour
	{
		#region Automated Parameters

		/// <summary>
		/// The current volume of the transient.
		/// </summary>
		internal float Volume
		{
			get { return volume.CurrentValue; }
			set { volume.DesiredValue = value; }
		}
		[SerializeField]
		private AutomatedParameter volume;

		/// <summary>
		/// The current pitch of the transient.
		/// </summary>
		internal float Pitch
		{
			get { return pitch.CurrentValue; }
			set { pitch.DesiredValue = value; }
		}
		[SerializeField]
		private AutomatedParameter pitch;

		/// <summary>
		/// The current stereo panning of the transient between 0 and 1, where 0
		/// is left and 1 is right.
		/// </summary>
		internal float PanStereo
		{
			get { return panStereo.CurrentValue; }
			set { panStereo.DesiredValue = value; }
		}
		[SerializeField]
		private AutomatedParameter panStereo;

		/// <summary>
		/// The current spatial blend of the transient between 0 and 1, where 0
		/// is 2D and 1 is 3D.
		/// </summary>
		internal float SpatialBlend
		{
			get { return spatialBlend.CurrentValue; }
			set { spatialBlend.DesiredValue = value; }
		}
		[SerializeField]
		private AutomatedParameter spatialBlend;

		#endregion

		#region Psuedo-Constructors

		private AudioSource Source
		{
			get
			{
				AudioSource source;
				if (gameObject.TryGetComponent(out source))
				{
					return source;
				}

				return gameObject.AddComponent<AudioSource>();
			}
		}

		internal Transient Play(AudioClip clip, bool loop, TransientParams param)
		{
			volume = new AutomatedParameter(param.GetVolume(), 0.25f);
			pitch = new AutomatedParameter(param.GetPitch(), 0.25f);
			remainingLeftSilence = param.GetLeftSilenceLength();
			remainingRightSilence = param.GetRightSilenceLength();
			panStereo = new AutomatedParameter(param.GetPanStereo(), 0.25f);
			spatialBlend = new AutomatedParameter(param.GetSpatialBlend(), 0.25f);

			Source.clip = clip;
			Source.loop = loop;
			Source.volume = volume.CurrentValue;
			Source.pitch = pitch.CurrentValue;
			Source.panStereo = panStereo.CurrentValue;
			Source.spatialBlend = spatialBlend.CurrentValue;
			Source.outputAudioMixerGroup = param.GetMixerGroup();

			// For some reason, Unity doesn't clamp this value so we need to
			// clamp it ourselves to ensure no exception will be thrown.
			Source.time = Mathf.Clamp(param.GetTime() * clip.length, 0, clip.length);

			// Play the clip right away if there is no silence to generate
			if (remainingLeftSilence == 0)
			{
				Source.Play();
			}
			return this;
		}

		internal Transient Apply(TransientParams param)
		{
			volume.DesiredValue = param.GetVolume();
			pitch.DesiredValue = param.GetPitch();
			this.panStereo.DesiredValue = param.GetPanStereo();
			this.spatialBlend.DesiredValue = param.GetSpatialBlend();
			return this;
		}

		#endregion

		[SerializeField]
		private float remainingLeftSilence;

		[SerializeField]
		private float remainingRightSilence;

		/// <summary>
		/// Returns the amount of remaining time left before the clip finishes
		/// playing.
		/// </summary>
		internal float RemainingTime
		{
			get
			{
				if (Source.clip == null)
				{
					return 0;
				}
				var remainingClipLength = Source.clip.length - Source.time;
				return remainingLeftSilence
					+ remainingClipLength
					+ remainingRightSilence;
			}
		}

		private void Update()
		{
			// Update audio parameters
			var channel = GetComponentInParent<Channel>();
			Source.volume = volume.Update(Time.deltaTime) * channel.Volume;
			Source.pitch = pitch.Update(Time.deltaTime);
			Source.panStereo = panStereo.Update(Time.deltaTime);
			Source.spatialBlend = spatialBlend.Update(Time.deltaTime);

			// Wait until there is no silence remaining before playing the audio
			if (remainingLeftSilence > 0)
			{
				remainingLeftSilence -= Time.deltaTime;
				if (remainingLeftSilence < 0)
				{
					remainingLeftSilence = 0;
					Source.Play();
				}
			}

			// Wait until there is no silence remaining after playing the audio
			if (remainingLeftSilence == 0
				&& !Source.isPlaying
				&& remainingRightSilence > 0)
			{
				remainingRightSilence -= Time.deltaTime;
				if (remainingRightSilence < 0)
				{
					remainingRightSilence = 0;
				}
			}

			// If we can no longer hear this voice and there is no more silence
			// to generate, destroy it.
			var audible = Source.isPlaying && Source.volume != 0;
			if (remainingLeftSilence <= 0 && remainingRightSilence <= 0 && !audible)
			{
				Destroy(gameObject);
			}
		}
	}
}