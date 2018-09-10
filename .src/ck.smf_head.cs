/* User: tfooo * Date: 11/12/2005 * Time: 4:19 PM */
using System;
using System.Runtime.InteropServices;

namespace on.smfio.chunk
{
	[ StructLayout( LayoutKind.Sequential, CharSet=CharSet.Ansi )]
	public struct smf_head {
		public string	lpData;
		public short	dwBufferLength;
		public short	dwBytesRecorded;
		public short	dwUser;
		public short	dwFlags;
		public short 	far_x_lpNext;
		public short	reserved;
		public short	dwOffset;
		public short	dwReserved;
	}
}
