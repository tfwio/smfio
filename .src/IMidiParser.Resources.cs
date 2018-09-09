/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;

using System.Drawing;
using CliEvent = System.EventArgs;

namespace on.smfio
{
	public interface IMidiParser_Resources
	{
		/// <summary>
		/// this method absolutely depends on RunningStatus32 value.
		/// </summary>
		/// <returns></returns>
		Color GetRseEventColor(Color clr);
		/// <summary>
		/// this method absolutely depends on RunningStatus32 value.
		/// </summary>
		/// <returns></returns>
		Color GetEventColor(Color clr);
		/// <summary>
		/// this method absolutely depends on RunningStatus32 value.
		/// </summary>
		/// <returns></returns>
		Color GetEventColor(int intMsg, Color clr);
		/// <summary>
		/// A collection of colors for erm...
		/// </summary>
		Dictionary<string, Color> Colors { get; }
	}
}
