﻿#region User/License
// oio * 10/20/2012 * 8:50 AM

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
using System.Drawing;

namespace modest100.Internals
{
  /// <summary></summary>
  [FlagsAttribute]
  public enum RenderStateType { None, Select, Deselect, Background, Mouse, MouseWheel, Text, Notes, XScroll, YScroll }

  /// <summary></summary>
  public interface ITrackMouse
  {
    /// <summary></summary>
    bool HasMouseDownPoint { get; }
    /// <summary></summary>
    FloatPoint MouseDownPoint { get; set; }
    /// <summary></summary>
    FloatPoint MouseMovePoint { get; set; }
  }
  class MouseState
  {
    FloatPoint PointDown { get;set; }

    FloatPoint PointMove { get;set; }
    
    public bool IsMouseMin { get { return MouseMinX || MouseMinY; } }

    public bool MouseMinX { get { return PointMove.X < 0; } }

    public bool MouseMinY { get { return PointMove.Y < 0; } }

    public FloatPoint InvMouseTrail { get { return new FloatPoint( MouseMinX ? -1*PointMove.X : PointMove.X, MouseMinY ? -1*PointMove.Y : PointMove.Y ); } }

    public FloatPoint InvMousePoint { get { return new FloatPoint( MouseMinX ? PointDown.X+PointMove.X : PointDown.X, MouseMinY ? PointDown.Y+PointMove.Y : PointDown.Y ); } }
    
    static public MouseState Create(ITrackMouse control)
    {
      MouseState state = new MouseState();
      state.PointDown = control.MouseDownPoint;
      state.PointMove = control.MouseDownPoint;
      return state;
    }
  }
}
