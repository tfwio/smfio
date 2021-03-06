﻿/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

using on.smfio.Common;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;

namespace on.smfio
{
	public class MidiKeySignature
	{
		public string MajorMinorString { get { return IsMajor ? "Major" : "Minor"; } }
		public bool IsMajor;
		public KeySignatureType KeyType = KeySignatureType.C;
		
    public void SetSignature(KeySignatureType ksigtype, bool ismaj)
    {
      IsMajor = ismaj;
      KeyType = ksigtype;
    }
    public void SetSignature(byte key, byte maj)
    {
      IsMajor = maj == 0;
      KeyType = (KeySignatureType)key;
    }
    
		public MidiKeySignature() : this(KeySignatureType.C,true) {}
    public MidiKeySignature(byte key, byte maj) { SetSignature(key, maj); }
    public MidiKeySignature(KeySignatureType ksigtype, bool ismaj) { SetSignature(ksigtype,ismaj); }
    
		public void Reset() { IsMajor = true; KeyType = KeySignatureType.C; }
		public MidiKeySignature Copy() { return new MidiKeySignature(KeyType, IsMajor); }

		public override string ToString()
		{
			return $"{KeyType.GetEnumDescriptionAttribute()} {MajorMinorString}";
		}

	}
}
