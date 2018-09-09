#region User/License
// oio * 7/31/2012 * 11:12 PM

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using on.smfio;

namespace on.smfio.util
{
	public interface IAudioConfig
	{
		int Rate { get;set; }
		float RateF { get; }
		int Channels { get;set; }
		int Latency { get;set; }
	}
	public interface IMidiConfig
	{
		int Division { get;set; }
		
		double Tempo    { get;set; }
		
		MidiTimeSignature TimeSignature { get;set; }
		
		MidiKeySignature KeySignature { get;set; }
		
		bool IsSingleZeroChannel { get;set; }
		
		double BarStart { get;set; }
		
		double BarStartPulses { get;set; }
		/// <summary>
		/// This is used in <see cref="Loop"/>.<see cref="Loop.Length"/>.
		/// ...As well as a particular instance within NAudioVST.One(Loop).
		/// </summary>
		double BarLength { get;set; }
		
		double BarLengthPulses { get;set; }
	}
	
	public interface ITimeConfiguration : IMidiConfig, IAudioConfig {
		void FromMidi(IMidiParser parser);
	}
}
