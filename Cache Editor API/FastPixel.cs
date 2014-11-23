using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace Cache_Editor_API
{
	public class FastPixel
	{
		public byte[] rgbValues = new byte[4];
		private BitmapData bmpData;
		private IntPtr bmpPtr;
		private bool locked = false;

		private bool _isAlpha = false;
		private Bitmap _bitmap;
		private int _width;
		private int _height;
		public int Width
		{
			get { return this._width; }
		}
		public int Height
		{
			get { return this._height; }
		}
		public bool IsAlphaBitmap
		{
			get { return this._isAlpha; }
		}
		public Bitmap Bitmap
		{
			get { return this._bitmap; }
		}

		public FastPixel(Bitmap bitmap)
		{
			if ((bitmap.PixelFormat == (bitmap.PixelFormat | PixelFormat.Indexed)))
			{
				throw new Exception("Cannot lock an Indexed image.");
			}
			this._bitmap = bitmap;
			this._isAlpha = (this.Bitmap.PixelFormat == (this.Bitmap.PixelFormat | PixelFormat.Alpha));
			this._width = bitmap.Width;
			this._height = bitmap.Height;
		}

		public void Lock()
		{
			if (this.locked)
			{
				throw new Exception("Bitmap already locked.");
			}

			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			this.bmpData = this.Bitmap.LockBits(rect, ImageLockMode.ReadWrite, this.Bitmap.PixelFormat);
			this.bmpPtr = this.bmpData.Scan0;

			if (this.IsAlphaBitmap)
			{
				int bytes = (this.Width * this.Height) * 4;
				this.rgbValues = new byte[bytes];

				System.Runtime.InteropServices.Marshal.Copy(this.bmpPtr, rgbValues, 0, this.rgbValues.Length);
			}
			else
			{
				int bytes = (this.Width * this.Height) * 3;
				this.rgbValues = new byte[bytes];

				System.Runtime.InteropServices.Marshal.Copy(this.bmpPtr, rgbValues, 0, this.rgbValues.Length);
			}

			this.locked = true;
		}
		public void Unlock(bool setPixels)
		{
			if (!this.locked)
			{
				throw new Exception("Bitmap not locked.");
			}
			// Copy the RGB values back to the bitmap
			if (setPixels) System.Runtime.InteropServices.Marshal.Copy(this.rgbValues, 0, this.bmpPtr, this.rgbValues.Length);
			// Unlock the bits.
			this.Bitmap.UnlockBits(bmpData);
			this.locked = false;
		}

		public void Clear(Color colour)
		{
			if (!this.locked)
			{
				throw new Exception("Bitmap not locked.");
			}

			if (this.IsAlphaBitmap)
			{
				for (int index = 0; index <= this.rgbValues.Length - 1; index += 4)
				{
					this.rgbValues[index] = colour.B;
					this.rgbValues[index + 1] = colour.G;
					this.rgbValues[index + 2] = colour.R;
					this.rgbValues[index + 3] = colour.A;
				}
			}
			else
			{
				for (int index = 0; index <= this.rgbValues.Length - 1; index += 3)
				{
					this.rgbValues[index] = colour.B;
					this.rgbValues[index + 1] = colour.G;
					this.rgbValues[index + 2] = colour.R;
				}
			}
		}
		public void SetPixel(Point location, Color colour)
		{
			this.SetPixel(location.X, location.Y, colour);
		}
		public void SetPixel(int x, int y, Color colour)
		{
			if (!this.locked)
			{
				throw new Exception("Bitmap not locked.");
			}

			if (this.IsAlphaBitmap)
			{
				int index = ((y * this.Width + x) * 4);
				this.rgbValues[index] = colour.B;
				this.rgbValues[index + 1] = colour.G;
				this.rgbValues[index + 2] = colour.R;
				this.rgbValues[index + 3] = colour.A;
			}
			else
			{
				int index = ((y * this.Width + x) * 3);
				this.rgbValues[index] = colour.B;
				this.rgbValues[index + 1] = colour.G;
				this.rgbValues[index + 2] = colour.R;
			}
		}
		public Color GetPixel(Point location)
		{
			return this.GetPixel(location.X, location.Y);
		}
		public Color GetPixel(int x, int y)
		{
			if (!this.locked)
			{
				throw new Exception("Bitmap not locked.");
			}

			if (this.IsAlphaBitmap)
			{
				int index = ((y * this.Width + x) * 4);
				int b = this.rgbValues[index];
				int g = this.rgbValues[index + 1];
				int r = this.rgbValues[index + 2];
				int a = this.rgbValues[index + 3];
				return Color.FromArgb(a, r, g, b);
			}
			else
			{
				int index = ((y * this.Width + x) * 3);
				int b = this.rgbValues[index];
				int g = this.rgbValues[index + 1];
				int r = this.rgbValues[index + 2];
				return Color.FromArgb(r, g, b);
			}
		}
	}
}