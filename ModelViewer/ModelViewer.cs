using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Cache_Editor_API;
using Cache_Editor_API.Graphics3D;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace ModelViewer
{
	public class ModelViewer : IPlugin
	{
		//basic
		public string Name { get { return "Model Viewer"; } }
		public string Author { get { return "Lin"; } }
		public string Version { get { return "1.0"; } }
		public string Description { get { return "Views and allows the basic importing and exporting of 317-format models."; } }
		public EditorClassifications Classifications { get; set; } //what triggers this editor to be used
		public bool Dominant { get { return true; } }

		//properties to be filled in by the shell
		public CacheItemNode Node { get; set; }
		public DataBuffer Data { get; set; }
		public Cache Cache { get; set; }

		//optional
		public Control[] Controls { get; set; }
		public Control[] ToolControls { get; set; }
		public string StaticFileExtensions { get; set; }
		public string FileExtensions { get; set; }
		public Form ConfigureForm { get; set; }

		//private Bitmap checkerboard;
		private int last_x;
		private int last_y;
		private Model model;
		private int[] checkerboard;
		private int[] pixel_buffer;

		private int yaw;
		private int pitch;
		private int c_x;
		private int c_y;
		private int c_z;

		private SimpleOpenGlControl gl_control;
		private SelectablePictureBox software_control;
		private static bool software_rendering;
		private static bool initialized;

		public ModelViewer()
		{
			try
			{
				Classifications = new EditorClassifications();
				Classifications.Archives = new int[] { 1 };
				StaticFileExtensions = "";
				FileExtensions = "";
				ConfigureForm = new Configure();

				Controls = new Control[3];
				Label not = new Label();
				not.AutoSize = false;
				not.Text = "Not a model";
				not.Dock = DockStyle.Fill;
				not.TextAlign = ContentAlignment.MiddleCenter;
				not.Visible = false;
				Controls[1] = not;

				ToolControls = new Control[0];
				/*ToolboxControls t = new ToolboxControls();
				ToolControls = new Control[t.Controls[0].Controls.Count];
				for (int i = 0; i < t.Controls[0].Controls.Count; i++)
					ToolControls[i] = t.Controls[0].Controls[i];*/

				try
				{
					gl_control = new SimpleOpenGlControl();
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
					software_rendering = true;
				}

				software_control = new SelectablePictureBox();
				software_control.Dock = DockStyle.Fill;
				software_control.MouseDown += image_MouseDown;
				software_control.MouseMove += image_MouseMove;
				software_control.MouseWheel += image_MouseWheel;
				software_control.SizeChanged += gl_control_SizeChanged;
				software_control.KeyDown += gl_control_KeyDown;

				Controls[0] = gl_control;
				Controls[2] = software_control;

				model = null;
			}
			catch (Exception e)
			{
				MessageBox.Show("An error occurred while loading the model viewer.\nIt's probably related to OpenGL.\n\n" + e.Message + "\n\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void PluginSelected()
		{
			if (!initialized)
			{
				initialized = true;
				Rasterizer.LoadTextures(Cache);
				Rasterizer.ApplyBrightness(0.59999999999999998D);
				Rasterizer.ResetTextures();
			}

			c_z = 400;
			yaw = 0;
			pitch = 0;
			c_x = 0;
			c_y = 0;
			c_z = 400;
			try
			{
				Model.LoadModel(Data.Buffer, Node.FileIndex);
				model = new Model(Node.FileIndex);
				model.Light(64, 768, -50, -10, -50, true);
			}
			catch (Exception)
			{
				model = null;
			}

			DrawModel();
		}

		public bool OnImport(string filename)
		{
			try
			{
				Model.LoadModel(Data.Buffer, Node.FileIndex);
				model = new Model(Node.FileIndex);
				model.Light(64, 768, -50, -10, -50, true);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public void OnExport(string filename)
		{
		}

		public void OnConfigure()
		{
			software_rendering = ((Configure)ConfigureForm).checkBox1.Checked;
			if (software_rendering)
			{
				Controls[0].Visible = false;
				Controls[2].Visible = true;
			}
			else
			{
				Controls[0].Visible = true;
				Controls[2].Visible = false;
			}
			try
			{
				PluginSelected();
			}
			catch (Exception)
			{
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
			yaw += dY * 2;
			pitch -= dX * 2;
			DrawModel();
		}

		private void image_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta != 0)
			{
				c_z -= e.Delta / 4;
				if(c_z < 50)
					c_z = 50;
				DrawModel();
			}
		}

		private void gl_control_SizeChanged(object sender, EventArgs e)
		{
			int width = Controls[(software_rendering ? 2 : 0)].Width;
			int height = Controls[(software_rendering ? 2 : 0)].Height;
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
			if (initialized)
				DrawModel();
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

		private void DrawModel()
		{
			int width = Controls[(software_rendering ? 2 : 0)].Width;
			int height = Controls[(software_rendering ? 2 : 0)].Height;
			Array.Copy(checkerboard, pixel_buffer, pixel_buffer.Length);
			DrawingArea.initDrawingArea(height, width, pixel_buffer);
			Rasterizer.method364();

			bool success = false;

			if (Data.Buffer != null && Data.Buffer.Length > 0 && model != null)
			{
				try
				{
					int k3 = Rasterizer.center_x;
					int j4 = Rasterizer.center_y;
					Rasterizer.center_x = c_x + DrawingArea.width / 2;
					Rasterizer.center_y = c_y + DrawingArea.height / 2;
					while (yaw < 0)
						yaw += Model.SIN.Length;
					while (pitch < 0)
						pitch += Model.COS.Length;
					yaw %= Model.SIN.Length;
					pitch %= Model.COS.Length;
					int i5 = Rasterizer.SIN[yaw] * c_z >> 16;
					int l5 = Rasterizer.COS[yaw] * c_z >> 16;
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
					if (model != null)
						model.Render(pitch, 0, yaw, 0, i5, l5);
					Rasterizer.center_x = k3;
					Rasterizer.center_y = j4;

					if (!software_rendering)
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
				}
			}

			if (!success)
			{
				Controls[(software_rendering ? 2 : 0)].Visible = false;
				Controls[1].Visible = true;
				if (!software_rendering)
					gl_control.Invalidate();
				else
					software_control.Invalidate();
			}
			else
			{
				Controls[(software_rendering ? 2 : 0)].Visible = true;
				Controls[1].Visible = false;
			}
		}

		private void SetupTexture()
		{
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 3, Controls[(software_rendering ? 2 : 0)].Width, Controls[(software_rendering ? 2 : 0)].Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, pixel_buffer);

			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);

			Gl.glEnable(Gl.GL_TEXTURE_2D);
		}
	}
}