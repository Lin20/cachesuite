using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Cache_Editor_API.Graphics3D;

namespace Cache_Editor_API.Definitions
{
	public class ItemDefinition
	{
		public bool method192(int j)
		{
			int k = anInt175;
			int l = anInt166;
			if (j == 1)
			{
				k = anInt197;
				l = anInt173;
			}
			if (k == -1)
				return true;
			bool flag = true;
			if (!Model.method463(k))
				flag = false;
			if (l != -1 && !Model.method463(l))
				flag = false;
			return flag;
		}

		public static void UnpackItems(Cache loaded_cache, DataBuffer data_file, DataBuffer index_file)
		{
			LoadedCache = loaded_cache;
			stream = data_file;
			DataBuffer index_stream = index_file;
			TotalItems = index_stream.ReadShort();
			ItemDataLocations = new int[TotalItems];
			int i = 2;
			for (int j = 0; j < TotalItems; j++)
			{
				ItemDataLocations[j] = i;
				i += index_stream.ReadShort();
			}

			cache = new ItemDefinition[10];
			for (int k = 0; k < 10; k++)
				cache[k] = new ItemDefinition();

		}

		public Model method194(int j)
		{
			int k = anInt175;
			int l = anInt166;
			if (j == 1)
			{
				k = anInt197;
				l = anInt173;
			}
			if (k == -1)
				return null;
			Model model = Model.LoadModel(LoadedCache, k);
			if (l != -1)
			{
				Model model_1 = Model.LoadModel(LoadedCache, l);
				Model[] aclass30_sub2_sub4_sub6s = {
                    model, model_1
            };
				model = new Model(2, aclass30_sub2_sub4_sub6s);
			}
			if (modifiedModelColors != null)
			{
				for (int i1 = 0; i1 < modifiedModelColors.Length; i1++)
					model.method476(modifiedModelColors[i1], originalModelColors[i1]);

			}
			return model;
		}

		public bool method195(int j)
		{
			int k = anInt165;
			int l = anInt188;
			int i1 = anInt185;
			if (j == 1)
			{
				k = anInt200;
				l = anInt164;
				i1 = anInt162;
			}
			if (k == -1)
				return true;
			bool flag = true;
			if (!Model.method463(k))
				flag = false;
			if (l != -1 && !Model.method463(l))
				flag = false;
			if (i1 != -1 && !Model.method463(i1))
				flag = false;
			return flag;
		}

		public Model method196(int i)
		{
			int j = anInt165;
			int k = anInt188;
			int l = anInt185;
			if (i == 1)
			{
				j = anInt200;
				k = anInt164;
				l = anInt162;
			}
			if (j == -1)
				return null;
			Model model = Model.LoadModel(LoadedCache, j);
			if (k != -1)
				if (l != -1)
				{
					Model model_1 = Model.LoadModel(LoadedCache, k);
					Model model_3 = Model.LoadModel(LoadedCache, l);
					Model[] aclass30_sub2_sub4_sub6_1s = {
                        model, model_1, model_3
                };
					model = new Model(3, aclass30_sub2_sub4_sub6_1s);
				}
				else
				{
					Model model_2 = Model.LoadModel(LoadedCache, k);
					Model[] aclass30_sub2_sub4_sub6s = {
                        model, model_2
                };
					model = new Model(2, aclass30_sub2_sub4_sub6s);
				}
			if (i == 0 && aByte205 != 0)
				model.method475(0, aByte205, 0);
			if (i == 1 && aByte154 != 0)
				model.method475(0, aByte154, 0);
			if (modifiedModelColors != null)
			{
				for (int i1 = 0; i1 < modifiedModelColors.Length; i1++)
					model.method476(modifiedModelColors[i1], originalModelColors[i1]);

			}
			return model;
		}

		private void Reset()
		{
			ModelIndex = 0;
			Name = null;
			Description = null;
			modifiedModelColors = null;
			originalModelColors = null;
			Zoom = 2000;
			RotationX = 0;
			RotationY = 0;
			anInt204 = 0;
			modelOffset1 = 0;
			modelOffset2 = 0;
			Stackable = false;
			Cost = 1;
			MembersObject = false;
			GroundActions = null;
			Actions = null;
			anInt165 = -1;
			anInt188 = -1;
			aByte205 = 0;
			anInt200 = -1;
			anInt164 = -1;
			aByte154 = 0;
			anInt185 = -1;
			anInt162 = -1;
			anInt175 = -1;
			anInt166 = -1;
			anInt197 = -1;
			anInt173 = -1;
			stackIDs = null;
			stackAmounts = null;
			NotedID = -1;
			certTemplateID = -1;
			anInt167 = 128;
			anInt192 = 128;
			anInt191 = 128;
			anInt196 = 0;
			anInt184 = 0;
			team = 0;
		}

		public static ItemDefinition GetItem(int id)
		{
			for (int j = 0; j < 10; j++)
				if (cache[j].ID == id)
					return cache[j];

			cacheIndex = (cacheIndex + 1) % 10;
			ItemDefinition itemDef = cache[cacheIndex];
			stream.Location = ItemDataLocations[id];
			itemDef.ID = id;
			itemDef.Reset();
			itemDef.ReadItem(stream);
			if (itemDef.certTemplateID != -1)
				itemDef.ConvertToNote();
			return itemDef;
		}

		private void ConvertToNote()
		{
			ItemDefinition itemDef = GetItem(certTemplateID);
			ModelIndex = itemDef.ModelIndex;
			Zoom = itemDef.Zoom;
			RotationX = itemDef.RotationX;
			RotationY = itemDef.RotationY;

			anInt204 = itemDef.anInt204;
			modelOffset1 = itemDef.modelOffset1;
			modelOffset2 = itemDef.modelOffset2;
			modifiedModelColors = itemDef.modifiedModelColors;
			originalModelColors = itemDef.originalModelColors;
			ItemDefinition itemDef_1 = GetItem(NotedID);
			Name = itemDef_1.Name;
			MembersObject = itemDef_1.MembersObject;
			Cost = itemDef_1.Cost;
			string s = "a";
			char c = itemDef_1.Name[0];
			if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
				s = "an";
			Description = "Swap this note at any bank for " + s + " " + itemDef_1.Name + ".";
			Stackable = true;
		}

		public static RSImage GetModelSprite(int id, int amount, int k)
		{
			Color k_color = Color.FromArgb(k);
			ItemDefinition itemDef = GetItem(id);
			if (itemDef.stackIDs == null)
				amount = -1;
			if (amount > 1)
			{
				int i1 = -1;
				for (int j1 = 0; j1 < 10; j1++)
					if (amount >= itemDef.stackAmounts[j1] && itemDef.stackAmounts[j1] != 0)
						i1 = itemDef.stackIDs[j1];

				if (i1 != -1)
					itemDef = GetItem(i1);
			}
			Model model = itemDef.method201(1);
			if (model == null)
				return null;
			RSImage sprite = null;
			if (itemDef.certTemplateID != -1)
			{
				sprite = GetModelSprite(itemDef.NotedID, 10, -1);
				if (sprite == null)
					return null;
			}
			RSImage sprite2 = new RSImage(32, 32);
			int k1 = Rasterizer.center_x;
			int l1 = Rasterizer.center_y;
			int[] ai = Rasterizer.line_pixel_locations;
			int[] ai1 = DrawingArea.pixels;
			int i2 = DrawingArea.width;
			int j2 = DrawingArea.height;
			int k2 = DrawingArea.topX;
			int l2 = DrawingArea.bottomX;
			int i3 = DrawingArea.topY;
			int j3 = DrawingArea.bottomY;
			Rasterizer.aBoolean1464 = false;
			DrawingArea.initDrawingArea(32, 32, new int[32 * 32]);
			DrawingArea.method336(32, 0, 0, 0, 32);
			Rasterizer.method364();
			int k3 = itemDef.Zoom;
			if (k == -1)
				k3 = (int)((double)k3 * 1.5D);
			if (k > 0)
				k3 = (int)((double)k3 * 1.04D);
			int l3 = Rasterizer.SIN[itemDef.RotationX] * k3 >> 16;
			int i4 = Rasterizer.COS[itemDef.RotationX] * k3 >> 16;
			model.Render(itemDef.RotationY, itemDef.anInt204, itemDef.RotationX, itemDef.modelOffset1, l3 + model.modelHeight / 2 + itemDef.modelOffset2, i4 + itemDef.modelOffset2);
			for (int i5 = 31; i5 >= 0; i5--)
			{
				for (int j4 = 31; j4 >= 0; j4--)
					if (sprite2.Pixels[i5 + j4 * 32].ToArgb() == 0)
						if (i5 > 0 && sprite2.Pixels[(i5 - 1) + j4 * 32].ToArgb() > 1)
							sprite2.Pixels[i5 + j4 * 32] = Color.Black;
						else
							if (j4 > 0 && sprite2.Pixels[i5 + (j4 - 1) * 32].ToArgb() > 1)
								sprite2.Pixels[i5 + j4 * 32] = Color.Black;
							else
								if (i5 < 31 && sprite2.Pixels[i5 + 1 + j4 * 32].ToArgb() > 1)
									sprite2.Pixels[i5 + j4 * 32] = Color.Black;
								else
									if (j4 < 31 && sprite2.Pixels[i5 + (j4 + 1) * 32].ToArgb() > 1)
										sprite2.Pixels[i5 + j4 * 32] = Color.Black;

			}

			if (k > 0)
			{
				for (int j5 = 31; j5 >= 0; j5--)
				{
					for (int k4 = 31; k4 >= 0; k4--)
						if (sprite2.Pixels[j5 + k4 * 32].ToArgb() == 0)
							if (j5 > 0 && sprite2.Pixels[(j5 - 1) + k4 * 32].ToArgb() == 1)
								sprite2.Pixels[j5 + k4 * 32] = k_color;
							else
								if (k4 > 0 && sprite2.Pixels[j5 + (k4 - 1) * 32].ToArgb() == 1)
									sprite2.Pixels[j5 + k4 * 32] = k_color;
								else
									if (j5 < 31 && sprite2.Pixels[j5 + 1 + k4 * 32].ToArgb() == 1)
										sprite2.Pixels[j5 + k4 * 32] = k_color;
									else
										if (k4 < 31 && sprite2.Pixels[j5 + (k4 + 1) * 32].ToArgb() == 1)
											sprite2.Pixels[j5 + k4 * 32] = k_color;

				}

			}
			else if (k == 0)
			{
				for (int k5 = 31; k5 >= 0; k5--)
				{
					for (int l4 = 31; l4 >= 0; l4--)
						if (sprite2.Pixels[k5 + l4 * 32].ToArgb() == 0 && k5 > 0 && l4 > 0 && sprite2.Pixels[(k5 - 1) + (l4 - 1) * 32].ToArgb() > 0)
							sprite2.Pixels[k5 + l4 * 32] = Color.FromArgb(0x302020);

				}

			}
			if (itemDef.certTemplateID != -1)
			{
				int l5 = sprite.WholeWidth;
				int j6 = sprite.WholeHeight;
				sprite.WholeWidth = 32;
				sprite.WholeHeight = 32;
				DrawingArea.DrawRSImage(sprite, 0, 0);
				sprite.WholeWidth = l5;
				sprite.WholeHeight = j6;
			}
			sprite2.SetPixels(DrawingArea.pixels);
			DrawingArea.initDrawingArea(j2, i2, ai1);
			DrawingArea.setDrawingArea(j3, k2, l2, i3);
			Rasterizer.center_x = k1;
			Rasterizer.center_y = l1;
			Rasterizer.line_pixel_locations = ai;
			Rasterizer.aBoolean1464 = true;
			if (itemDef.Stackable)
				sprite2.WholeWidth = 33;
			else
				sprite2.WholeWidth = 32;
			sprite2.WholeHeight = amount;
			return sprite2;
		}

		public Model method201(int i)
		{
			if (stackIDs != null && i > 1)
			{
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (i >= stackAmounts[k] && stackAmounts[k] != 0)
						j = stackIDs[k];

				if (j != -1)
					return GetItem(j).method201(1);
			}
			Model model = Model.LoadModel(LoadedCache, ModelIndex);
			if (model == null)
				return null;
			if (anInt167 != 128 || anInt192 != 128 || anInt191 != 128)
				model.method478(anInt167, anInt191, anInt192);
			if (modifiedModelColors != null)
			{
				for (int l = 0; l < modifiedModelColors.Length; l++)
					model.method476(modifiedModelColors[l], originalModelColors[l]);

			}
			model.Light(64 + anInt196, 768 + anInt184, -50, -10, -50, true);
			model.aBoolean1659 = true;
			return model;
		}

		public Model method202(int i)
		{
			if (stackIDs != null && i > 1)
			{
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (i >= stackAmounts[k] && stackAmounts[k] != 0)
						j = stackIDs[k];

				if (j != -1)
					return GetItem(j).method202(1);
			}
			Model model = Model.LoadModel(LoadedCache, ModelIndex);
			if (model == null)
				return null;
			if (modifiedModelColors != null)
			{
				for (int l = 0; l < modifiedModelColors.Length; l++)
					model.method476(modifiedModelColors[l], originalModelColors[l]);

			}
			return model;
		}

		private void ReadItem(DataBuffer stream)
		{
			do
			{
				int i = stream.ReadByte();
				if (i == 0)
					return;
				if (i == 1)
					ModelIndex = stream.ReadShort();
				else if (i == 2)
					Name = stream.ReadString();
				else if (i == 3)
					Description = stream.ReadString();
				else if (i == 4)
					Zoom = stream.ReadShort();
				else if (i == 5)
					RotationX = stream.ReadShort();
				else if (i == 6)
					RotationY = stream.ReadShort();
				else if (i == 7)
				{
					modelOffset1 = stream.ReadShort();
					if (modelOffset1 > 32767)
						modelOffset1 -= 0x10000;
				}
				else if (i == 8)
				{
					modelOffset2 = stream.ReadShort();
					if (modelOffset2 > 32767)
						modelOffset2 -= 0x10000;
				}
				else if (i == 10)
					stream.ReadShort();
				else if (i == 11)
					Stackable = true;
				else if (i == 12)
					Cost = stream.ReadInteger();
				else if (i == 16)
					MembersObject = true;
				else if (i == 23)
				{
					anInt165 = stream.ReadShort();
					aByte205 = stream.ReadSignedByte();
				}
				else if (i == 24)
					anInt188 = stream.ReadShort();
				else if (i == 25)
				{
					anInt200 = stream.ReadShort();
					aByte154 = stream.ReadSignedByte();
				}
				else if (i == 26)
					anInt164 = stream.ReadShort();
				else if (i >= 30 && i < 35)
				{
					if (GroundActions == null)
						GroundActions = new String[5];
					GroundActions[i - 30] = stream.ReadString();
					if (GroundActions[i - 30].ToLower().Equals("hidden"))
						GroundActions[i - 30] = null;
				}
				else if (i >= 35 && i < 40)
				{
					if (Actions == null)
						Actions = new String[5];
					Actions[i - 35] = stream.ReadString();
				}
				else if (i == 40)
				{
					int j = stream.ReadByte();
					modifiedModelColors = new int[j];
					originalModelColors = new int[j];
					for (int k = 0; k < j; k++)
					{
						modifiedModelColors[k] = stream.ReadShort();
						originalModelColors[k] = stream.ReadShort();
					}

				}
				else if (i == 78)
					anInt185 = stream.ReadShort();
				else if (i == 79)
					anInt162 = stream.ReadShort();
				else if (i == 90)
					anInt175 = stream.ReadShort();
				else if (i == 91)
					anInt197 = stream.ReadShort();
				else if (i == 92)
					anInt166 = stream.ReadShort();
				else if (i == 93)
					anInt173 = stream.ReadShort();
				else if (i == 95)
					anInt204 = stream.ReadShort();
				else if (i == 97)
					NotedID = stream.ReadShort();
				else if (i == 98)
					certTemplateID = stream.ReadShort();
				else if (i >= 100 && i < 110)
				{
					if (stackIDs == null)
					{
						stackIDs = new int[10];
						stackAmounts = new int[10];
					}
					stackIDs[i - 100] = stream.ReadShort();
					stackAmounts[i - 100] = stream.ReadShort();
				}
				else if (i == 110)
					anInt167 = stream.ReadShort();
				else if (i == 111)
					anInt192 = stream.ReadShort();
				else if (i == 112)
					anInt191 = stream.ReadShort();
				else if (i == 113)
					anInt196 = stream.ReadSignedByte();
				else if (i == 114)
					anInt184 = stream.ReadSignedByte() * 5;
				else if (i == 115)
					team = stream.ReadByte();
			} while (true);
		}

		private ItemDefinition()
		{
			ID = -1;
		}

		public static Cache LoadedCache { get; set; }
		private sbyte aByte154;
		public int Cost { get; set; }
		private int[] modifiedModelColors;
		public int ID { get; set; }
		private int[] originalModelColors;
		public bool MembersObject { get; set; }
		private int anInt162;
		private int certTemplateID;
		private int anInt164;
		private int anInt165;
		private int anInt166;
		private int anInt167;
		public string[] GroundActions { get; set; }
		private int modelOffset1;
		public string Name { get; set; }
		private static ItemDefinition[] cache;
		private int anInt173;
		public int ModelIndex { get; set; }
		private int anInt175;
		public bool Stackable { get; set; }
		public string Description { get; set; }
		private int NotedID;
		private static int cacheIndex;
		public int Zoom { get; set; }
		private static DataBuffer stream;
		private int anInt184;
		private int anInt185;
		private int anInt188;
		public string[] Actions { get; set; }
		public int RotationX { get; set; }
		private int anInt191;
		private int anInt192;
		private int[] stackIDs;
		private int modelOffset2;
		private static int[] ItemDataLocations;
		private int anInt196;
		private int anInt197;
		public int RotationY { get; set; }
		private int anInt200;
		private int[] stackAmounts;
		public int team;
		public static int TotalItems;
		private int anInt204;
		private sbyte aByte205;

	}

}
