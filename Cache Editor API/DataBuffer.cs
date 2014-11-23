using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache_Editor_API
{
	public class DataBuffer
	{
		public byte[] Buffer { get; set; }
		public int Location { get; set; }

		public DataBuffer(byte[] buffer)
		{
			Buffer = buffer;
			Location = 0;
		}

		public byte ReadByte()
		{
			return Buffer[Location++];
		}

		public int ReadShort()
		{
			return (Buffer[Location++] << 8) + Buffer[Location++];
		}

		public int Read3Bytes()
		{
			return (Buffer[Location++] << 16) + (Buffer[Location++] << 8) + Buffer[Location++];
		}

		public int ReadInteger()
		{
			return (Buffer[Location++] << 24) + (Buffer[Location++] << 16) + (Buffer[Location++] << 8) + Buffer[Location++];
		}

		public int ReadSmart()
		{
			int i = Buffer[Location];
			if (i < 128)
				return ReadByte() - 64;
			return ReadShort() - 49152;
		}

		public void WriteByte(byte b)
		{
			Buffer[Location++] = b;
		}

		public void WriteShort(int b)
		{
			Buffer[Location++] = (byte)(b >> 8);
			Buffer[Location++] = (byte)b;
		}

		public void Write3Bytes(int b)
		{
			Buffer[Location++] = (byte)(b >> 16);
			Buffer[Location++] = (byte)(b >> 8);
			Buffer[Location++] = (byte)b;
		}

		public void WriteInteger(int b)
		{
			Buffer[Location++] = (byte)(b >> 24);
			Buffer[Location++] = (byte)(b >> 16);
			Buffer[Location++] = (byte)(b >> 8);
			Buffer[Location++] = (byte)b;
		}

		public void Write(byte[] buffer)
		{
			Array.Copy(buffer, 0, Buffer, Location, buffer.Length);
			Location += buffer.Length;
		}

		public void Write(byte[] buffer, int src_offset, int length)
		{
			Array.Copy(buffer, src_offset, Buffer, Location, length);
			Location += length;
		}
	}
}
