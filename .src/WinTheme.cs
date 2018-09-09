#region User/License
// oio * 8/14/2012 * 9:18 PM

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
using System.Windows.Forms;

namespace System
{
	public partial class WindowsInterop
	{
		/// <summary>
		/// Transclude this class (File-&gt;Link).
		/// </summary>
		public static class WindowsTheme
		{
			const int BS_COMMANDLINK	= 0x0000000E;
			const int BCM_SETNOTE			= 0x00001609;
			// see article ‘Fully Themed Windows Vista Controls’ for implementation detail
			// http://www.codeproject.com/Articles/18858/Fully-themed-Windows-Vista-Controls
			const int BS_SPLITBUTTON	= 0x0000000C;
			// const int BS_COMMANDLINK = 0x0000000E;
			
			static public void HandleTheme(Control controlWithHandle)
			{
				HandleTheme(controlWithHandle,false);
			}
			/// <summary>
			/// This method is for a ListView or TreeView.
			/// </summary>
			/// <param name="controlWithHandle"></param>
			static public void HandleTheme(Control controlWithHandle, bool tvAutoScroll)
			{
				SetWindowTheme(controlWithHandle.Handle, "explorer", null);
				if (controlWithHandle is TreeView)
				{
					(controlWithHandle as TreeView).HotTracking = true;
					(controlWithHandle as TreeView).FullRowSelect = true;
					SendMessage(controlWithHandle.Handle, 0x112C, 0x0020, 0x0020);
				}
				else if (controlWithHandle is ListView)
				{
					SendMessage(controlWithHandle.Handle, 0x1036, 0x00010000, 0x00010000);
				}
			}
			
			[System.Runtime.InteropServices.DllImport("user32.dll",CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
			static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);
			[System.Runtime.InteropServices.DllImport("user32.dll",CharSet = System.Runtime.InteropServices.CharSet.Auto)]
			static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
			
			/// <summary>
			/// Native interop method, be sure to include the  System.Runtime.InteropServices
			/// name space
			/// </summary>
			/// <param name="hWnd"></param>
			/// <param name="appName"></param>
			/// <param name="partList"></param>
			/// <returns></returns>
			[System.Runtime.InteropServices.DllImport("uxtheme.dll",CharSet = System.Runtime.InteropServices.CharSet.Unicode,ExactSpelling = true)]
			static extern int SetWindowTheme(IntPtr hWnd, string appName, string partList);
		}
	}
}
