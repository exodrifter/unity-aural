using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exodrifter.Aural
{
	public static class Audio
	{
		#region Channel Functions

		internal static T FindChannel<T>(Object obj) where T : Channel
		{
			var typeName = typeof(T).GetHashCode();
			var instanceId = obj.GetInstanceID().ToString();

			var child = Channel.Parent.transform.Find(typeName + " " + instanceId);
			return child?.GetComponent<T>();
		}

		internal static List<Channel> AllChannels()
		{
			var children = new List<Channel>();

			var parent = Channel.Parent.transform;
			for (int i = 0; i < parent.childCount; i++)
			{
				var child = parent.GetChild(i).GetComponent<Channel>();
				if (child != null)
				{
					children.Add(child);
				}
			}

			return children;
		}

		#endregion

		#region Hits

		public static void Hit(AudioClip clip)
		{
			Hit(clip, VoiceParams.Variable2D());
		}

		public static void Hit(AudioClip clip, VoiceParams param, Vector3? pos = null)
		{
			// If there is nothing to play, we don't need to do anything.
			if (clip == null)
			{
				return;
			}

			var channel = FindChannel<AudioClipHit>(clip)
				?? AudioClipHit.New(clip, param, pos);
			channel.Volume = 1;
			channel.Trigger(param);
		}

		public static void Hit(Soundbank soundbank)
		{
			Hit(soundbank, VoiceParams.Variable2D());
		}

		public static void Hit(Soundbank soundbank, VoiceParams param, Vector3? pos = null)
		{
			// If there is nothing to play, we don't need to do anything.
			if (soundbank == null)
			{
				return;
			}

			var channel = FindChannel<SoundbankHit>(soundbank)
				?? SoundbankHit.New(soundbank, pos);
			channel.Volume = 1;
			channel.Trigger(param);
		}

		#endregion

		#region Loop

		public static void Loop(AudioClip clip)
		{
			Loop(clip, VoiceParams.Const2D());
		}

		public static void Loop(AudioClip clip, VoiceParams param, Vector3? pos = null)
		{
			// If there is nothing to play, we don't need to do anything.
			if (clip == null)
			{
				return;
			}

			var channel = FindChannel<AudioClipLoop>(clip)
				?? AudioClipLoop.New(clip, pos);
			channel.Volume = 1;
			channel.Trigger(param);
		}

		public static void Loop(Soundbank soundbank)
		{
			Loop(soundbank, VoiceParams.Const2D());
		}

		public static void Loop(Soundbank soundbank, VoiceParams param, Vector3? pos = null)
		{
			// If there is nothing to play, we don't need to do anything.
			if (soundbank == null)
			{
				return;
			}

			var channel = FindChannel<SoundbankLoop>(soundbank)
				?? SoundbankLoop.New(soundbank, pos);
			channel.Volume = 1;
			channel.Trigger(param);
		}

		#endregion

		#region Stop

		public static void Stop(AudioClip clip)
		{
			var hit = FindChannel<AudioClipHit>(clip);
			if (hit != null)
			{
				hit.Volume = 0;
			}

			var loop = FindChannel<AudioClipLoop>(clip);
			if (loop != null)
			{
				loop.Volume = 0;
			}
		}

		public static void Stop(Soundbank soundbank)
		{
			var hit = FindChannel<SoundbankHit>(soundbank);
			if (hit != null)
			{
				hit.Volume = 0;
			}

			var loop = FindChannel<SoundbankLoop>(soundbank);
			if (loop != null)
			{
				loop.Volume = 0;
			}
		}

		public static void StopAll()
		{
			AllChannels().ForEach(x => x.Volume = 0);
		}

		/// <summary>
		/// Stops all audio except for the specified audio.
		/// </summary>
		/// <param name="clips">The clips to stop.</param>
		/// <param name="soundbanks">The soundbanks to stop.</param>
		public static void StopAllExcept(IEnumerable<AudioClip> clips, IEnumerable<Soundbank> soundbanks)
		{
			var instanceIds = clips
				.Select(x => x?.GetInstanceID())
				.Union(soundbanks.Select(x => x?.GetInstanceID()))
				.Where(x => x.HasValue)
				.Select(x => x.Value.ToString())
				.ToList();

			// Stop every audio not in the list of instance ids to keep
			var toRemove = new List<int>();
			foreach (var channel in AllChannels())
			{
				if (instanceIds.Contains(channel.name.Split()[1]))
				{
					continue;
				}

				channel.Volume = 0;
			}
		}

		#endregion
	}
}
