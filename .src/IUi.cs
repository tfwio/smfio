/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using System.Drawing;
namespace SMFIOViewer
{
	using Keys = System.Windows.Forms.Keys;

	public interface IUi
	{
		/// <summary></summary>
		Rectangle ClientRectangle {
			get;
		}
	}
}






