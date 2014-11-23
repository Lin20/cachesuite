
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Cache_Editor_API;

namespace Cache_Editor_API.Graphics3D
{
	public class Rasterizer : DrawingArea
	{

		public static void nullLoader()
		{
			anIntArray1468 = null;
			anIntArray1468 = null;
			SIN = null;
			COS = null;
			line_pixel_locations = null;
			textures = null;
			aBooleanArray1475 = null;
			texture_color_cache = null;
			recently_used_textures = null;
			texture_cache = null;
			anIntArray1480 = null;
			rgb_palette = null;
			texture_pixels = null;
		}

		public static void method364()
		{
			line_pixel_locations = new int[DrawingArea.height];
			for (int j = 0; j < DrawingArea.height; j++)
				line_pixel_locations[j] = DrawingArea.width * j;

			center_x = DrawingArea.width / 2;
			center_y = DrawingArea.height / 2;
		}

		public static void method365(int j, int k)
		{
			line_pixel_locations = new int[k];
			for (int l = 0; l < k; l++)
				line_pixel_locations[l] = j * l;

			center_x = j / 2;
			center_y = k / 2;
		}

		public static void ClearTextureCache()
		{
			recently_used_textures = null;
			for (int j = 0; j < 50; j++)
				texture_cache[j] = null;

		}

		public static void ResetTextures()
		{
			if (recently_used_textures == null)
			{
				rut_index = 20;//was parameter
				if (low_memory)
					recently_used_textures = NetDrawingTools.Create2DArray<int>(rut_index, 16384); //new int[anInt1477][16384];
				else
					recently_used_textures = NetDrawingTools.Create2DArray<int>(rut_index, 0x10000); //new int[anInt1477][0x10000];
				for (int k = 0; k < 50; k++)
					texture_cache[k] = null;

			}
		}

		public static void LoadTextures(Cache cache)
		{
			loaded_texture_count = 0;
			for (int j = 0; j < 50; j++)
				try
				{
					textures[j] = new RSImage(cache.SubArchives[6], j);
					textures[j].ResizeToWhole();
					//texture_cache[j] = textures[j].;
					//if (low_memory && textures[j].WholeWidth == 128)
					//	textures[j].LoadHalf();
					//else
					//	textures[j].LoadFull();
					loaded_texture_count++;
				}
				catch (Exception) { }

		}

		public static int GetAverageColor(int index)
		{
			if (texture_color_cache[index] != 0)
				return texture_color_cache[index];
			int k = 0;
			int l = 0;
			int i1 = 0;
			int j1 = texture_pixels[index].Length;
			for (int k1 = 0; k1 < j1; k1++)
			{
				k += texture_pixels[index][k1] >> 16 & 0xff;
				l += texture_pixels[index][k1] >> 8 & 0xff;
				i1 += texture_pixels[index][k1] & 0xff;
			}

			int l1 = (k / j1 << 16) + (l / j1 << 8) + i1 / j1;
			l1 = method373(l1, 1.3999999999999999D);
			if (l1 == 0)
				l1 = 1;
			texture_color_cache[index] = l1;
			return l1;
		}

		public static void BumpTexture(int index)
		{
			if (texture_cache[index] == null)
				return;
			recently_used_textures[rut_index++] = texture_cache[index];
			texture_cache[index] = null;
		}

		private static int[] method371(int i)
		{
			anIntArray1480[i] = anInt1481++;
			if (texture_cache[i] != null)
				return texture_cache[i];
			int[] ai;
			if (rut_index > 0)
			{
				ai = recently_used_textures[--rut_index];
				recently_used_textures[rut_index] = null;
			}
			else
			{
				int j = 0;
				int k = -1;
				for (int l = 0; l < loaded_texture_count; l++)
					if (texture_cache[l] != null && (anIntArray1480[l] < j || k == -1))
					{
						j = anIntArray1480[l];
						k = l;
					}

				ai = texture_cache[k];
				texture_cache[k] = null;
			}
			texture_cache[i] = ai;
			RSImage background = textures[i];
			int[] ai1 = texture_pixels[i];
			if (low_memory)
			{
				aBooleanArray1475[i] = false;
				for (int i1 = 0; i1 < 4096; i1++)
				{
					int i2 = ai[i1] = ai1[background.PaletteIndexes[i1]] & 0xf8f8ff;
					if (i2 == 0)
						aBooleanArray1475[i] = true;
					ai[4096 + i1] = i2 - (int)((uint)(i2 >> 3)) & 0xf8f8ff;
					ai[8192 + i1] = i2 - (int)((uint)(i2 >> 2)) & 0xf8f8ff;
					ai[12288 + i1] = i2 - (int)((uint)(i2 >> 2)) - (int)((uint)(i2 >> 3)) & 0xf8f8ff;
				}

			}
			else
			{
				if (background.Width == 64)
				{
					for (int j1 = 0; j1 < 128; j1++)
					{
						for (int j2 = 0; j2 < 128; j2++)
							ai[j2 + (j1 << 7)] = ai1[background.PaletteIndexes[(j2 >> 1) + ((j1 >> 1) << 6)]];

					}

				}
				else
				{
					for (int k1 = 0; k1 < 16384; k1++)
						ai[k1] = ai1[background.PaletteIndexes[k1]];

				}
				aBooleanArray1475[i] = false;
				for (int l1 = 0; l1 < 16384; l1++)
				{
					ai[l1] &= 0xf8f8ff;
					int k2 = ai[l1];
					if (k2 == 0)
						aBooleanArray1475[i] = true;
					ai[16384 + l1] = k2 - (int)((uint)(k2 >> 3)) & 0xf8f8ff;
					ai[32768 + l1] = k2 - (int)((uint)(k2 >> 2)) & 0xf8f8ff;
					ai[49152 + l1] = k2 - (int)((uint)(k2 >> 2)) - (int)((uint)(k2 >> 3)) & 0xf8f8ff;
				}

			}
			return ai;
		}

		public static void ApplyBrightness(double d)
		{
			d += .01D * 0.029999999999999999D - 0.014999999999999999D;
			int j = 0;
			for (int k = 0; k < 512; k++)
			{
				double d1 = (double)(k / 8) / 64D + 0.0078125D;
				double d2 = (double)(k & 7) / 8D + 0.0625D;
				for (int k1 = 0; k1 < 128; k1++)
				{
					double d3 = (double)k1 / 128D;
					double d4 = d3;
					double d5 = d3;
					double d6 = d3;
					if (d2 != 0.0D)
					{
						double d7;
						if (d3 < 0.5D)
							d7 = d3 * (1.0D + d2);
						else
							d7 = (d3 + d2) - d3 * d2;
						double d8 = 2D * d3 - d7;
						double d9 = d1 + 0.33333333333333331D;
						if (d9 > 1.0D)
							d9--;
						double d10 = d1;
						double d11 = d1 - 0.33333333333333331D;
						if (d11 < 0.0D)
							d11++;
						if (6D * d9 < 1.0D)
							d4 = d8 + (d7 - d8) * 6D * d9;
						else
							if (2D * d9 < 1.0D)
								d4 = d7;
							else
								if (3D * d9 < 2D)
									d4 = d8 + (d7 - d8) * (0.66666666666666663D - d9) * 6D;
								else
									d4 = d8;
						if (6D * d10 < 1.0D)
							d5 = d8 + (d7 - d8) * 6D * d10;
						else
							if (2D * d10 < 1.0D)
								d5 = d7;
							else
								if (3D * d10 < 2D)
									d5 = d8 + (d7 - d8) * (0.66666666666666663D - d10) * 6D;
								else
									d5 = d8;
						if (6D * d11 < 1.0D)
							d6 = d8 + (d7 - d8) * 6D * d11;
						else
							if (2D * d11 < 1.0D)
								d6 = d7;
							else
								if (3D * d11 < 2D)
									d6 = d8 + (d7 - d8) * (0.66666666666666663D - d11) * 6D;
								else
									d6 = d8;
					}
					int l1 = (int)(d4 * 256D);
					int i2 = (int)(d5 * 256D);
					int j2 = (int)(d6 * 256D);
					int k2 = (l1 << 16) + (i2 << 8) + j2;
					k2 = method373(k2, d);
					if (k2 == 0)
						k2 = 1;
					rgb_palette[j++] = k2;
				}

			}

			for (int l = 0; l < 50; l++)
				if (textures[l] != null)
				{
					Color[] ai = textures[l].Palette;
					texture_pixels[l] = new int[ai.Length];
					for (int j1 = 0; j1 < ai.Length; j1++)
					{
						texture_pixels[l][j1] = method373(ai[j1].ToArgb() & 0xFFFFFF, d);
						if ((texture_pixels[l][j1] & 0xf8f8ff) == 0 && j1 != 0)
							texture_pixels[l][j1] = 1;
					}

				}

			for (int i1 = 0; i1 < 50; i1++)
				BumpTexture(i1);

		}

		private static int method373(int i, double d)
		{
			double d1 = (double)(i >> 16) / 256D;
			double d2 = (double)(i >> 8 & 0xff) / 256D;
			double d3 = (double)(i & 0xff) / 256D;
			d1 = Math.Pow(d1, d);
			d2 = Math.Pow(d2, d);
			d3 = Math.Pow(d3, d);
			int j = (int)(d1 * 256D);
			int k = (int)(d2 * 256D);
			int l = (int)(d3 * 256D);
			return (j << 16) + (k << 8) + l;
		}

		private static int crossProduct(int x1, int x2, int y1, int y2)
		{
			return (x1 * y2 - y1 * x2);
		}

		public static void DrawShadedTriangle(int y1, int y2, int y3, int x1, int x2, int x3, int c1, int c2,
				int c3)
		{
			/*int maxX = Math.Max(x1, Math.Max(x2, x3));
			int minX = Math.Min(x1, Math.Min(x2, x3));
			int maxY = Math.Max(y1, Math.Max(y2, y3));
			int minY = Math.Min(y1, Math.Min(y2, y3));

			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					int qx = x - x1;
					int qy = y - y1;

					float s = (float)crossProduct(qx, x3 - x1, qy, y3 - y1) / crossProduct(x2 - x1, x3 - x1, y2 - y1, y3 - y1);
					float t = (float)crossProduct(x2 - x1, qx, y2 - y1, qy) / crossProduct(x2 - x1, x3 - x1, y2 - y1, y3 - y1);

					if ((s >= 0) && (t >= 0) && (s + t <= 1))
					{
						DrawingArea.pixels[x + y * DrawingArea.width] = rgb_palette[c1];
					}
				}
			}*/

			int j2 = 0;
			int k2 = 0;
			if (y2 != y1)
			{
				j2 = (x2 - x1 << 16) / (y2 - y1);
				k2 = (c2 - c1 << 15) / (y2 - y1);
			}
			int l2 = 0;
			int i3 = 0;
			if (y3 != y2)
			{
				l2 = (x3 - x2 << 16) / (y3 - y2);
				i3 = (c3 - c2 << 15) / (y3 - y2);
			}
			int j3 = 0;
			int k3 = 0;
			if (y3 != y1)
			{
				j3 = (x1 - x3 << 16) / (y1 - y3);
				k3 = (c1 - c3 << 15) / (y1 - y3);
			}
			if (y1 <= y2 && y1 <= y3)
			{
				if (y1 >= DrawingArea.bottomY)
					return;
				if (y2 > DrawingArea.bottomY)
					y2 = DrawingArea.bottomY;
				if (y3 > DrawingArea.bottomY)
					y3 = DrawingArea.bottomY;
				if (y2 < y3)
				{
					x3 = x1 <<= 16;
					c3 = c1 <<= 15;
					if (y1 < 0)
					{
						x3 -= j3 * y1;
						x1 -= j2 * y1;
						c3 -= k3 * y1;
						c1 -= k2 * y1;
						y1 = 0;
					}
					x2 <<= 16;
					c2 <<= 15;
					if (y2 < 0)
					{
						x2 -= l2 * y2;
						c2 -= i3 * y2;
						y2 = 0;
					}
					if (y1 != y2 && j3 < j2 || y1 == y2 && j3 > l2)
					{
						y3 -= y2;
						y2 -= y1;
						for (y1 = line_pixel_locations[y1]; --y2 >= 0; y1 += DrawingArea.width)
						{
							DrawShadedLine(DrawingArea.pixels, y1, x3 >> 16, x1 >> 16, c3 >> 7, c1 >> 7);
							x3 += j3;
							x1 += j2;
							c3 += k3;
							c1 += k2;
						}

						while (--y3 >= 0)
						{
							DrawShadedLine(DrawingArea.pixels, y1, x3 >> 16, x2 >> 16, c3 >> 7, c2 >> 7);
							x3 += j3;
							x2 += l2;
							c3 += k3;
							c2 += i3;
							y1 += DrawingArea.width;
						}
						return;
					}
					y3 -= y2;
					y2 -= y1;
					for (y1 = line_pixel_locations[y1]; --y2 >= 0; y1 += DrawingArea.width)
					{
						DrawShadedLine(DrawingArea.pixels, y1, x1 >> 16, x3 >> 16, c1 >> 7, c3 >> 7);
						x3 += j3;
						x1 += j2;
						c3 += k3;
						c1 += k2;
					}

					while (--y3 >= 0)
					{
						DrawShadedLine(DrawingArea.pixels, y1, x2 >> 16, x3 >> 16, c2 >> 7, c3 >> 7);
						x3 += j3;
						x2 += l2;
						c3 += k3;
						c2 += i3;
						y1 += DrawingArea.width;
					}
					return;
				}
				x2 = x1 <<= 16;
				c2 = c1 <<= 15;
				if (y1 < 0)
				{
					x2 -= j3 * y1;
					x1 -= j2 * y1;
					c2 -= k3 * y1;
					c1 -= k2 * y1;
					y1 = 0;
				}
				x3 <<= 16;
				c3 <<= 15;
				if (y3 < 0)
				{
					x3 -= l2 * y3;
					c3 -= i3 * y3;
					y3 = 0;
				}
				if (y1 != y3 && j3 < j2 || y1 == y3 && l2 > j2)
				{
					y2 -= y3;
					y3 -= y1;
					for (y1 = line_pixel_locations[y1]; --y3 >= 0; y1 += DrawingArea.width)
					{
						DrawShadedLine(DrawingArea.pixels, y1, x2 >> 16, x1 >> 16, c2 >> 7, c1 >> 7);
						x2 += j3;
						x1 += j2;
						c2 += k3;
						c1 += k2;
					}

					while (--y2 >= 0)
					{
						DrawShadedLine(DrawingArea.pixels, y1, x3 >> 16, x1 >> 16, c3 >> 7, c1 >> 7);
						x3 += l2;
						x1 += j2;
						c3 += i3;
						c1 += k2;
						y1 += DrawingArea.width;
					}
					return;
				}
				y2 -= y3;
				y3 -= y1;
				for (y1 = line_pixel_locations[y1]; --y3 >= 0; y1 += DrawingArea.width)
				{
					DrawShadedLine(DrawingArea.pixels, y1, x1 >> 16, x2 >> 16, c1 >> 7, c2 >> 7);
					x2 += j3;
					x1 += j2;
					c2 += k3;
					c1 += k2;
				}

				while (--y2 >= 0)
				{
					DrawShadedLine(DrawingArea.pixels, y1, x1 >> 16, x3 >> 16, c1 >> 7, c3 >> 7);
					x3 += l2;
					x1 += j2;
					c3 += i3;
					c1 += k2;
					y1 += DrawingArea.width;
				}
				return;
			}
			if (y2 <= y3)
			{
				if (y2 >= DrawingArea.bottomY)
					return;
				if (y3 > DrawingArea.bottomY)
					y3 = DrawingArea.bottomY;
				if (y1 > DrawingArea.bottomY)
					y1 = DrawingArea.bottomY;
				if (y3 < y1)
				{
					x1 = x2 <<= 16;
					c1 = c2 <<= 15;
					if (y2 < 0)
					{
						x1 -= j2 * y2;
						x2 -= l2 * y2;
						c1 -= k2 * y2;
						c2 -= i3 * y2;
						y2 = 0;
					}
					x3 <<= 16;
					c3 <<= 15;
					if (y3 < 0)
					{
						x3 -= j3 * y3;
						c3 -= k3 * y3;
						y3 = 0;
					}
					if (y2 != y3 && j2 < l2 || y2 == y3 && j2 > j3)
					{
						y1 -= y3;
						y3 -= y2;
						for (y2 = line_pixel_locations[y2]; --y3 >= 0; y2 += DrawingArea.width)
						{
							DrawShadedLine(DrawingArea.pixels, y2, x1 >> 16, x2 >> 16, c1 >> 7, c2 >> 7);
							x1 += j2;
							x2 += l2;
							c1 += k2;
							c2 += i3;
						}

						while (--y1 >= 0)
						{
							DrawShadedLine(DrawingArea.pixels, y2, x1 >> 16, x3 >> 16, c1 >> 7, c3 >> 7);
							x1 += j2;
							x3 += j3;
							c1 += k2;
							c3 += k3;
							y2 += DrawingArea.width;
						}
						return;
					}
					y1 -= y3;
					y3 -= y2;
					for (y2 = line_pixel_locations[y2]; --y3 >= 0; y2 += DrawingArea.width)
					{
						DrawShadedLine(DrawingArea.pixels, y2, x2 >> 16, x1 >> 16, c2 >> 7, c1 >> 7);
						x1 += j2;
						x2 += l2;
						c1 += k2;
						c2 += i3;
					}

					while (--y1 >= 0)
					{
						DrawShadedLine(DrawingArea.pixels, y2, x3 >> 16, x1 >> 16, c3 >> 7, c1 >> 7);
						x1 += j2;
						x3 += j3;
						c1 += k2;
						c3 += k3;
						y2 += DrawingArea.width;
					}
					return;
				}
				x3 = x2 <<= 16;
				c3 = c2 <<= 15;
				if (y2 < 0)
				{
					x3 -= j2 * y2;
					x2 -= l2 * y2;
					c3 -= k2 * y2;
					c2 -= i3 * y2;
					y2 = 0;
				}
				x1 <<= 16;
				c1 <<= 15;
				if (y1 < 0)
				{
					x1 -= j3 * y1;
					c1 -= k3 * y1;
					y1 = 0;
				}
				if (j2 < l2)
				{
					y3 -= y1;
					y1 -= y2;
					for (y2 = line_pixel_locations[y2]; --y1 >= 0; y2 += DrawingArea.width)
					{
						DrawShadedLine(DrawingArea.pixels, y2, x3 >> 16, x2 >> 16, c3 >> 7, c2 >> 7);
						x3 += j2;
						x2 += l2;
						c3 += k2;
						c2 += i3;
					}

					while (--y3 >= 0)
					{
						DrawShadedLine(DrawingArea.pixels, y2, x1 >> 16, x2 >> 16, c1 >> 7, c2 >> 7);
						x1 += j3;
						x2 += l2;
						c1 += k3;
						c2 += i3;
						y2 += DrawingArea.width;
					}
					return;
				}
				y3 -= y1;
				y1 -= y2;
				for (y2 = line_pixel_locations[y2]; --y1 >= 0; y2 += DrawingArea.width)
				{
					DrawShadedLine(DrawingArea.pixels, y2, x2 >> 16, x3 >> 16, c2 >> 7, c3 >> 7);
					x3 += j2;
					x2 += l2;
					c3 += k2;
					c2 += i3;
				}

				while (--y3 >= 0)
				{
					DrawShadedLine(DrawingArea.pixels, y2, x2 >> 16, x1 >> 16, c2 >> 7, c1 >> 7);
					x1 += j3;
					x2 += l2;
					c1 += k3;
					c2 += i3;
					y2 += DrawingArea.width;
				}
				return;
			}
			if (y3 >= DrawingArea.bottomY)
				return;
			if (y1 > DrawingArea.bottomY)
				y1 = DrawingArea.bottomY;
			if (y2 > DrawingArea.bottomY)
				y2 = DrawingArea.bottomY;
			if (y1 < y2)
			{
				x2 = x3 <<= 16;
				c2 = c3 <<= 15;
				if (y3 < 0)
				{
					x2 -= l2 * y3;
					x3 -= j3 * y3;
					c2 -= i3 * y3;
					c3 -= k3 * y3;
					y3 = 0;
				}
				x1 <<= 16;
				c1 <<= 15;
				if (y1 < 0)
				{
					x1 -= j2 * y1;
					c1 -= k2 * y1;
					y1 = 0;
				}
				if (l2 < j3)
				{
					y2 -= y1;
					y1 -= y3;
					for (y3 = line_pixel_locations[y3]; --y1 >= 0; y3 += DrawingArea.width)
					{
						DrawShadedLine(DrawingArea.pixels, y3, x2 >> 16, x3 >> 16, c2 >> 7, c3 >> 7);
						x2 += l2;
						x3 += j3;
						c2 += i3;
						c3 += k3;
					}

					while (--y2 >= 0)
					{
						DrawShadedLine(DrawingArea.pixels, y3, x2 >> 16, x1 >> 16, c2 >> 7, c1 >> 7);
						x2 += l2;
						x1 += j2;
						c2 += i3;
						c1 += k2;
						y3 += DrawingArea.width;
					}
					return;
				}
				y2 -= y1;
				y1 -= y3;
				for (y3 = line_pixel_locations[y3]; --y1 >= 0; y3 += DrawingArea.width)
				{
					DrawShadedLine(DrawingArea.pixels, y3, x3 >> 16, x2 >> 16, c3 >> 7, c2 >> 7);
					x2 += l2;
					x3 += j3;
					c2 += i3;
					c3 += k3;
				}

				while (--y2 >= 0)
				{
					DrawShadedLine(DrawingArea.pixels, y3, x1 >> 16, x2 >> 16, c1 >> 7, c2 >> 7);
					x2 += l2;
					x1 += j2;
					c2 += i3;
					c1 += k2;
					y3 += DrawingArea.width;
				}
				return;
			}
			x1 = x3 <<= 16;
			c1 = c3 <<= 15;
			if (y3 < 0)
			{
				x1 -= l2 * y3;
				x3 -= j3 * y3;
				c1 -= i3 * y3;
				c3 -= k3 * y3;
				y3 = 0;
			}
			x2 <<= 16;
			c2 <<= 15;
			if (y2 < 0)
			{
				x2 -= j2 * y2;
				c2 -= k2 * y2;
				y2 = 0;
			}
			if (l2 < j3)
			{
				y1 -= y2;
				y2 -= y3;
				for (y3 = line_pixel_locations[y3]; --y2 >= 0; y3 += DrawingArea.width)
				{
					DrawShadedLine(DrawingArea.pixels, y3, x1 >> 16, x3 >> 16, c1 >> 7, c3 >> 7);
					x1 += l2;
					x3 += j3;
					c1 += i3;
					c3 += k3;
				}

				while (--y1 >= 0)
				{
					DrawShadedLine(DrawingArea.pixels, y3, x2 >> 16, x3 >> 16, c2 >> 7, c3 >> 7);
					x2 += j2;
					x3 += j3;
					c2 += k2;
					c3 += k3;
					y3 += DrawingArea.width;
				}
				return;
			}
			y1 -= y2;
			y2 -= y3;
			for (y3 = line_pixel_locations[y3]; --y2 >= 0; y3 += DrawingArea.width)
			{
				DrawShadedLine(DrawingArea.pixels, y3, x3 >> 16, x1 >> 16, c3 >> 7, c1 >> 7);
				x1 += l2;
				x3 += j3;
				c1 += i3;
				c3 += k3;
			}

			while (--y1 >= 0)
			{
				DrawShadedLine(DrawingArea.pixels, y3, x3 >> 16, x2 >> 16, c3 >> 7, c2 >> 7);
				x2 += j2;
				x3 += j3;
				c2 += k2;
				c3 += k3;
				y3 += DrawingArea.width;
			}
		}

		private static void DrawShadedLine(int[] dest, int offset, int x1, int x2, int c1, int c2)
		{
			int j;//was parameter
			int pixels_left;//was parameter
			int color_grad;
			if (aBoolean1464)
			{
				if (aBoolean1462)
				{
					if (x2 - x1 > 3)
						color_grad = (c2 - c1) / (x2 - x1);
					else
						color_grad = 0;
					if (x2 > DrawingArea.centerX)
						x2 = DrawingArea.centerX;
					if (x1 < 0)
					{
						c1 -= x1 * color_grad;
						x1 = 0;
					}
					if (x1 >= x2)
						return;
					offset += x1;
					pixels_left = x2 - x1 >> 2;
					color_grad <<= 2;
				}
				else
				{
					if (x1 >= x2)
						return;
					offset += x1;
					pixels_left = x2 - x1 >> 2;
					if (pixels_left > 0)
						color_grad = (c2 - c1) * anIntArray1468[pixels_left] >> 15;
					else
						color_grad = 0;
				}
				if (alpha == 0)
				{
					while (--pixels_left >= 0)
					{
						j = rgb_palette[c1 >> 8];
						c1 += color_grad;
						dest[offset++] = j;
						dest[offset++] = j;
						dest[offset++] = j;
						dest[offset++] = j;
					}
					pixels_left = x2 - x1 & 3;
					if (pixels_left > 0)
					{
						j = rgb_palette[c1 >> 8];
						do
							dest[offset++] = j;
						while (--pixels_left > 0);
						return;
					}
				}
				else
				{
					int j2 = alpha;
					int l2 = 256 - alpha;
					while (--pixels_left >= 0)
					{
						j = rgb_palette[c1 >> 8];
						c1 += color_grad;
						j = ((j & 0xff00ff) * l2 >> 8 & 0xff00ff) + ((j & 0xff00) * l2 >> 8 & 0xff00);
						dest[offset++] = j + ((dest[offset] & 0xff00ff) * j2 >> 8 & 0xff00ff) + ((dest[offset] & 0xff00) * j2 >> 8 & 0xff00);
						dest[offset++] = j + ((dest[offset] & 0xff00ff) * j2 >> 8 & 0xff00ff) + ((dest[offset] & 0xff00) * j2 >> 8 & 0xff00);
						dest[offset++] = j + ((dest[offset] & 0xff00ff) * j2 >> 8 & 0xff00ff) + ((dest[offset] & 0xff00) * j2 >> 8 & 0xff00);
						dest[offset++] = j + ((dest[offset] & 0xff00ff) * j2 >> 8 & 0xff00ff) + ((dest[offset] & 0xff00) * j2 >> 8 & 0xff00);
					}
					pixels_left = x2 - x1 & 3;
					if (pixels_left > 0)
					{
						j = rgb_palette[c1 >> 8];
						j = ((j & 0xff00ff) * l2 >> 8 & 0xff00ff) + ((j & 0xff00) * l2 >> 8 & 0xff00);
						do
							dest[offset++] = j + ((dest[offset] & 0xff00ff) * j2 >> 8 & 0xff00ff) + ((dest[offset] & 0xff00) * j2 >> 8 & 0xff00);
						while (--pixels_left > 0);
					}
				}
				return;
			}
			if (x1 >= x2)
				return;
			color_grad = (c2 - c1) / (x2 - x1);
			if (aBoolean1462)
			{
				if (x2 > DrawingArea.centerX)
					x2 = DrawingArea.centerX;
				if (x1 < 0)
				{
					c1 -= x1 * color_grad;
					x1 = 0;
				}
				if (x1 >= x2)
					return;
			}
			offset += x1;
			pixels_left = x2 - x1;
			if (alpha == 0)
			{
				do
				{
					dest[offset++] = rgb_palette[c1 >> 8];
					c1 += color_grad;
				} while (--pixels_left > 0);
				return;
			}
			int k2 = alpha;
			int i3 = 256 - alpha;
			do
			{
				j = rgb_palette[c1 >> 8];
				c1 += color_grad;
				j = ((j & 0xff00ff) * i3 >> 8 & 0xff00ff) + ((j & 0xff00) * i3 >> 8 & 0xff00);
				dest[offset++] = j + ((dest[offset] & 0xff00ff) * k2 >> 8 & 0xff00ff) + ((dest[offset] & 0xff00) * k2 >> 8 & 0xff00);
			} while (--pixels_left > 0);
		}

		public static void DrawFlatTriangle(int y1, int y2, int y3, int x1, int x2, int x3, int color)
		{
			int l1 = 0;
			if (y2 != y1)
				l1 = (x2 - x1 << 16) / (y2 - y1);
			int i2 = 0;
			if (y3 != y2)
				i2 = (x3 - x2 << 16) / (y3 - y2);
			int j2 = 0;
			if (y3 != y1)
				j2 = (x1 - x3 << 16) / (y1 - y3);
			if (y1 <= y2 && y1 <= y3)
			{
				if (y1 >= DrawingArea.bottomY)
					return;
				if (y2 > DrawingArea.bottomY)
					y2 = DrawingArea.bottomY;
				if (y3 > DrawingArea.bottomY)
					y3 = DrawingArea.bottomY;
				if (y2 < y3)
				{
					x3 = x1 <<= 16;
					if (y1 < 0)
					{
						x3 -= j2 * y1;
						x1 -= l1 * y1;
						y1 = 0;
					}
					x2 <<= 16;
					if (y2 < 0)
					{
						x2 -= i2 * y2;
						y2 = 0;
					}
					if (y1 != y2 && j2 < l1 || y1 == y2 && j2 > i2)
					{
						y3 -= y2;
						y2 -= y1;
						for (y1 = line_pixel_locations[y1]; --y2 >= 0; y1 += DrawingArea.width)
						{
							DrawHorizontalLine(DrawingArea.pixels, y1, color, x3 >> 16, x1 >> 16);
							x3 += j2;
							x1 += l1;
						}

						while (--y3 >= 0)
						{
							DrawHorizontalLine(DrawingArea.pixels, y1, color, x3 >> 16, x2 >> 16);
							x3 += j2;
							x2 += i2;
							y1 += DrawingArea.width;
						}
						return;
					}
					y3 -= y2;
					y2 -= y1;
					for (y1 = line_pixel_locations[y1]; --y2 >= 0; y1 += DrawingArea.width)
					{
						DrawHorizontalLine(DrawingArea.pixels, y1, color, x1 >> 16, x3 >> 16);
						x3 += j2;
						x1 += l1;
					}

					while (--y3 >= 0)
					{
						DrawHorizontalLine(DrawingArea.pixels, y1, color, x2 >> 16, x3 >> 16);
						x3 += j2;
						x2 += i2;
						y1 += DrawingArea.width;
					}
					return;
				}
				x2 = x1 <<= 16;
				if (y1 < 0)
				{
					x2 -= j2 * y1;
					x1 -= l1 * y1;
					y1 = 0;
				}
				x3 <<= 16;
				if (y3 < 0)
				{
					x3 -= i2 * y3;
					y3 = 0;
				}
				if (y1 != y3 && j2 < l1 || y1 == y3 && i2 > l1)
				{
					y2 -= y3;
					y3 -= y1;
					for (y1 = line_pixel_locations[y1]; --y3 >= 0; y1 += DrawingArea.width)
					{
						DrawHorizontalLine(DrawingArea.pixels, y1, color, x2 >> 16, x1 >> 16);
						x2 += j2;
						x1 += l1;
					}

					while (--y2 >= 0)
					{
						DrawHorizontalLine(DrawingArea.pixels, y1, color, x3 >> 16, x1 >> 16);
						x3 += i2;
						x1 += l1;
						y1 += DrawingArea.width;
					}
					return;
				}
				y2 -= y3;
				y3 -= y1;
				for (y1 = line_pixel_locations[y1]; --y3 >= 0; y1 += DrawingArea.width)
				{
					DrawHorizontalLine(DrawingArea.pixels, y1, color, x1 >> 16, x2 >> 16);
					x2 += j2;
					x1 += l1;
				}

				while (--y2 >= 0)
				{
					DrawHorizontalLine(DrawingArea.pixels, y1, color, x1 >> 16, x3 >> 16);
					x3 += i2;
					x1 += l1;
					y1 += DrawingArea.width;
				}
				return;
			}
			if (y2 <= y3)
			{
				if (y2 >= DrawingArea.bottomY)
					return;
				if (y3 > DrawingArea.bottomY)
					y3 = DrawingArea.bottomY;
				if (y1 > DrawingArea.bottomY)
					y1 = DrawingArea.bottomY;
				if (y3 < y1)
				{
					x1 = x2 <<= 16;
					if (y2 < 0)
					{
						x1 -= l1 * y2;
						x2 -= i2 * y2;
						y2 = 0;
					}
					x3 <<= 16;
					if (y3 < 0)
					{
						x3 -= j2 * y3;
						y3 = 0;
					}
					if (y2 != y3 && l1 < i2 || y2 == y3 && l1 > j2)
					{
						y1 -= y3;
						y3 -= y2;
						for (y2 = line_pixel_locations[y2]; --y3 >= 0; y2 += DrawingArea.width)
						{
							DrawHorizontalLine(DrawingArea.pixels, y2, color, x1 >> 16, x2 >> 16);
							x1 += l1;
							x2 += i2;
						}

						while (--y1 >= 0)
						{
							DrawHorizontalLine(DrawingArea.pixels, y2, color, x1 >> 16, x3 >> 16);
							x1 += l1;
							x3 += j2;
							y2 += DrawingArea.width;
						}
						return;
					}
					y1 -= y3;
					y3 -= y2;
					for (y2 = line_pixel_locations[y2]; --y3 >= 0; y2 += DrawingArea.width)
					{
						DrawHorizontalLine(DrawingArea.pixels, y2, color, x2 >> 16, x1 >> 16);
						x1 += l1;
						x2 += i2;
					}

					while (--y1 >= 0)
					{
						DrawHorizontalLine(DrawingArea.pixels, y2, color, x3 >> 16, x1 >> 16);
						x1 += l1;
						x3 += j2;
						y2 += DrawingArea.width;
					}
					return;
				}
				x3 = x2 <<= 16;
				if (y2 < 0)
				{
					x3 -= l1 * y2;
					x2 -= i2 * y2;
					y2 = 0;
				}
				x1 <<= 16;
				if (y1 < 0)
				{
					x1 -= j2 * y1;
					y1 = 0;
				}
				if (l1 < i2)
				{
					y3 -= y1;
					y1 -= y2;
					for (y2 = line_pixel_locations[y2]; --y1 >= 0; y2 += DrawingArea.width)
					{
						DrawHorizontalLine(DrawingArea.pixels, y2, color, x3 >> 16, x2 >> 16);
						x3 += l1;
						x2 += i2;
					}

					while (--y3 >= 0)
					{
						DrawHorizontalLine(DrawingArea.pixels, y2, color, x1 >> 16, x2 >> 16);
						x1 += j2;
						x2 += i2;
						y2 += DrawingArea.width;
					}
					return;
				}
				y3 -= y1;
				y1 -= y2;
				for (y2 = line_pixel_locations[y2]; --y1 >= 0; y2 += DrawingArea.width)
				{
					DrawHorizontalLine(DrawingArea.pixels, y2, color, x2 >> 16, x3 >> 16);
					x3 += l1;
					x2 += i2;
				}

				while (--y3 >= 0)
				{
					DrawHorizontalLine(DrawingArea.pixels, y2, color, x2 >> 16, x1 >> 16);
					x1 += j2;
					x2 += i2;
					y2 += DrawingArea.width;
				}
				return;
			}
			if (y3 >= DrawingArea.bottomY)
				return;
			if (y1 > DrawingArea.bottomY)
				y1 = DrawingArea.bottomY;
			if (y2 > DrawingArea.bottomY)
				y2 = DrawingArea.bottomY;
			if (y1 < y2)
			{
				x2 = x3 <<= 16;
				if (y3 < 0)
				{
					x2 -= i2 * y3;
					x3 -= j2 * y3;
					y3 = 0;
				}
				x1 <<= 16;
				if (y1 < 0)
				{
					x1 -= l1 * y1;
					y1 = 0;
				}
				if (i2 < j2)
				{
					y2 -= y1;
					y1 -= y3;
					for (y3 = line_pixel_locations[y3]; --y1 >= 0; y3 += DrawingArea.width)
					{
						DrawHorizontalLine(DrawingArea.pixels, y3, color, x2 >> 16, x3 >> 16);
						x2 += i2;
						x3 += j2;
					}

					while (--y2 >= 0)
					{
						DrawHorizontalLine(DrawingArea.pixels, y3, color, x2 >> 16, x1 >> 16);
						x2 += i2;
						x1 += l1;
						y3 += DrawingArea.width;
					}
					return;
				}
				y2 -= y1;
				y1 -= y3;
				for (y3 = line_pixel_locations[y3]; --y1 >= 0; y3 += DrawingArea.width)
				{
					DrawHorizontalLine(DrawingArea.pixels, y3, color, x3 >> 16, x2 >> 16);
					x2 += i2;
					x3 += j2;
				}

				while (--y2 >= 0)
				{
					DrawHorizontalLine(DrawingArea.pixels, y3, color, x1 >> 16, x2 >> 16);
					x2 += i2;
					x1 += l1;
					y3 += DrawingArea.width;
				}
				return;
			}
			x1 = x3 <<= 16;
			if (y3 < 0)
			{
				x1 -= i2 * y3;
				x3 -= j2 * y3;
				y3 = 0;
			}
			x2 <<= 16;
			if (y2 < 0)
			{
				x2 -= l1 * y2;
				y2 = 0;
			}
			if (i2 < j2)
			{
				y1 -= y2;
				y2 -= y3;
				for (y3 = line_pixel_locations[y3]; --y2 >= 0; y3 += DrawingArea.width)
				{
					DrawHorizontalLine(DrawingArea.pixels, y3, color, x1 >> 16, x3 >> 16);
					x1 += i2;
					x3 += j2;
				}

				while (--y1 >= 0)
				{
					DrawHorizontalLine(DrawingArea.pixels, y3, color, x2 >> 16, x3 >> 16);
					x2 += l1;
					x3 += j2;
					y3 += DrawingArea.width;
				}
				return;
			}
			y1 -= y2;
			y2 -= y3;
			for (y3 = line_pixel_locations[y3]; --y2 >= 0; y3 += DrawingArea.width)
			{
				DrawHorizontalLine(DrawingArea.pixels, y3, color, x3 >> 16, x1 >> 16);
				x1 += i2;
				x3 += j2;
			}

			while (--y1 >= 0)
			{
				DrawHorizontalLine(DrawingArea.pixels, y3, color, x3 >> 16, x2 >> 16);
				x2 += l1;
				x3 += j2;
				y3 += DrawingArea.width;
			}
		}

		private static void DrawHorizontalLine(int[] ai, int i, int j, int l, int i1)
		{
			int k;//was parameter
			if (aBoolean1462)
			{
				if (i1 > DrawingArea.centerX)
					i1 = DrawingArea.centerX;
				if (l < 0)
					l = 0;
			}
			if (l >= i1)
				return;
			i += l;
			k = i1 - l >> 2;
			if (alpha == 0)
			{
				while (--k >= 0)
				{
					ai[i++] = j;
					ai[i++] = j;
					ai[i++] = j;
					ai[i++] = j;
				}
				for (k = i1 - l & 3; --k >= 0; )
					ai[i++] = j;

				return;
			}
			int j1 = alpha;
			int k1 = 256 - alpha;
			j = ((j & 0xff00ff) * k1 >> 8 & 0xff00ff) + ((j & 0xff00) * k1 >> 8 & 0xff00);
			while (--k >= 0)
			{
				ai[i++] = j + ((ai[i] & 0xff00ff) * j1 >> 8 & 0xff00ff) + ((ai[i] & 0xff00) * j1 >> 8 & 0xff00);
				ai[i++] = j + ((ai[i] & 0xff00ff) * j1 >> 8 & 0xff00ff) + ((ai[i] & 0xff00) * j1 >> 8 & 0xff00);
				ai[i++] = j + ((ai[i] & 0xff00ff) * j1 >> 8 & 0xff00ff) + ((ai[i] & 0xff00) * j1 >> 8 & 0xff00);
				ai[i++] = j + ((ai[i] & 0xff00ff) * j1 >> 8 & 0xff00ff) + ((ai[i] & 0xff00) * j1 >> 8 & 0xff00);
			}
			for (k = i1 - l & 3; --k >= 0; )
				ai[i++] = j + ((ai[i] & 0xff00ff) * j1 >> 8 & 0xff00ff) + ((ai[i] & 0xff00) * j1 >> 8 & 0xff00);

		}

		public static void DrawTexturedTriangle(int y1, int y2, int y3, int x1, int x2, int x3, int k1, int l1,
				int i2, int j2, int k2, int l2, int i3, int j3, int k3,
				int l3, int i4, int j4, int texture_index)
		{
			int[] ai = method371(texture_index);
			aBoolean1463 = !aBooleanArray1475[texture_index];
			k2 = j2 - k2;
			j3 = i3 - j3;
			i4 = l3 - i4;
			l2 -= j2;
			k3 -= i3;
			j4 -= l3;
			int l4 = l2 * i3 - k3 * j2 << 14;
			int i5 = k3 * l3 - j4 * i3 << 8;
			int j5 = j4 * j2 - l2 * l3 << 5;
			int k5 = k2 * i3 - j3 * j2 << 14;
			int l5 = j3 * l3 - i4 * i3 << 8;
			int i6 = i4 * j2 - k2 * l3 << 5;
			int j6 = j3 * l2 - k2 * k3 << 14;
			int k6 = i4 * k3 - j3 * j4 << 8;
			int l6 = k2 * j4 - i4 * l2 << 5;
			int i7 = 0;
			int j7 = 0;
			if (y2 != y1)
			{
				i7 = (x2 - x1 << 16) / (y2 - y1);
				j7 = (l1 - k1 << 16) / (y2 - y1);
			}
			int k7 = 0;
			int l7 = 0;
			if (y3 != y2)
			{
				k7 = (x3 - x2 << 16) / (y3 - y2);
				l7 = (i2 - l1 << 16) / (y3 - y2);
			}
			int i8 = 0;
			int j8 = 0;
			if (y3 != y1)
			{
				i8 = (x1 - x3 << 16) / (y1 - y3);
				j8 = (k1 - i2 << 16) / (y1 - y3);
			}
			if (y1 <= y2 && y1 <= y3)
			{
				if (y1 >= DrawingArea.bottomY)
					return;
				if (y2 > DrawingArea.bottomY)
					y2 = DrawingArea.bottomY;
				if (y3 > DrawingArea.bottomY)
					y3 = DrawingArea.bottomY;
				if (y2 < y3)
				{
					x3 = x1 <<= 16;
					i2 = k1 <<= 16;
					if (y1 < 0)
					{
						x3 -= i8 * y1;
						x1 -= i7 * y1;
						i2 -= j8 * y1;
						k1 -= j7 * y1;
						y1 = 0;
					}
					x2 <<= 16;
					l1 <<= 16;
					if (y2 < 0)
					{
						x2 -= k7 * y2;
						l1 -= l7 * y2;
						y2 = 0;
					}
					int k8 = y1 - center_y;
					l4 += j5 * k8;
					k5 += i6 * k8;
					j6 += l6 * k8;
					if (y1 != y2 && i8 < i7 || y1 == y2 && i8 > k7)
					{
						y3 -= y2;
						y2 -= y1;
						y1 = line_pixel_locations[y1];
						while (--y2 >= 0)
						{
							DrawTexturedLine(DrawingArea.pixels, ai, y1, x3 >> 16, x1 >> 16, i2 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
							x3 += i8;
							x1 += i7;
							i2 += j8;
							k1 += j7;
							y1 += DrawingArea.width;
							l4 += j5;
							k5 += i6;
							j6 += l6;
						}
						while (--y3 >= 0)
						{
							DrawTexturedLine(DrawingArea.pixels, ai, y1, x3 >> 16, x2 >> 16, i2 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
							x3 += i8;
							x2 += k7;
							i2 += j8;
							l1 += l7;
							y1 += DrawingArea.width;
							l4 += j5;
							k5 += i6;
							j6 += l6;
						}
						return;
					}
					y3 -= y2;
					y2 -= y1;
					y1 = line_pixel_locations[y1];
					while (--y2 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y1, x1 >> 16, x3 >> 16, k1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
						x3 += i8;
						x1 += i7;
						i2 += j8;
						k1 += j7;
						y1 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					while (--y3 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y1, x2 >> 16, x3 >> 16, l1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
						x3 += i8;
						x2 += k7;
						i2 += j8;
						l1 += l7;
						y1 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					return;
				}
				x2 = x1 <<= 16;
				l1 = k1 <<= 16;
				if (y1 < 0)
				{
					x2 -= i8 * y1;
					x1 -= i7 * y1;
					l1 -= j8 * y1;
					k1 -= j7 * y1;
					y1 = 0;
				}
				x3 <<= 16;
				i2 <<= 16;
				if (y3 < 0)
				{
					x3 -= k7 * y3;
					i2 -= l7 * y3;
					y3 = 0;
				}
				int l8 = y1 - center_y;
				l4 += j5 * l8;
				k5 += i6 * l8;
				j6 += l6 * l8;
				if (y1 != y3 && i8 < i7 || y1 == y3 && k7 > i7)
				{
					y2 -= y3;
					y3 -= y1;
					y1 = line_pixel_locations[y1];
					while (--y3 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y1, x2 >> 16, x1 >> 16, l1 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
						x2 += i8;
						x1 += i7;
						l1 += j8;
						k1 += j7;
						y1 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					while (--y2 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y1, x3 >> 16, x1 >> 16, i2 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
						x3 += k7;
						x1 += i7;
						i2 += l7;
						k1 += j7;
						y1 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					return;
				}
				y2 -= y3;
				y3 -= y1;
				y1 = line_pixel_locations[y1];
				while (--y3 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y1, x1 >> 16, x2 >> 16, k1 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
					x2 += i8;
					x1 += i7;
					l1 += j8;
					k1 += j7;
					y1 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				while (--y2 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y1, x1 >> 16, x3 >> 16, k1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
					x3 += k7;
					x1 += i7;
					i2 += l7;
					k1 += j7;
					y1 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				return;
			}
			if (y2 <= y3)
			{
				if (y2 >= DrawingArea.bottomY)
					return;
				if (y3 > DrawingArea.bottomY)
					y3 = DrawingArea.bottomY;
				if (y1 > DrawingArea.bottomY)
					y1 = DrawingArea.bottomY;
				if (y3 < y1)
				{
					x1 = x2 <<= 16;
					k1 = l1 <<= 16;
					if (y2 < 0)
					{
						x1 -= i7 * y2;
						x2 -= k7 * y2;
						k1 -= j7 * y2;
						l1 -= l7 * y2;
						y2 = 0;
					}
					x3 <<= 16;
					i2 <<= 16;
					if (y3 < 0)
					{
						x3 -= i8 * y3;
						i2 -= j8 * y3;
						y3 = 0;
					}
					int i9 = y2 - center_y;
					l4 += j5 * i9;
					k5 += i6 * i9;
					j6 += l6 * i9;
					if (y2 != y3 && i7 < k7 || y2 == y3 && i7 > i8)
					{
						y1 -= y3;
						y3 -= y2;
						y2 = line_pixel_locations[y2];
						while (--y3 >= 0)
						{
							DrawTexturedLine(DrawingArea.pixels, ai, y2, x1 >> 16, x2 >> 16, k1 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
							x1 += i7;
							x2 += k7;
							k1 += j7;
							l1 += l7;
							y2 += DrawingArea.width;
							l4 += j5;
							k5 += i6;
							j6 += l6;
						}
						while (--y1 >= 0)
						{
							DrawTexturedLine(DrawingArea.pixels, ai, y2, x1 >> 16, x3 >> 16, k1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
							x1 += i7;
							x3 += i8;
							k1 += j7;
							i2 += j8;
							y2 += DrawingArea.width;
							l4 += j5;
							k5 += i6;
							j6 += l6;
						}
						return;
					}
					y1 -= y3;
					y3 -= y2;
					y2 = line_pixel_locations[y2];
					while (--y3 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y2, x2 >> 16, x1 >> 16, l1 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
						x1 += i7;
						x2 += k7;
						k1 += j7;
						l1 += l7;
						y2 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					while (--y1 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y2, x3 >> 16, x1 >> 16, i2 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
						x1 += i7;
						x3 += i8;
						k1 += j7;
						i2 += j8;
						y2 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					return;
				}
				x3 = x2 <<= 16;
				i2 = l1 <<= 16;
				if (y2 < 0)
				{
					x3 -= i7 * y2;
					x2 -= k7 * y2;
					i2 -= j7 * y2;
					l1 -= l7 * y2;
					y2 = 0;
				}
				x1 <<= 16;
				k1 <<= 16;
				if (y1 < 0)
				{
					x1 -= i8 * y1;
					k1 -= j8 * y1;
					y1 = 0;
				}
				int j9 = y2 - center_y;
				l4 += j5 * j9;
				k5 += i6 * j9;
				j6 += l6 * j9;
				if (i7 < k7)
				{
					y3 -= y1;
					y1 -= y2;
					y2 = line_pixel_locations[y2];
					while (--y1 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y2, x3 >> 16, x2 >> 16, i2 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
						x3 += i7;
						x2 += k7;
						i2 += j7;
						l1 += l7;
						y2 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					while (--y3 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y2, x1 >> 16, x2 >> 16, k1 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
						x1 += i8;
						x2 += k7;
						k1 += j8;
						l1 += l7;
						y2 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					return;
				}
				y3 -= y1;
				y1 -= y2;
				y2 = line_pixel_locations[y2];
				while (--y1 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y2, x2 >> 16, x3 >> 16, l1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
					x3 += i7;
					x2 += k7;
					i2 += j7;
					l1 += l7;
					y2 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				while (--y3 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y2, x2 >> 16, x1 >> 16, l1 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
					x1 += i8;
					x2 += k7;
					k1 += j8;
					l1 += l7;
					y2 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				return;
			}
			if (y3 >= DrawingArea.bottomY)
				return;
			if (y1 > DrawingArea.bottomY)
				y1 = DrawingArea.bottomY;
			if (y2 > DrawingArea.bottomY)
				y2 = DrawingArea.bottomY;
			if (y1 < y2)
			{
				x2 = x3 <<= 16;
				l1 = i2 <<= 16;
				if (y3 < 0)
				{
					x2 -= k7 * y3;
					x3 -= i8 * y3;
					l1 -= l7 * y3;
					i2 -= j8 * y3;
					y3 = 0;
				}
				x1 <<= 16;
				k1 <<= 16;
				if (y1 < 0)
				{
					x1 -= i7 * y1;
					k1 -= j7 * y1;
					y1 = 0;
				}
				int k9 = y3 - center_y;
				l4 += j5 * k9;
				k5 += i6 * k9;
				j6 += l6 * k9;
				if (k7 < i8)
				{
					y2 -= y1;
					y1 -= y3;
					y3 = line_pixel_locations[y3];
					while (--y1 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y3, x2 >> 16, x3 >> 16, l1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
						x2 += k7;
						x3 += i8;
						l1 += l7;
						i2 += j8;
						y3 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					while (--y2 >= 0)
					{
						DrawTexturedLine(DrawingArea.pixels, ai, y3, x2 >> 16, x1 >> 16, l1 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
						x2 += k7;
						x1 += i7;
						l1 += l7;
						k1 += j7;
						y3 += DrawingArea.width;
						l4 += j5;
						k5 += i6;
						j6 += l6;
					}
					return;
				}
				y2 -= y1;
				y1 -= y3;
				y3 = line_pixel_locations[y3];
				while (--y1 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y3, x3 >> 16, x2 >> 16, i2 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
					x2 += k7;
					x3 += i8;
					l1 += l7;
					i2 += j8;
					y3 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				while (--y2 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y3, x1 >> 16, x2 >> 16, k1 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
					x2 += k7;
					x1 += i7;
					l1 += l7;
					k1 += j7;
					y3 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				return;
			}
			x1 = x3 <<= 16;
			k1 = i2 <<= 16;
			if (y3 < 0)
			{
				x1 -= k7 * y3;
				x3 -= i8 * y3;
				k1 -= l7 * y3;
				i2 -= j8 * y3;
				y3 = 0;
			}
			x2 <<= 16;
			l1 <<= 16;
			if (y2 < 0)
			{
				x2 -= i7 * y2;
				l1 -= j7 * y2;
				y2 = 0;
			}
			int l9 = y3 - center_y;
			l4 += j5 * l9;
			k5 += i6 * l9;
			j6 += l6 * l9;
			if (k7 < i8)
			{
				y1 -= y2;
				y2 -= y3;
				y3 = line_pixel_locations[y3];
				while (--y2 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y3, x1 >> 16, x3 >> 16, k1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
					x1 += k7;
					x3 += i8;
					k1 += l7;
					i2 += j8;
					y3 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				while (--y1 >= 0)
				{
					DrawTexturedLine(DrawingArea.pixels, ai, y3, x2 >> 16, x3 >> 16, l1 >> 8, i2 >> 8, l4, k5, j6, i5, l5, k6);
					x2 += i7;
					x3 += i8;
					l1 += j7;
					i2 += j8;
					y3 += DrawingArea.width;
					l4 += j5;
					k5 += i6;
					j6 += l6;
				}
				return;
			}
			y1 -= y2;
			y2 -= y3;
			y3 = line_pixel_locations[y3];
			while (--y2 >= 0)
			{
				DrawTexturedLine(DrawingArea.pixels, ai, y3, x3 >> 16, x1 >> 16, i2 >> 8, k1 >> 8, l4, k5, j6, i5, l5, k6);
				x1 += k7;
				x3 += i8;
				k1 += l7;
				i2 += j8;
				y3 += DrawingArea.width;
				l4 += j5;
				k5 += i6;
				j6 += l6;
			}
			while (--y1 >= 0)
			{
				DrawTexturedLine(DrawingArea.pixels, ai, y3, x3 >> 16, x2 >> 16, i2 >> 8, l1 >> 8, l4, k5, j6, i5, l5, k6);
				x2 += i7;
				x3 += i8;
				l1 += j7;
				i2 += j8;
				y3 += DrawingArea.width;
				l4 += j5;
				k5 += i6;
				j6 += l6;
			}
		}

		public static void DrawTexturedLine(int[] dest, int[] t_src, int offset, int x1, int x2, int j1,
									  int k1, int l1, int i2, int j2, int k2, int l2, int i3)
		{
			int i = 0;//was parameter
			int j = 0;//was parameter
			if (x1 >= x2)
				return;
			int t_grad;
			int k3;
			if (aBoolean1462)
			{
				t_grad = (k1 - j1) / (x2 - x1);
				if (x2 > DrawingArea.centerX)
					x2 = DrawingArea.centerX;
				if (x1 < 0)
				{
					j1 -= x1 * t_grad;
					x1 = 0;
				}
				if (x1 >= x2)
					return;
				k3 = x2 - x1 >> 3;
				t_grad <<= 12;
				j1 <<= 9;
			}
			else
			{
				if (x2 - x1 > 7)
				{
					k3 = x2 - x1 >> 3;
					t_grad = (k1 - j1) * anIntArray1468[k3] >> 6;
				}
				else
				{
					k3 = 0;
					t_grad = 0;
				}
				j1 <<= 9;
			}
			offset += x1;
			if (low_memory)
			{
				int i4 = 0;
				int k4 = 0;
				int k6 = x1 - center_x;
				l1 += (k2 >> 3) * k6;
				i2 += (l2 >> 3) * k6;
				j2 += (i3 >> 3) * k6;
				int i5 = j2 >> 12;
				if (i5 != 0)
				{
					i = l1 / i5;
					j = i2 / i5;
					if (i < 0)
						i = 0;
					else
						if (i > 4032)
							i = 4032;
				}
				l1 += k2;
				i2 += l2;
				j2 += i3;
				i5 = j2 >> 12;
				if (i5 != 0)
				{
					i4 = l1 / i5;
					k4 = i2 / i5;
					if (i4 < 7)
						i4 = 7;
					else
						if (i4 > 4032)
							i4 = 4032;
				}
				int i7 = i4 - i >> 3;
				int k7 = k4 - j >> 3;
				i += (j1 & 0x600000) >> 3;
				int i8 = j1 >> 23;
				if (aBoolean1463)
				{
					while (k3-- > 0)
					{
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i = i4;
						j = k4;
						l1 += k2;
						i2 += l2;
						j2 += i3;
						int j5 = j2 >> 12;
						if (j5 != 0)
						{
							i4 = l1 / j5;
							k4 = i2 / j5;
							if (i4 < 7)
								i4 = 7;
							else
								if (i4 > 4032)
									i4 = 4032;
						}
						i7 = i4 - i >> 3;
						k7 = k4 - j >> 3;
						j1 += t_grad;
						i += (j1 & 0x600000) >> 3;
						i8 = j1 >> 23;
					}
					for (k3 = x2 - x1 & 7; k3-- > 0; )
					{
						dest[offset++] = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8);
						i += i7;
						j += k7;
					}

					return;
				}
				while (k3-- > 0)
				{
					int k8;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i += i7;
					j += k7;
					if ((k8 = (int)((uint)t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = k8;
					offset++;
					i = i4;
					j = k4;
					l1 += k2;
					i2 += l2;
					j2 += i3;
					int k5 = j2 >> 12;
					if (k5 != 0)
					{
						i4 = l1 / k5;
						k4 = i2 / k5;
						if (i4 < 7)
							i4 = 7;
						else
							if (i4 > 4032)
								i4 = 4032;
					}
					i7 = i4 - i >> 3;
					k7 = k4 - j >> 3;
					j1 += t_grad;
					i += (j1 & 0x600000) >> 3;
					i8 = j1 >> 23;
				}
				for (k3 = x2 - x1 & 7; k3-- > 0; )
				{
					int l8;
					if ((int)((uint)(l8 = t_src[(j & 0xfc0) + (i >> 6)] >> i8)) != 0)
						dest[offset] = l8;
					offset++;
					i += i7;
					j += k7;
				}

				return;
			}
			int j4 = 0;
			int l4 = 0;
			int l6 = x1 - center_x;
			l1 += (k2 >> 3) * l6;
			i2 += (l2 >> 3) * l6;
			j2 += (i3 >> 3) * l6;
			int l5 = j2 >> 14;
			if (l5 != 0)
			{
				i = l1 / l5;
				j = i2 / l5;
				if (i < 0)
					i = 0;
				else
					if (i > 16256)
						i = 16256;
			}
			l1 += k2;
			i2 += l2;
			j2 += i3;
			l5 = j2 >> 14;
			if (l5 != 0)
			{
				j4 = l1 / l5;
				l4 = i2 / l5;
				if (j4 < 7)
					j4 = 7;
				else
					if (j4 > 16256)
						j4 = 16256;
			}
			int j7 = j4 - i >> 3;
			int l7 = l4 - j >> 3;
			i += j1 & 0x600000;
			int j8 = j1 >> 23;
			if (aBoolean1463)
			{
				while (k3-- > 0)
				{
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i = j4;
					j = l4;
					l1 += k2;
					i2 += l2;
					j2 += i3;
					int i6 = j2 >> 14;
					if (i6 != 0)
					{
						j4 = l1 / i6;
						l4 = i2 / i6;
						if (j4 < 7)
							j4 = 7;
						else
							if (j4 > 16256)
								j4 = 16256;
					}
					j7 = j4 - i >> 3;
					l7 = l4 - j >> 3;
					j1 += t_grad;
					i += j1 & 0x600000;
					j8 = j1 >> 23;
				}
				for (k3 = x2 - x1 & 7; k3-- > 0; )
				{
					dest[offset++] = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8);
					i += j7;
					j += l7;
				}

				return;
			}
			while (k3-- > 0)
			{
				int i9;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i += j7;
				j += l7;
				if ((i9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = i9;
				offset++;
				i = j4;
				j = l4;
				l1 += k2;
				i2 += l2;
				j2 += i3;
				int j6 = j2 >> 14;
				if (j6 != 0)
				{
					j4 = l1 / j6;
					l4 = i2 / j6;
					if (j4 < 7)
						j4 = 7;
					else
						if (j4 > 16256)
							j4 = 16256;
				}
				j7 = j4 - i >> 3;
				l7 = l4 - j >> 3;
				j1 += t_grad;
				i += j1 & 0x600000;
				j8 = j1 >> 23;
			}
			for (int l3 = x2 - x1 & 7; l3-- > 0; )
			{
				int j9;
				if ((j9 = (int)((uint)t_src[(j & 0x3f80) + (i >> 7)] >> j8)) != 0)
					dest[offset] = j9;
				offset++;
				i += j7;
				j += l7;
			}

		}

		public static int anInt1459 = -477;
		public static bool low_memory = false;
		public static bool aBoolean1462;
		private static bool aBoolean1463;
		public static bool aBoolean1464 = true;
		public static int alpha;
		public static int center_x;
		public static int center_y;
		private static int[] anIntArray1468;
		public static int[] anIntArray1469;
		public static int[] SIN;
		public static int[] COS;
		public static int[] line_pixel_locations;
		private static int loaded_texture_count;
		public static RSImage[] textures = new RSImage[50];
		private static bool[] aBooleanArray1475 = new bool[50];
		private static int[] texture_color_cache = new int[50];
		private static int rut_index;
		private static int[][] recently_used_textures;
		private static int[][] texture_cache = new int[50][];
		public static int[] anIntArray1480 = new int[50];
		public static int anInt1481;
		public static int[] rgb_palette = new int[0x10000];
		private static int[][] texture_pixels = new int[50][];

		static Rasterizer()
		{
			anIntArray1468 = new int[512];
			anIntArray1469 = new int[2048];
			SIN = new int[2048];
			COS = new int[2048];
			for (int i = 1; i < 512; i++)
				anIntArray1468[i] = 32768 / i;

			for (int j = 1; j < 2048; j++)
				anIntArray1469[j] = 0x10000 / j;

			for (int k = 0; k < 2048; k++)
			{
				SIN[k] = (int)(65536D * Math.Sin((double)k * 0.0030679614999999999D));
				COS[k] = (int)(65536D * Math.Cos((double)k * 0.0030679614999999999D));
			}

		}
	}
}