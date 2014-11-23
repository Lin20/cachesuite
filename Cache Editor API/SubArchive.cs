using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.BZip2;

namespace Cache_Editor_API
{
	//aka StreamLoader
	public class SubArchive
	{
		public byte[] MainBuffer { get; set; }
		public byte[] CompressedBuffer { get; set; }
		public int FileCount { get; private set; }
		public int[] FileNames { get; private set; }
		public int[] UncompressedFileSizes { get; private set; }
		public int[] CompressedFileSizes { get; private set; }
		public int[] FileLocations { get; private set; }
		public byte[][] FileBuffers { get; set; }
		/*
		 * Unlike the client, we keep an array of file buffers here.
		 * When a file is extracted, it is written to the file buffer.
		 * Due to how the cache is unpacked, we must recompose the archive to maximize space efficiency.
		 * This requires extracting every file and replacing them, which is annoying and can be costly given the speed of BZip2.
		 * Unfortunately could have been avoided if this used the same format as the typical archive, which allowed data to be broken out in 512 byte blocks. Oh well.
		 */

		public bool ArchiveDecompressed { get; private set; }

		public SubArchive(byte[] buffer)
		{
			LoadBuffer(buffer);
		}

		private void LoadBuffer(byte[] buffer)
		{
			CompressedBuffer = buffer;
			if (buffer == null || buffer.Length == 0)
			{
				MainBuffer = buffer;
				FileCount = 0;
				FileNames = new int[FileCount];
				UncompressedFileSizes = new int[FileCount];
				CompressedFileSizes = new int[FileCount];
				FileLocations = new int[FileCount];
				FileBuffers = new byte[0][];
				return;
			}
			DataBuffer read_buffer = new DataBuffer(buffer);
			int uncompressed_size = read_buffer.Read3Bytes();
			int compressed_size = read_buffer.Read3Bytes();

			if (uncompressed_size != compressed_size) //no rhyme or reason as to when this is used
			{
				MainBuffer = new byte[uncompressed_size];
				byte[] temp = new byte[buffer.Length - 6];
				Array.Copy(buffer, 6, temp, 0, temp.Length);
				MemoryStream ms = new MemoryStream(temp);
				BZip2InputStream bz = new BZip2InputStream(ms);
				bz.Read(MainBuffer, 0, uncompressed_size);
				//BZ2Decompressor.decompress(Buffer, uncompressed_size, buffer, compressed_size, 6);
				ArchiveDecompressed = true;
				read_buffer = new DataBuffer(MainBuffer);
			}
			else
			{
				MainBuffer = buffer;
				ArchiveDecompressed = false;
			}

			FileCount = read_buffer.ReadShort();
			FileNames = new int[FileCount];
			UncompressedFileSizes = new int[FileCount];
			CompressedFileSizes = new int[FileCount];
			FileLocations = new int[FileCount];
			FileBuffers = new byte[FileCount][];

			int pos = read_buffer.Location + FileCount * 10;
			for (int i = 0; i < FileCount; i++)
			{
				FileNames[i] = read_buffer.ReadInteger();
				UncompressedFileSizes[i] = read_buffer.Read3Bytes();
				CompressedFileSizes[i] = read_buffer.Read3Bytes();
				FileLocations[i] = pos;
				pos += CompressedFileSizes[i];
			}
		}

		public int GetFileIndex(int name)
		{
			for (int i = 0; i < FileNames.Length; i++)
			{
				if (FileNames[i] == name)
					return i;
			}

			return -1;
		}

		public int GetFileIndex(string name)
		{
			name = name.ToUpper();
			int i = 0;
			for (int j = 0; j < name.Length; j++)
				i = (i * 61 + name[j]) - 32;

			return GetFileIndex(i);
		}

		public int GetHashCode(string name)
		{
			name = name.ToUpper();
			int i = 0;
			for (int j = 0; j < name.Length; j++)
				i = (i * 61 + name[j]) - 32;
			return i;
		}

		[Obsolete("This function does not work due to integer overflows. Useful only if names are very small")]
		public string GetFilename(int file)
		{
			StringBuilder s = new StringBuilder();
			int i = FileNames[file];
			for (; i != 0 && s.Length < 10; )
			{
				char c = (char)((i % 61) + 32);
				s.Insert(0, c);
				i /= 61;
			}

			return s.ToString();
		}

		public bool FileExists(string name)
		{
			int i = 0;
			name = name.ToUpper();
			for (int j = 0; j < name.Length; j++)
				i = (i * 61 + name[j]) - 32;

			for (int k = 0; k < FileCount; k++)
			{
				if (FileNames[k] == i)
				{
					return true;
				}
			}

			return false;
		}

		public DataBuffer ExtractFile(string name)
		{
			int i = 0;
			name = name.ToUpper();
			for (int j = 0; j < name.Length; j++)
				i = (i * 61 + name[j]) - 32;

			for (int k = 0; k < FileCount; k++)
			{
				if (FileNames[k] == i)
				{
					return ExtractFile(k);
				}
			}

			return null;
		}

		public DataBuffer ExtractFile(int k)
		{
			if (k >= FileCount || k < 0)
				return null;

			if (FileBuffers[k] != null)
				return new DataBuffer(FileBuffers[k]);

			FileBuffers[k] = new byte[UncompressedFileSizes[k]];
			if (!ArchiveDecompressed)
			{
				byte[] temp = new byte[CompressedFileSizes[k]];
				FileBuffers[k] = new byte[UncompressedFileSizes[k]];
				Array.Copy(MainBuffer, FileLocations[k], temp, 0, temp.Length);
				MemoryStream ms = new MemoryStream(temp);
				BZip2InputStream bz = new BZip2InputStream(ms);
				bz.Read(FileBuffers[k], 0, UncompressedFileSizes[k]);
			}
			else
			{
				Array.Copy(MainBuffer, FileLocations[k], FileBuffers[k], 0, UncompressedFileSizes[k]);

			}
			return new DataBuffer(FileBuffers[k]);
		}

		public bool RenameFile(int index, int new_name)
		{
			if (index < 0 || index >= FileNames.Length)
				return false;
			for (int i = 0; i < FileNames.Length; i++)
			{
				if (FileNames[i] == new_name)
					return false;
			}

			FileNames[index] = new_name;
			return true;
		}

		public bool RenameFile(int index, string new_name)
		{
			return RenameFile(index, GetHashCode(new_name));
		}

		public bool RenameFile(string old_name, string new_name)
		{
			return RenameFile(GetFileIndex(old_name), GetHashCode(new_name));
		}

		public void WriteFile(int index, byte[] data)
		{
			if (index < 0 || index >= FileCount)
				return;
			UncompressedFileSizes[index] = data.Length;
			FileBuffers[index] = data;
		}

		public void WriteFile(string name, byte[] data)
		{
			WriteFile(GetFileIndex(name), data);
		}

		public bool RewriteArchive()
		{
			DataBuffer data_buffer = new DataBuffer(new byte[8192]);
			for (int i = 0; i < FileCount; i++)
			{
				if (FileBuffers[i] == null)
					ExtractFile(i);
				PackFile(ref data_buffer, i, FileBuffers[i], !ArchiveDecompressed);
			}

			DataBuffer header = new DataBuffer(new byte[2 + FileCount * 10]);

			header.WriteShort(FileCount);
			for (int i = 0; i < FileCount; i++)
			{
				header.WriteInteger(FileNames[i]);
				header.Write3Bytes(UncompressedFileSizes[i]);
				header.Write3Bytes(CompressedFileSizes[i]);
				//pos += CompressedFileSizes[i];
			}


			if (ArchiveDecompressed)
			{
				DataBuffer d = new DataBuffer(new byte[data_buffer.Buffer.Length + header.Buffer.Length]);
				//d.Write3Bytes(data_buffer.Buffer.Length);
				//d.Write3Bytes(0);
				d.Write(header.Buffer, 0, header.Location);
				d.Write(data_buffer.Buffer, 0, data_buffer.Location);
				MemoryStream ms = new MemoryStream();
				BZip2OutputStream os = new BZip2OutputStream(ms, 1);
				os.Write(d.Buffer, 0, d.Location);
				os.Close();

				byte[] c = ms.GetBuffer();
				DataBuffer final = new DataBuffer(new byte[os.BytesWritten + 6]);
				final.Write3Bytes(d.Buffer.Length);
				final.Write3Bytes(os.BytesWritten);
				final.Write(c, 0, os.BytesWritten);
				MainBuffer = final.Buffer;
			}
			else
			{
				DataBuffer final = new DataBuffer(new byte[data_buffer.Buffer.Length + header.Buffer.Length + 100000]);
				final.Write3Bytes(data_buffer.Buffer.Length);
				if (ArchiveDecompressed)
					final.Write3Bytes(0);
				else
					final.Write3Bytes(data_buffer.Buffer.Length);
				final.Write(header.Buffer, 0, header.Location);
				final.Write(data_buffer.Buffer, 0, data_buffer.Location);
				MainBuffer = final.Buffer;
			}
			LoadBuffer(MainBuffer);

			return true;
		}

		private bool PackFile(ref DataBuffer dest, int index, byte[] data, bool compress)
		{
			if (!compress)
			{
				if (dest.Location + data.Length >= dest.Buffer.Length)
				{
					byte[] nbuffer = new byte[(dest.Buffer.Length + data.Length) / 1024 * 1024 + 4096];
					Array.Copy(dest.Buffer, nbuffer, dest.Buffer.Length);
					dest.Buffer = nbuffer;
				}

				FileLocations[index] = dest.Location;
				CompressedFileSizes[index] = UncompressedFileSizes[index];
				dest.Write(data);
				return true;
			}
			else
			{
				//using streams is really stupid
				MemoryStream ms = new MemoryStream();
				BZip2OutputStream output = new BZip2OutputStream(ms, 1);
				output.Write(data, 0, data.Length);
				output.Close();
				byte[] compressed = ms.ToArray();
				CompressedFileSizes[index] = (int)output.BytesWritten;

				if (dest.Location + CompressedFileSizes[index] >= dest.Buffer.Length)
				{
					byte[] nbuffer = new byte[(dest.Buffer.Length + CompressedFileSizes[index]) + 4096];
					Array.Copy(dest.Buffer, nbuffer, dest.Buffer.Length);
					dest.Buffer = nbuffer;
				}

				dest.Write(compressed, 0, compressed.Length);
			}

			return true;
		}

		public void CreateFile(string name)
		{
			FileNames = ExpandArray<int>(FileNames);
			UncompressedFileSizes = ExpandArray<int>(UncompressedFileSizes);
			CompressedFileSizes = ExpandArray<int>(CompressedFileSizes);
			FileLocations = ExpandArray<int>(FileLocations);
			FileBuffers = ExpandArray<byte[]>(FileBuffers);
			//FileCount;

			FileNames[FileCount] = GetHashCode(name);
			FileBuffers[FileCount] = new byte[512];
			UncompressedFileSizes[FileCount] = 512;
			FileCount++;
			RewriteArchive();
		}

		private static T[] ExpandArray<T>(T[] src)
		{
			T[] n = new T[src.Length + 1];
			Array.Copy(src, n, src.Length);
			return n;
		}
	}
}
