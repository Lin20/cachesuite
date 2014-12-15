using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cache_Editor_API;
using Cache_Editor_API.Graphics3D;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Cache_Editor_API.Controls
{
	public partial class ModelViewer : UserControl
	{
		private int last_x;
		private int last_y;
		private Model selected_model;
		public Model SelectedModel
		{
			get { return selected_model; }
			set
			{
				selected_model = value;
				c_z = 400;
				Yaw = 0;
				Pitch = 0;
				c_x = 0;
				c_y = 0;
				c_z = 400;
				DrawModel();
			}
		}
		private int[] checkerboard;
		private int[] pixel_buffer;

		public int Yaw { get; set; }
		public int Pitch { get; set; }
		public int Roll { get; set; }
		private int c_x;
		private int c_y;
		private int c_z;

		public SimpleOpenGlControl gl_control;
		public SelectablePictureBox software_control;
		public static bool SoftwareRendering { get; set; }

		public ModelViewer()
		{
			try
			{
				InitializeComponent();
				Label not = new Label();
				not.AutoSize = false;
				not.Text = "Not a model";
				not.Dock = DockStyle.Fill;
				not.TextAlign = ContentAlignment.MiddleCenter;
				not.Visible = false;
				Controls.Add(not);

				try
				{
					gl_control = new SimpleOpenGlControl();
					if (SoftwareRendering)
						gl_control.Visible = false;
					gl_control.Dock = DockStyle.Fill;
					gl_control.MouseDown += image_MouseDown;
					gl_control.MouseMove += image_MouseMove;
					gl_control.MouseWheel += image_MouseWheel;
					gl_control.InitializeContexts();
					gl_control.SizeChanged += gl_control_SizeChanged;
					gl_control.KeyDown += gl_control_KeyDown;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to initialize OpenGL.\n\n" + ex.Message + "\n\n" + ex.StackTrace, "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					SoftwareRendering = true;
				}

				software_control = new SelectablePictureBox();
				software_control.Dock = DockStyle.Fill;
				software_control.MouseDown += image_MouseDown;
				software_control.MouseMove += image_MouseMove;
				software_control.MouseWheel += image_MouseWheel;
				software_control.SizeChanged += gl_control_SizeChanged;
				software_control.KeyDown += gl_control_KeyDown;
				if (!SoftwareRendering)
					software_control.Visible = false;

				Controls.Add(gl_control);
				Controls.Add(software_control);

				selected_model = null;
			}
			catch (Exception e)
			{
				MessageBox.Show("An error occurred while loading the model viewer.\nIt's probably related to OpenGL.\n\n" + e.Message + "\n\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void image_MouseDown(object sender, MouseEventArgs e)
		{
			last_x = e.X;
			last_y = e.Y;
		}

		private void image_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			int dX = e.X - last_x;
			int dY = e.Y - last_y;
			last_x = e.X;
			last_y = e.Y;
			Yaw += dY * 2;
			Pitch -= dX * 2;
			DrawModel();
		}

		private void image_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta != 0)
			{
				c_z -= e.Delta / 4;
				if (c_z < 50)
					c_z = 50;
				DrawModel();
			}
		}

		private void gl_control_SizeChanged(object sender, EventArgs e)
		{
			UpdateBuffer();
		}

		private void gl_control_KeyDown(object sender, KeyEventArgs e)
		{
			bool redraw = false;
			if (e.KeyCode == Keys.A)
			{
				c_x -= 50;
				redraw = true;
			}
			if (e.KeyCode == Keys.D)
			{
				c_x += 50;
				redraw = true;
			}
			if (e.KeyCode == Keys.W)
			{
				c_y -= 50;
				redraw = true;
			}
			if (e.KeyCode == Keys.S)
			{
				c_y += 50;
				redraw = true;
			}
			if (redraw)
				DrawModel();
		}

		public void DrawModel()
		{
			int width = Controls[(SoftwareRendering ? 2 : 1)].Width;
			int height = Controls[(SoftwareRendering ? 2 : 1)].Height;
			Array.Copy(checkerboard, pixel_buffer, pixel_buffer.Length);
			DrawingArea.initDrawingArea(height, width, pixel_buffer);
			Rasterizer.method364();

			bool success = false;

			if (SelectedModel != null)
			{
				try
				{
					int k3 = Rasterizer.center_x;
					int j4 = Rasterizer.center_y;
					Rasterizer.center_x = c_x + DrawingArea.width / 2;
					Rasterizer.center_y = c_y + DrawingArea.height / 2;
					while (Yaw < 0)
						Yaw += Model.SIN.Length;
					while (Pitch < 0)
						Pitch += Model.COS.Length;
					while (Roll < 0)
						Roll += Model.COS.Length;
					Yaw %= Model.SIN.Length;
					Pitch %= Model.COS.Length;
					Roll %= Model.COS.Length;
					int i5 = Rasterizer.SIN[Yaw] * c_z >> 16;
					int l5 = Rasterizer.COS[Yaw] * c_z >> 16;
					bool animated = false; //interfaceIsSelected(class9_1);
					int i7;
					/*if (animated)
						i7 = class9_1.anInt258;
					else
						i7 = class9_1.anInt257;
					Model model;
					if (i7 == -1)
					{
						model = class9_1.method209(-1, -1, animated);
					}
					else
					{
						Animation animation = Animation.anims[i7];
						model = class9_1.method209(animation.anIntArray354[class9_1.anInt246], animation.anIntArray353[class9_1.anInt246], animated);
					}*/
					if (SelectedModel != null)
						SelectedModel.Render(Pitch, Roll, Yaw, 0, i5, l5);
					Rasterizer.center_x = k3;
					Rasterizer.center_y = j4;

					if (!SoftwareRendering)
					{
						Gl.glClearColor(.5f, 1, 1, 1);
						Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

						Gl.glMatrixMode(Gl.GL_PROJECTION);
						Gl.glLoadIdentity();
						Gl.glOrtho(0, width, 0, height, -1, 1);
						Gl.glMatrixMode(Gl.GL_MODELVIEW);
						Gl.glViewport(0, 0, width, height);

						Gl.glTexSubImage2D(Gl.GL_TEXTURE_2D, 0, 0, 0, width, height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, DrawingArea.pixels);
						Gl.glBegin(Gl.GL_QUADS);
						Gl.glTexCoord2d(0.0, 1.0); Gl.glVertex2d(0.0, 0.0);
						Gl.glTexCoord2d(1.0, 1.0); Gl.glVertex2d(width, 0.0);
						Gl.glTexCoord2d(1.0, 0.0); Gl.glVertex2d(width, height);
						Gl.glTexCoord2d(0.0, 0.0); Gl.glVertex2d(0.0, height);
						Gl.glEnd();

						gl_control.Invalidate();
					}
					else
					{
						software_control.Image = DrawingArea.ToBitmap();
					}
					success = true;
				}
				catch (Exception)
				{
					success = false;
				}
			}
			else
			{
				Controls[(SoftwareRendering ? 2 : 1)].Visible = false;
				Controls[0].Visible = true;
				this.Invalidate();
			}

			if (!success)
			{
				Controls[(SoftwareRendering ? 2 : 1)].Visible = false;
				Controls[0].Visible = true;
				if (!SoftwareRendering)
					gl_control.Invalidate();
				else
					software_control.Invalidate();
			}
			else
			{
				Controls[(SoftwareRendering ? 2 : 1)].Visible = true;
				Controls[0].Visible = false;
			}
		}

		private void SetupTexture()
		{
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 3, Controls[(SoftwareRendering ? 2 : 1)].Width, Controls[(SoftwareRendering ? 2 : 1)].Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, pixel_buffer);

			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);

			Gl.glEnable(Gl.GL_TEXTURE_2D);
		}

		public void SetModel(Model m, bool reset_view = true, bool redraw = true)
		{
			selected_model = m;
			if (reset_view)
			{
				c_z = 400;
				Yaw = 0;
				Pitch = 0;
				Roll = 0;
				c_x = 0;
				c_y = 0;
				c_z = 400;
			}
			if (selected_model == null)
			{
				Controls[(SoftwareRendering ? 2 : 1)].Visible = false;
				Controls[0].Visible = true;
			}
			else if (redraw)
			{
				Controls[(SoftwareRendering ? 2 : 1)].Visible = true;
				Controls[0].Visible = false;
				DrawModel();
			}
		}

		public void UpdateBuffer()
		{
			this.Refresh();
			int width = Controls[(SoftwareRendering ? 2 : 1)].Width;
			int height = Controls[(SoftwareRendering ? 2 : 1)].Height;
			checkerboard = new int[width * height];
			for (int i = 0; i < width * height; i++)
			{
				int x = i % width;
				int y = i / width;
				if ((x % 32 < 16 && y % 32 < 16) || (x % 32 >= 16 && y % 32 >= 16))
					checkerboard[i] = unchecked((int)0xFFD3D3D3);
				else
					checkerboard[i] = unchecked((int)0xFFFFFFFF);
			}

			pixel_buffer = new int[checkerboard.Length];
			SetupTexture();
			DrawModel();
		}
	}
}
