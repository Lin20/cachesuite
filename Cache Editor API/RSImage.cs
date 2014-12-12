using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using nQuant;

namespace Cache_Editor_API
{
	public class RSImage
	{
		public int IndexLocation { get; set; }
		public int WholeWidth { get; set; }
		public int WholeHeight { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int XOffset { get; set; }
		public int YOffset { get; set; }
		public Color[] Pixels { get; set; }
		public byte[] PaletteIndexes { get; set; }
		public Color[] Palette { get; set; }

		public RSImage(Cache cache, CacheItemNode node, DataBuffer image_data, int sprite_index = 0)
			: this(cache.SubArchives[node.SubArchive], image_data, sprite_index)
		{
		}

		public RSImage(SubArchive archive, int file, int sprite_index = 0)
			: this(archive, archive.ExtractFile(file), sprite_index)
		{
		}

		public RSImage(SubArchive archive, string file, int sprite_index = 0)
			: this(archive, archive.ExtractFile(file), sprite_index)
		{
		}

		public RSImage(SubArchive archive, DataBuffer image_data, int sprite_index = 0)
		{
			try
			{
				DataBuffer index_data = archive.ExtractFile("index.dat");
				if (index_data == null || image_data == null)
					return;

				image_data.Location = 0;
				index_data.Location = image_data.ReadShort();
				WholeWidth = index_data.ReadShort();
				WholeHeight = index_data.ReadShort();
				Palette = new Color[index_data.ReadByte()];
				for (int i = 0; i < Palette.Length - 1; i++)
					Palette[i + 1] = Color.FromArgb(index_data.ReadByte(), index_data.ReadByte(), index_data.ReadByte());

				for (int i = 0; i < sprite_index; i++)
				{
					index_data.Location += 2; //x/y offset
					image_data.Location += index_data.ReadShort() * index_data.ReadShort(); //width * height
					index_data.Location++; //type
				}

				XOffset = index_data.ReadByte();
				YOffset = index_data.ReadByte();
				Width = index_data.ReadShort();
				Height = index_data.ReadShort();
				byte type = index_data.ReadByte();

				Pixels = new Color[Width * Height];
				PaletteIndexes = new byte[Width * Height];
				if (type == 0)
				{
					for (int i = 0; i < Pixels.Length; i++)
					{
						PaletteIndexes[i] = image_data.ReadByte();
						Pixels[i] = Palette[PaletteIndexes[i]];
					}
				}
				else if (type == 1)
				{
					for (int x = 0; x < Width; x++)
					{
						for (int y = 0; y < Height; y++)
						{
							PaletteIndexes[x + y * Width] = image_data.ReadByte();
							Pixels[x + y * Width] = Palette[PaletteIndexes[x + y * Width]];
						}
					}
				}
			}
			catch (Exception)
			{
				Pixels = null;
			}
		}

		public RSImage(string filename, out Exception ex)
		{
			try
			{
				Bitmap bmp = new Bitmap(filename, true);
				if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
					bmp.MakeTransparent(Color.Transparent); //force bmp to be 32-bit
				WuQuantizer q = new WuQuantizer();
				bmp = new Bitmap(q.QuantizeImage(bmp));
				QuantizedPalette p = q.Palette;
				Palette = new Color[p.Colors.Count];
				for (int i = 0; i < p.Colors.Count; i++)
					Palette[i] = p.Colors[i];

				Pixels = new Color[bmp.Width * bmp.Height];
				for (int i = 0; i < bmp.Width * bmp.Height; i++)
					Pixels[i] = bmp.GetPixel(i % bmp.Width, i / bmp.Width);
				WholeWidth = Width = bmp.Width;
				WholeHeight = Height = bmp.Height;
				XOffset = 0;
				YOffset = 0;

				ex = null;
			}
			catch (Exception e)
			{
				ex = e;
				Pixels = null;
			}
		}

		public RSImage(int width, int height)
		{
			Pixels = new Color[width * height];
			Width = WholeWidth = width;
			Height = WholeHeight = height;
			XOffset = 0;
			YOffset = 0;
		}

		public Bitmap GenerateBitmap()
		{
			if (Pixels == null || Width <= 0 || Height <= 0 || Width >= 2048 || Height >= 2048)
				return null;
			Bitmap bmp = new Bitmap(Width, Height);
			FastPixel fp = new FastPixel(bmp);
			fp.Lock();
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					fp.SetPixel(x, y, Pixels[x + y * Width]);
				}
			}
			fp.Unlock(true);

			return bmp;
		}

		public Bitmap GeneratePaletteBitmap()
		{
			if (Palette == null)
				return null;
			Bitmap bmp = new Bitmap(256, 256);
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.Black);
			for (int i = 0; i < Palette.Length; i++)
				g.FillRectangle(new SolidBrush(Palette[i]), (i % 16) * 16, i / 16 * 16, 16, 16);
			return bmp;
		}

		public bool Write(Cache cache, CacheItemNode node, DataBuffer original)
		{
			try
			{
				DataBuffer index_data = cache.SubArchives[node.SubArchive].ExtractFile("index.dat");
				original.Location = 0;
				index_data.Location = original.ReadShort() + 4;
				byte pcount = index_data.ReadByte();
				if (pcount >= Palette.Length)
					index_data.Location -= 5;
				else //move to back
				{
					index_data.Location = index_data.Buffer.Length;
					byte[] temp = new byte[index_data.Buffer.Length + 12 + Palette.Length * 3];
					Array.Copy(index_data.Buffer, temp, index_data.Buffer.Length);
					index_data.Buffer = temp;
				}
				IndexLocation = index_data.Location;
				index_data.WriteShort(WholeWidth);
				index_data.WriteShort(WholeHeight);
				index_data.WriteByte((byte)Palette.Length);
				for (int i = 0; i < Palette.Length - 1; i++)
				{
					index_data.WriteByte(Palette[i].R);
					index_data.WriteByte(Palette[i].G);
					index_data.WriteByte(Palette[i].B);
				}
				index_data.WriteByte((byte)XOffset);
				index_data.WriteByte((byte)YOffset);
				index_data.WriteShort(Width);
				index_data.WriteShort(Height);

				index_data.WriteByte(0); //x, y (1 is y, x)
				byte[] data = new byte[Width * Height + 2];
				data[0] = (byte)(IndexLocation >> 8);
				data[1] = (byte)IndexLocation;
				for (int i = 0; i < Width * Height; i++)
				{
					//todo: optimize
					Color c = Pixels[i];
					for (int k = 0; k < Palette.Length; k++)
					{
						if (Palette[k].R == c.R && Palette[k].G == c.G && Palette[k].B == c.B && Palette[k].A == c.A)
						{
							if (c.A == 0)
								data[i + 2] = 0;
							else
								data[i + 2] = (byte)(k + 1);
						}
					}
				}

				cache.SubArchives[node.SubArchive].WriteFile("index.dat", index_data.Buffer);
				cache.SubArchives[node.SubArchive].WriteFile(node.FileIndex, data);
				cache.SubArchives[node.SubArchive].RewriteArchive();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void ResizeToWhole()
		{
			if (Width == WholeWidth && Height == WholeHeight)
				return;

			byte[] b = new byte[WholeWidth * WholeHeight];
			int i = 0;
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					b[x + XOffset + (y + YOffset) * WholeWidth] = PaletteIndexes[i++];
				}
			}

			PaletteIndexes = b;
			Width = WholeWidth;
			Height = WholeHeight;
			XOffset = 0;
			YOffset = 0;
		}

		public void SetPixels(int[] pixels)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (pixels[x + y * Width] != 0)
						Pixels[x + y * Width] = Color.FromArgb((int)((uint)pixels[x + y * Width] | 0xFF000000));
				}
			}
		}
	}
}
