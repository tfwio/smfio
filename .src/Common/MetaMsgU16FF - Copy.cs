#region Info
// oio * 2005-11-12 * 4:19 PM
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
#region Using
using System;
#endregion
namespace on.smfio.Common
{
	/// <summary>
	/// Message (bits | 0xFF00)
	/// </summary>
	public enum MetaMsgU16FF : ushort
	{
		/// 0xFF00
		SequenceNo		  = 0xFF00,
		/// 0xFF01
		Text			      = 0xFF01,
		/// 0xFF02
		Copyright		    = 0xFF02,
		/// 0xFF03
		SequenceName  	= 0xFF03,
		/// 0xFF04
		InstrumentName	= 0xFF04,
		/// 0xFF05
		Lyric		      	= 0xFF05,
		/// 0xFF06
		Marker		    	= 0xFF06,
		/// 0xFF07
		Cue				      = 0xFF07,
		/// 0xFF20
		Chanel			    = 0xFF20,
		/// 0xFF21
		Port			      = 0xFF21,
		/// 0xFF51
		Tempo			      = 0xFF51,
		/// 0xFF54
		SMPTE			      = 0xFF54,
		/// 0xFF58
		TimeSignature	  = 0xFF58,
		/// 0xFF59
		KeySignature	  = 0xFF59,
		/// 0xFF2F
		EndOfTrack		  = 0xFF2F,
		/// 0xFFF0
		SystemExclusive = 0xFFF0,
		/// 0xFF7F
		SystemSpecific  = 0xFF7F,
		
	}

}
