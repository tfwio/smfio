using System;
namespace on.smfio
{
	public class Patch
	{
		public Patch(byte bank, byte program)
		{
			Bank = bank;
			Program = program;
		}

		public Patch(InstrumentType gmPatch)
		{
			var data = gmPatch.GetBytes();
			Bank = data[0];
			Program = data[1];
		}

		/// <summary>
		/// input data must be two bytes.
		/// </summary>
		/// <param name="data">byte[0] = bank, byte[1] = patch</param>
		public Patch(byte[] data)
		{
			Bank = data[0];
			Program = data[1];
		}

		/// <summary>
		/// Converts integer (little-endian) integer to patch.
		/// 
		/// our input must be little-endian:  
		/// E.G. 0x000F = MSB: 0x0F, LSB: 0x00.
		/// </summary>
		/// <param name="data"></param>
		public Patch(int data)
		{
			Bank = Convert.ToByte((data & 0xFF00) >> 8);
			Program = Convert.ToByte(data & 0xFF);
		}

		public byte Bank;

		public byte Program;

		public byte[] Bytes {
			get {
				return new byte[2] {
					Bank,
					Program
				};
			}
		}

		static public implicit operator Patch(byte[] bytes) {
			return new Patch(bytes);
		}
	}
}


