/* tfwxo * 10/15/2018 * 7:51 AM */
using System;
using System.IO;
namespace System
{
  static class EndianConvertExtension
  {
    static public byte[] GetBytesEndian(this ulong integer, bool inverse = false) { return BitConverter.GetBytes(integer).Flip64(inverse); }
    static public byte[] GetBytesEndian(this long integer, bool inverse = false) { return BitConverter.GetBytes(integer).Flip64(inverse); }
    static public byte[] GetBytesEndian(this int integer, bool inverse = false) { return BitConverter.GetBytes(integer).Flip32(inverse); }
    static public byte[] GetBytesEndian(this uint integer, bool inverse = false) { return BitConverter.GetBytes(integer).Flip32(inverse); }
    static public byte[] GetBytesEndian(this ushort integer, bool inverse = false) { return BitConverter.GetBytes(integer).Flip16(inverse); }
    static public byte[] GetBytesEndian(this short integer, bool inverse = false) { return BitConverter.GetBytes(integer).Flip16(inverse); }
    
    static public short Swap(this short value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return BitConverter.ToInt16(bytes, 0);
    }
    static public ushort Swap(this ushort value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return BitConverter.ToUInt16(bytes, 0);
    }
    static public int Swap(this int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return BitConverter.ToInt32(bytes, 0);
    }
    static public uint Swap(this uint value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse(bytes);
      return BitConverter.ToUInt32(bytes, 0);
    }
    
    static public long Flip(this long input, bool inverse=false)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian) bytes = bytes.Flip64();
      return BitConverter.ToInt64(bytes, 0);
    }
    
    static public long Flip(this ulong input, bool inverse=false)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian) bytes = bytes.Flip64();
      return BitConverter.ToInt64(bytes, 0);
    }

    static public int Flip(this int input, bool inverse=false)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian) bytes = bytes.Flip32();
      return BitConverter.ToInt32(bytes, 0);
    }

    static public uint Flip(this uint input, bool inverse=false)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian) bytes = bytes.Flip32();
      return BitConverter.ToUInt32(bytes, 0);
    }

    static public short Flip(this short input, bool inverse=false)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian) bytes = bytes.Flip16();
      return BitConverter.ToInt16(bytes, 0);
    }

    static public ushort Flip(this ushort input, bool inverse=false)
    {
      byte[] bytes = BitConverter.GetBytes(input);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian) bytes = bytes.Flip16();
      return BitConverter.ToUInt16(bytes, 0);
    }

    static byte[] FlipN(this byte[] inputData, int size, bool inverse=false)
    {
      int zize = size - 1;
      var data = new byte[size];
      Array.Copy(inputData, data, size);
      if (inverse ? !BitConverter.IsLittleEndian : BitConverter.IsLittleEndian)
      {
        for (int i = 0; i < size; i++) data[i] = inputData[zize - i];
      }
      return data;
    }

    /// <summary>INPUT DATA MUST BE 8 bytes long!</summary>
    static public byte[] Flip64(this byte[] inputData, bool inverse=false) { return FlipN(inputData, 8, inverse); }

    /// <summary>DATA MUST BE 4 bytes long!</summary>
    static public byte[] Flip32(this byte[] inputData, bool inverse=false) { return FlipN(inputData, 4, inverse); }

    /// <summary>INPUT DATA MUST BE 2 bytes long!</summary>
    static public byte[] Flip16(this byte[] inputData, bool inverse=false) { return FlipN(inputData, 2, inverse); }
  }
}


