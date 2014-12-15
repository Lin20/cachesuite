using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using Cache_Editor_API.Graphics3D;

namespace Cache_Editor_API.Config
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
			if (ModelRecolorIndexes != null)
			{
				for (int i1 = 0; i1 < ModelRecolorIndexes.Length; i1++)
					model.SetColor(ModelRecolorIndexes[i1], ModelRecolorColors[i1]);

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
				model.Translate(0, aByte205, 0);
			if (i == 1 && aByte154 != 0)
				model.Translate(0, aByte154, 0);
			if (ModelRecolorIndexes != null)
			{
				for (int i1 = 0; i1 < ModelRecolorIndexes.Length; i1++)
					model.SetColor(ModelRecolorIndexes[i1], ModelRecolorColors[i1]);

			}
			return model;
		}

		private void Reset()
		{
			ModelIndex = 0;
			Name = null;
			Description = null;
			ModelRecolorIndexes = null;
			ModelRecolorColors = null;
			Zoom = 2000;
			RotationX = 0;
			RotationY = 0;
			RotationZ = 0;
			SpriteX = 0;
			SpriteY = 0;
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
			StackedItemIDs = null;
			StackChangeAmounts = null;
			NotedID = -1;
			NotedModelItemID = -1;
			ScaleX = 128;
			ScaleY = 128;
			ScaleZ = 128;
			LightIntensity = 0;
			LightDistance = 0;
			Team = 0;
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
			if (itemDef.NotedModelItemID != -1)
				itemDef.ConvertToNote();
			return itemDef;
		}

		private void ConvertToNote()
		{
			ItemDefinition itemDef = GetItem(NotedModelItemID);
			ModelIndex = itemDef.ModelIndex;
			Zoom = itemDef.Zoom;
			RotationX = itemDef.RotationX;
			RotationY = itemDef.RotationY;

			RotationZ = itemDef.RotationZ;
			SpriteX = itemDef.SpriteX;
			SpriteY = itemDef.SpriteY;
			ModelRecolorIndexes = itemDef.ModelRecolorIndexes;
			ModelRecolorColors = itemDef.ModelRecolorColors;
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

		public static RSImage GetModelSprite(int id, int amount, Color selection_color)
		{
			return GetModelSprite(id, amount, selection_color.ToArgb());
		}

		public static RSImage GetModelSprite(int id, int amount, int selection_color)
		{
			Color k_color = Color.FromArgb(selection_color);
			ItemDefinition itemDef = GetItem(id);
			if (itemDef.StackedItemIDs == null)
				amount = -1;
			if (amount > 1)
			{
				int i1 = -1;
				for (int j1 = 0; j1 < 10; j1++)
					if (amount >= itemDef.StackChangeAmounts[j1] && itemDef.StackChangeAmounts[j1] != 0)
						i1 = itemDef.StackedItemIDs[j1];

				if (i1 != -1)
					itemDef = GetItem(i1);
			}
			Model model = itemDef.GetModel(1);
			if (model == null)
				return null;
			RSImage sprite = null;
			if (itemDef.NotedModelItemID != -1)
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
			if (selection_color == -1)
				k3 = (int)((double)k3 * 1.5D);
			if (selection_color > 0)
				k3 = (int)((double)k3 * 1.04D);
			int l3 = Rasterizer.SIN[itemDef.RotationX] * k3 >> 16;
			int i4 = Rasterizer.COS[itemDef.RotationX] * k3 >> 16;
			model.Render(itemDef.RotationY, itemDef.RotationZ, itemDef.RotationX, itemDef.SpriteX, l3 + model.modelHeight / 2 + itemDef.SpriteY, i4 + itemDef.SpriteY);
			
			//draw the black outline
			for (int i5 = 31; i5 >= 0; i5--)
			{
				for (int j4 = 31; j4 >= 0; j4--)
				{
					if (DrawingArea.pixels[i5 + j4 * 32] == 0)
					{
						if (i5 > 0 && DrawingArea.pixels[(i5 - 1) + j4 * 32] > 1)
							DrawingArea.pixels[i5 + j4 * 32] = 1;
						else if (j4 > 0 && DrawingArea.pixels[i5 + (j4 - 1) * 32] > 1)
							DrawingArea.pixels[i5 + j4 * 32] = 1;
						else if (i5 < 31 && DrawingArea.pixels[i5 + 1 + j4 * 32] > 1)
							DrawingArea.pixels[i5 + j4 * 32] = 1;
						else if (j4 < 31 && DrawingArea.pixels[i5 + (j4 + 1) * 32] > 1)
							DrawingArea.pixels[i5 + j4 * 32] = 1;
					}
				}
			}

			//draw the selection outline or shadow
			if (selection_color > 0) //selection outline
			{
				for (int j5 = 31; j5 >= 0; j5--)
				{
					for (int k4 = 31; k4 >= 0; k4--)
					{
						if (DrawingArea.pixels[j5 + k4 * 32] == 0)
						{
							if (j5 > 0 && DrawingArea.pixels[(j5 - 1) + k4 * 32] == 1)
								DrawingArea.pixels[j5 + k4 * 32] = selection_color;
							else if (k4 > 0 && DrawingArea.pixels[j5 + (k4 - 1) * 32] == 1)
								DrawingArea.pixels[j5 + k4 * 32] = selection_color;
							else if (j5 < 31 && DrawingArea.pixels[j5 + 1 + k4 * 32] == 1)
								DrawingArea.pixels[j5 + k4 * 32] = selection_color;
							else if (k4 < 31 && DrawingArea.pixels[j5 + (k4 + 1) * 32] == 1)
								DrawingArea.pixels[j5 + k4 * 32] = selection_color;
						}
					}
				}

			}
			else if (selection_color == 0) //shadow
			{
				for (int k5 = 31; k5 >= 0; k5--)
				{
					for (int l4 = 31; l4 >= 0; l4--)
					{
						if (DrawingArea.pixels[k5 + l4 * 32] == 0 && k5 > 0 && l4 > 0 && DrawingArea.pixels[(k5 - 1) + (l4 - 1) * 32] > 0)
							DrawingArea.pixels[k5 + l4 * 32] = 0x302020;
					}

				}

			}
			if (itemDef.NotedModelItemID != -1)
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

		public int GetModelIndex(int amount)
		{
			if (StackedItemIDs != null && amount > 1)
			{
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (amount >= StackChangeAmounts[k] && StackChangeAmounts[k] != 0)
						j = StackedItemIDs[k];

				if (j != -1)
					return GetItem(j).ModelIndex;
			}

			return ModelIndex;
		}

		public Model GetModel(int amount)
		{
			if (StackedItemIDs != null && amount > 1)
			{
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (amount >= StackChangeAmounts[k] && StackChangeAmounts[k] != 0)
						j = StackedItemIDs[k];

				if (j != -1)
					return GetItem(j).GetModel(1);
			}
			Model model = Model.LoadModel(LoadedCache, ModelIndex);
			if (model == null)
				return null;
			if (ScaleX != 128 || ScaleY != 128 || ScaleZ != 128)
				model.Scale(ScaleX, ScaleZ, ScaleY);
			if (ModelRecolorIndexes != null)
			{
				for (int l = 0; l < ModelRecolorIndexes.Length; l++)
					model.SetColor(ModelRecolorIndexes[l], ModelRecolorColors[l]);

			}
			model.Light(64 + LightIntensity, 768 + LightDistance, -50, -10, -50, true);
			model.aBoolean1659 = true;
			return model;
		}

		public Model GetModelWithoutLighting(int amount)
		{
			if (StackedItemIDs != null && amount > 1)
			{
				int j = -1;
				for (int k = 0; k < 10; k++)
					if (amount >= StackChangeAmounts[k] && StackChangeAmounts[k] != 0)
						j = StackedItemIDs[k];

				if (j != -1)
					return GetItem(j).GetModelWithoutLighting(1);
			}
			Model model = Model.LoadModel(LoadedCache, ModelIndex);
			if (model == null)
				return null;
			if (ModelRecolorIndexes != null)
			{
				for (int l = 0; l < ModelRecolorIndexes.Length; l++)
					model.SetColor(ModelRecolorIndexes[l], ModelRecolorColors[l]);

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
					SpriteX = stream.ReadShort();
					if (SpriteX > 32767)
						SpriteX -= 0x10000;
				}
				else if (i == 8)
				{
					SpriteY = stream.ReadShort();
					if (SpriteY > 32767)
						SpriteY -= 0x10000;
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
					ModelRecolorIndexes = new int[j];
					ModelRecolorColors = new int[j];
					for (int k = 0; k < j; k++)
					{
						ModelRecolorIndexes[k] = stream.ReadShort();
						ModelRecolorColors[k] = stream.ReadShort();
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
					RotationZ = stream.ReadShort();
				else if (i == 97)
					NotedID = stream.ReadShort();
				else if (i == 98)
					NotedModelItemID = stream.ReadShort();
				else if (i >= 100 && i < 110)
				{
					if (StackedItemIDs == null)
					{
						StackedItemIDs = new int[10];
						StackChangeAmounts = new int[10];
					}
					StackedItemIDs[i - 100] = stream.ReadShort();
					StackChangeAmounts[i - 100] = stream.ReadShort();
				}
				else if (i == 110)
					ScaleX = stream.ReadShort();
				else if (i == 111)
					ScaleY = stream.ReadShort();
				else if (i == 112)
					ScaleZ = stream.ReadShort();
				else if (i == 113)
					LightIntensity = stream.ReadSignedByte();
				else if (i == 114)
					LightDistance = stream.ReadSignedByte() * 5;
				else if (i == 115)
					Team = stream.ReadByte();
			} while (true);
		}

		private ItemDefinition()
		{
			ID = -1;
		}

		[Category("Basic"), Description("The ID of the item.")]
		public int ID { get; private set; }
		[Category("Basic"), Description("The name of the item.")]
		public string Name { get; set; }
		[Category("Basic"), Description("The examine text.")]
		public string Description { get; set; }
		[Category("Basic"), Description("Whether or not the item is members-only.")]
		public bool MembersObject { get; set; }
		[Category("Basic"), Description("The additional right-click options when the item is on the ground.")]
		public string[] GroundActions { get; set; }
		[Category("Basic"), Description("The cost of the item.")]
		public int Cost { get; set; }
		[Category("Basic"), Description("The additional right-click options when the item is in an interface.")]
		public string[] Actions { get; set; }
		[Category("Basic"), Description("The team ID of the item. This is used for team capes.")]
		public int Team { get; set; }

		[Category("Stacking"), Description("Whether or not the item is stackable.")]
		public bool Stackable { get; set; }
		[Category("Stacking"), Description("The item ID used for the noted back model. It is generally the certificate model, 799.")]
		public int NotedModelItemID { get; set; }
		[Category("Stacking"), Description("The item IDs to change to as the item's amount changes.")]
		public int[] StackedItemIDs { get; set; }
		[Category("Stacking"), Description("The item amounts at which the stack ID changes.")]
		public int[] StackChangeAmounts { get; set; }

		[Category("Model"), Description("The item's associated model."), ChangesItemSprite(true), ChangesModel(true)]
		public int ModelIndex { get; set; }
		[Category("Model"), Description("The indexes of the colors to change in the model."), ChangesItemSprite(true), ChangesModel(true)]
		public int[] ModelRecolorIndexes { get; set; }
		[Category("Model"), Description("The new color values, indexed in accordance to ModelRecolorIndexes."), ChangesItemSprite(true), ChangesModel(true)]
		public int[] ModelRecolorColors { get; set; }
		[Category("Model"), Description("The distance the light source is from the model."), ChangesItemSprite(true), ChangesModel(true)]
		public int LightDistance { get; set; }
		[Category("Model"), Description("The intensity of the light source."), ChangesItemSprite(true), ChangesModel(true)]
		public int LightIntensity { get; set; }

		[Category("Sprite"), Description("The item index that appears on the bank note when the item is noted."), ChangesItemSprite(true)]
		public int NotedID { get; set; }
		[Category("Sprite"), Description("The X offset of the model's rendered sprite."), ChangesItemSprite(true)]
		public int SpriteX { get; set; }
		[Category("Sprite"), Description("The Y offset of the model's rendered sprite."), ChangesItemSprite(true)]
		public int SpriteY { get; set; }
		[Category("Sprite"), Description("The X rotation of the model's rendered sprite."), ChangesItemSprite(true)]
		public int RotationX { get; set; }
		[Category("Sprite"), Description("The Y rotation of the model's rendered sprite."), ChangesItemSprite(true)]
		public int RotationY { get; set; }
		[Category("Sprite"), Description("The Z rotation of the model's rendered sprite."), ChangesItemSprite(true)]
		public int RotationZ { get; set; }
		[Category("Sprite"), Description("The zoom of the model's rendered sprite."), ChangesItemSprite(true)]
		public int Zoom { get; set; }
		[Category("Sprite"), Description("The X scale of the model."), ChangesItemSprite(true), ChangesModel(true)]
		public int ScaleX { get; set; }
		[Category("Sprite"), Description("The Y scale of the model."), ChangesItemSprite(true), ChangesModel(true)]
		public int ScaleY { get; set; }
		[Category("Sprite"), Description("The Z scale of the model."), ChangesItemSprite(true), ChangesModel(true)]
		public int ScaleZ { get; set; }

		public static Cache LoadedCache { get; set; }
		public sbyte aByte154 { get; set; }
		public int anInt162 { get; set; }
		
		public int anInt164 { get; set; }
		public int anInt165 { get; set; }
		public int anInt166{ get; set; }
		private static ItemDefinition[] cache;
		public int anInt173 { get; set; }
		public int anInt175 { get; set; }
		private static int cacheIndex;
		private static DataBuffer stream;
		public int anInt185 { get; set; }
		public int anInt188 { get; set; }
		public static int[] ItemDataLocations;
		public int anInt197 { get; set; }
		public int anInt200 { get; set; }
		public static int TotalItems;
		public sbyte aByte205 { get; set; }

	}

}
