using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Cache_Editor_API
{
	public class Archive
	{
		public FileStream DataFile { get; set; }
		public FileStream IndexFile { get; set; }
		public int ArchiveIndex { get; set; }
		public string ArchiveName { get; set; }
		private byte[] gzip_buffer;

		public Archive(string name, FileStream data, FileStream indexes)
		{
			DataFile = data;
			IndexFile = indexes;
			ArchiveName = name;
			gzip_buffer = new byte[65000];
		}

		public int GetFileCount()
		{
			if (IndexFile == null)
				return 0;
			return (int)(IndexFile.Length / 6);
		}

		public int GetFileSize(int file_index)
		{
			IndexFile.Seek(file_index * 6, SeekOrigin.Begin);
			return (IndexFile.ReadByte() << 16) + (IndexFile.ReadByte() << 8) + IndexFile.ReadByte();
		}

		public int GetChunkOffset(int file_index)
		{
			IndexFile.Seek(file_index * 6 + 3, SeekOrigin.Begin);
			return (IndexFile.ReadByte() << 16) + (IndexFile.ReadByte() << 8) + IndexFile.ReadByte();
		}

		public byte[] ExtractFile(int file_index)
		{
			if (file_index >= GetFileCount())
				return null;

			IndexFile.Seek(file_index * 6, SeekOrigin.Begin);
			int file_size = (IndexFile.ReadByte() << 16) + (IndexFile.ReadByte() << 8) + IndexFile.ReadByte();
			int chunk_offset = (IndexFile.ReadByte() << 16) + (IndexFile.ReadByte() << 8) + IndexFile.ReadByte();

			if (chunk_offset < 0 || chunk_offset > DataFile.Length / 520L)
				return null;

			byte[] out_buffer = new byte[file_size];
			int write_offset = 0;
			for (int chunk_index = 0; write_offset < file_size; chunk_index++)
			{
				if (chunk_offset == 0)
					return null;
				int chunk_size = Math.Min(512, file_size - write_offset);

				DataFile.Seek(chunk_offset * 520L, SeekOrigin.Begin);
				if (DataFile.Position + chunk_size + 8 > DataFile.Length)
					return null;

				int checksum_file_index = (DataFile.ReadByte() << 8) + DataFile.ReadByte();
				int checksum_chunk_index = (DataFile.ReadByte() << 8) + DataFile.ReadByte();
				int next_chunk_offset = (DataFile.ReadByte() << 16) + (DataFile.ReadByte() << 8) + DataFile.ReadByte();
				int checksum_archive_index = DataFile.ReadByte();

				if (checksum_file_index != file_index || checksum_chunk_index != chunk_index || checksum_archive_index - 1 != ArchiveIndex)
					return null;
				if (next_chunk_offset < 0 || next_chunk_offset * 520L > DataFile.Length)
					return null;
				DataFile.Read(out_buffer, write_offset, chunk_size);
				write_offset += chunk_size;
				chunk_offset = next_chunk_offset;
			}

			if (ArchiveIndex > 0)
			{
				int i = 0;
				try
				{
					GZipStream gzipinputstream = new GZipStream(new MemoryStream(out_buffer), CompressionMode.Decompress);
					do
					{
						if (i == gzip_buffer.Length)
							return null;
						int k = gzipinputstream.Read(gzip_buffer, i, gzip_buffer.Length - i);
						if (k == 0)
							break;
						i += k;
					} while (true);
					out_buffer = new byte[i];
					Array.Copy(gzip_buffer, out_buffer, out_buffer.Length);
				}
				catch (Exception)
				{
					return null;
				}
			}

			return out_buffer;
		}

		public bool WriteFile(int file_index, byte[] data, int length)
		{
			if (ArchiveIndex > 0)
			{
				MemoryStream ms = new MemoryStream();
				GZipStream output = new GZipStream(ms, CompressionLevel.Optimal);
				output.Write(data, 0, length);
				output.Close();
				data = ms.ToArray();
				length = data.Length;
			}
			bool b = WriteFile(true, file_index, length, data);
			if (!b)
				b = WriteFile(false, file_index, length, data);
			return b;
		}

		public bool WriteFile(bool exists, int file_index, int file_size, byte[] data)
		{

			try
			{
				int chunk_offset = 0;
				if (exists)
				{
					IndexFile.Seek(file_index * 6 + 3, SeekOrigin.Begin);
					chunk_offset = (IndexFile.ReadByte() << 16) + (IndexFile.ReadByte() << 8) + IndexFile.ReadByte();

					if (chunk_offset <= 0 || (long)chunk_offset > DataFile.Length / 520L)
						return false;
				}
				else
				{
					chunk_offset = (int)((DataFile.Length + 519L) / 520L);
					if (chunk_offset == 0)
						chunk_offset = 1;
				}
				IndexFile.Seek(file_index * 6, SeekOrigin.Begin);
				IndexFile.WriteByte((byte)(file_size >> 16));
				IndexFile.WriteByte((byte)(file_size >> 8));
				IndexFile.WriteByte((byte)file_size);
				IndexFile.WriteByte((byte)(chunk_offset >> 16));
				IndexFile.WriteByte((byte)(chunk_offset >> 8));
				IndexFile.WriteByte((byte)chunk_offset);
				int write_offset = 0;
				for (int chunk_index = 0; write_offset < file_size; chunk_index++)
				{
					int next_chunk_offset = 0;
					if (exists)
					{
						DataFile.Seek(chunk_offset * 520L, SeekOrigin.Begin);
						if (DataFile.Position + 8 >= DataFile.Length)
						{
							int checksum_file_index = (DataFile.ReadByte() << 8) + DataFile.ReadByte();
							int checksum_chunk_index = (DataFile.ReadByte() << 8) + DataFile.ReadByte();
							next_chunk_offset = (DataFile.ReadByte() << 16) + (DataFile.ReadByte() << 8) + DataFile.ReadByte();
							int checksum_archive_index = DataFile.ReadByte();
							if (checksum_file_index != file_index || checksum_chunk_index != chunk_index || checksum_archive_index - 1 != ArchiveIndex)
								return false;
							if (next_chunk_offset < 0 || (long)next_chunk_offset > DataFile.Length / 520L)
								return false;
						}
						else
							DataFile.Position = DataFile.Length - 1;
					}
					if (next_chunk_offset == 0)
					{
						exists = false;
						next_chunk_offset = (int)((DataFile.Length + 519L) / 520L);
						if (next_chunk_offset == 0)
							next_chunk_offset++;
						if (next_chunk_offset == chunk_offset)
							next_chunk_offset++;
					}
					if (file_size - write_offset <= 512)
						next_chunk_offset = 0;
					DataFile.Seek(chunk_offset * 520L, SeekOrigin.Begin);
					DataFile.WriteByte((byte)(file_index >> 8));
					DataFile.WriteByte((byte)file_index);
					DataFile.WriteByte((byte)(chunk_index >> 8));
					DataFile.WriteByte((byte)chunk_index);
					DataFile.WriteByte((byte)(next_chunk_offset >> 16));
					DataFile.WriteByte((byte)(next_chunk_offset >> 8));
					DataFile.WriteByte((byte)next_chunk_offset);
					DataFile.WriteByte((byte)(ArchiveIndex + 1));
					int chunk_size = file_size - write_offset;
					if (chunk_size > 512)
						chunk_size = 512;
					DataFile.Write(data, write_offset, chunk_size);
					write_offset += chunk_size;
					chunk_offset = next_chunk_offset;
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool CreateFile()
		{
			return WriteFile(GetFileCount(), new byte[512], 512);
		}
	}
}
