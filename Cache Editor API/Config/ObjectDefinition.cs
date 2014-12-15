using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Cache_Editor_API.Graphics3D;
using Cache_Editor_API;

namespace Cache_Editor_API.Config
{
	public class ObjectDefinition
	{
		public static ObjectDefinition GetObject(int i)
		{
			for (int j = 0; j < 20; j++)
				if (cache[j].ID == i)
					return cache[j];

			cacheIndex = (cacheIndex + 1) % 20;
			ObjectDefinition class46 = cache[cacheIndex];
			class46.ID = i;
			stream.Location = streamIndices[i];
			class46.Reset();
			class46.ReadObject(stream);
			return class46;
		}

		private void Reset()
		{
			ModelIndexes = null;
			anIntArray776 = null;
			Name = null;
			Description = null;
			ModelRecolorIndexes = null;
			ModelRecolorColors = null;
			TileSizeX = 1;
			TileSizeZ = 1;
			Clips = true;
			aBoolean757 = true;
			HasActions = false;
			RotateOnGround = false;
			FlatShading = false;
			aBoolean764 = false;
			AnimationID = -1;
			anInt775 = 16;
			LightIntensity = 0;
			LightDistance = 0;
			Actions = null;
			MinimapFunction = -1;
			MinimapIcon = -1;
			MirrorAlongX = false;
			aBoolean779 = true;
			ScaleX = 128;
			ScaleY = 128;
			ScaleZ = 128;
			anInt768 = 0;
			TranslationX = 0;
			TranslationY = 0;
			TranslationZ = 0;
			aBoolean736 = false;
			aBoolean766 = false;
			anInt760 = -1;
			anInt774 = -1;
			anInt749 = -1;
			SubObjectIDs = null;
		}

		/*public void method574(OnDemandFetcher class42_sub1)
		{
			if (anIntArray773 == null)
				return;
			for (int j = 0; j < anIntArray773.Length; j++)
				class42_sub1.method560(anIntArray773[j] & 0xffff, 0);
		}*/

		public static void nullLoader()
		{
			streamIndices = null;
			cache = null;
			stream = null;
		}

		public static void UnpackObjects(Cache loaded_cache, DataBuffer data_file, DataBuffer index_file)
		{
			LoadedCache = loaded_cache;
			stream = data_file;
			DataBuffer index_stream = index_file;
			int totalObjects = index_stream.ReadShort();
			streamIndices = new int[totalObjects];
			int i = 2;
			for (int j = 0; j < totalObjects; j++)
			{
				streamIndices[j] = i;
				i += index_stream.ReadShort();
			}

			cache = new ObjectDefinition[20];
			for (int k = 0; k < 20; k++)
				cache[k] = new ObjectDefinition();

		}

		public bool method577(int i)
		{
			if (anIntArray776 == null)
			{
				if (ModelIndexes == null)
					return true;
				if (i != 10)
					return true;
				bool flag1 = true;
				for (int k = 0; k < ModelIndexes.Length; k++)
					flag1 &= Model.method463(ModelIndexes[k] & 0xffff);

				return flag1;
			}
			for (int j = 0; j < anIntArray776.Length; j++)
				if (anIntArray776[j] == i)
					return Model.method463(ModelIndexes[j] & 0xffff);

			return true;
		}

		public Model GuessModel(int j, int k, int l, int i1, int j1, int k1)
		{
			Model model = null;
			int i = 0;
			for (i = 0; i <= 22; i++)
			{
				model = method581(i, k1, j);
				if (model != null)
					break;
			}
			GuessedType = i;
			if (model == null)
				return null;
			if (RotateOnGround || FlatShading)
				model = new Model(RotateOnGround, FlatShading, model);
			if (/*aBoolean762*/ false)
			{
				int l1 = (k + l + i1 + j1) / 4;
				for (int i2 = 0; i2 < model.VertexCount; i2++)
				{
					int j2 = model.vertices_x[i2];
					int k2 = model.vertices_z[i2];
					int l2 = k + ((l - k) * (j2 + 64)) / 128;
					int i3 = j1 + ((i1 - j1) * (j2 + 64)) / 128;
					int j3 = l2 + ((i3 - l2) * (k2 + 64)) / 128;
					model.vertices_y[i2] += j3 - l1;
				}

				model.method467();
			}
			return model;
		}

		public Model GetModel(int object_type, int j, int k, int l, int i1, int j1, int k1)
		{
			Model model = method581(object_type, k1, j);
			if (model == null)
				return null;
			if (RotateOnGround || FlatShading)
				model = new Model(RotateOnGround, FlatShading, model);
			if (/*aBoolean762*/ false)
			{
				int l1 = (k + l + i1 + j1) / 4;
				for (int i2 = 0; i2 < model.VertexCount; i2++)
				{
					int j2 = model.vertices_x[i2];
					int k2 = model.vertices_z[i2];
					int l2 = k + ((l - k) * (j2 + 64)) / 128;
					int i3 = j1 + ((i1 - j1) * (j2 + 64)) / 128;
					int j3 = l2 + ((i3 - l2) * (k2 + 64)) / 128;
					model.vertices_y[i2] += j3 - l1;
				}

				model.method467();
			}
			return model;
		}

		public bool method579()
		{
			if (ModelIndexes == null)
				return true;
			bool flag1 = true;
			for (int i = 0; i < ModelIndexes.Length; i++)
				flag1 &= Model.method463(ModelIndexes[i] & 0xffff);
			return flag1;
		}

		/*public ObjectDefinition method580()
		{
			int i = -1;
			if (anInt774 != -1)
			{
				VarBit varBit = VarBit.cache[anInt774];
				int j = varBit.anInt648;
				int k = varBit.anInt649;
				int l = varBit.anInt650;
				int i1 = client.anIntArray1232[l - k];
				i = clientInstance.variousSettings[j] >> k & i1;
			}
			else
				if (anInt749 != -1)
					i = clientInstance.variousSettings[anInt749];
			if (i < 0 || i >= childrenIDs.Length || childrenIDs[i] == -1)
				return null;
			else
				return forID(childrenIDs[i]);
		}*/

		private Model method581(int object_type, int k, int l)
		{
			Model model = null;
			long l1;
			if (anIntArray776 == null)
			{
				if (object_type != 10)
					return null;
				l1 = (long)((ID << 6) + l) + ((long)(k + 1) << 32);
				if (ModelIndexes == null)
					return null;
				bool flag1 = MirrorAlongX ^ (l > 3);
				int k1 = ModelIndexes.Length;
				for (int i2 = 0; i2 < k1; i2++)
				{
					int l2 = ModelIndexes[i2];
					if (flag1)
						l2 += 0x10000;
					model = null;// (Model)mruNodes1.insertFromCache(l2);
					if (model == null)
					{
						model = Model.LoadModel(LoadedCache, l2 & 0xffff);
						if (model == null)
							return null;
						if (flag1)
							model.method477();
						//mruNodes1.removeFromCache(model, l2);
					}
					if (k1 > 1)
						aModelArray741s[i2] = model;
				}

				if (k1 > 1)
					model = new Model(k1, aModelArray741s);
			}
			else
			{
				int i1 = -1;
				for (int j1 = 0; j1 < anIntArray776.Length; j1++)
				{
					if (anIntArray776[j1] != object_type)
						continue;
					i1 = j1;
					break;
				}

				if (i1 == -1)
					return null;
				l1 = (long)((ID << 6) + (i1 << 3) + l) + ((long)(k + 1) << 32);
				Model model_2 = null;// (Model)mruNodes2.insertFromCache(l1);
				if (model_2 != null)
					return model_2;
				int j2 = ModelIndexes[i1];
				bool flag3 = MirrorAlongX ^ (l > 3);
				if (flag3)
					j2 += 0x10000;
				model = null;// (Model)mruNodes1.insertFromCache(j2);
				if (model == null)
				{
					model = Model.LoadModel(LoadedCache, j2 & 0xffff);
					if (model == null)
						return null;
					if (flag3)
						model.method477();
					//mruNodes1.removeFromCache(model, j2);
				}
			}
			bool flag;
			flag = ScaleX != 128 || ScaleY != 128 || ScaleZ != 128;
			bool flag2;
			flag2 = TranslationX != 0 || TranslationY != 0 || TranslationZ != 0;
			Model model_3 = new Model(ModelRecolorIndexes == null, k == -1, l == 0 && k == -1 && !flag && !flag2, model);
			if (k != -1)
			{
				model_3.method469();
				model_3.method470(k);
				model_3.anIntArrayArray1658 = null;
				model_3.anIntArrayArray1657 = null;
			}
			while (l-- > 0)
				model_3.method473();
			if (ModelRecolorIndexes != null)
			{
				for (int k2 = 0; k2 < ModelRecolorIndexes.Length; k2++)
					model_3.SetColor(ModelRecolorIndexes[k2], ModelRecolorColors[k2]);

			}
			if (flag)
				model_3.Scale(ScaleX, ScaleZ, ScaleY);
			if (flag2)
				model_3.Translate(TranslationX, TranslationY, TranslationZ);
			model_3.Light(64 + LightIntensity, 768 + LightDistance * 5, -50, -10, -50, !FlatShading);
			if (anInt760 == 1)
				model_3.anInt1654 = model_3.modelHeight;
			//mruNodes2.removeFromCache(model_3, l1);
			return model_3;
		}

		private void ReadObject(DataBuffer stream)
		{
			int i = -1;

			do
			{
				int j;
				do
				{
					j = stream.ReadByte();
					if (j == 0)
						goto Finalize;
					if (j == 1)
					{
						int k = stream.ReadByte();
						if (k > 0)
						{
							if (ModelIndexes == null || lowMem)
							{
								anIntArray776 = new int[k];
								ModelIndexes = new int[k];
								for (int k1 = 0; k1 < k; k1++)
								{
									ModelIndexes[k1] = stream.ReadShort();
									anIntArray776[k1] = stream.ReadByte();
								}

							}
							else
							{
								stream.Location += k * 3;
							}
						}
					}
					else if (j == 2)
						Name = stream.ReadString();
					else if (j == 3)
						Description = stream.ReadString();
					else if (j == 5)
					{
						int l = stream.ReadByte();
						if (l > 0)
						{
							if (ModelIndexes == null || lowMem)
							{
								anIntArray776 = null;
								ModelIndexes = new int[l];
								for (int l1 = 0; l1 < l; l1++)
									ModelIndexes[l1] = stream.ReadShort();

							}
							else
							{
								stream.Location += l * 2;
							}
						}
					}
					else if (j == 14)
						TileSizeX = stream.ReadByte();
					else if (j == 15)
						TileSizeZ = stream.ReadByte();
					else if (j == 17)
						Clips = false;
					else if (j == 18)
						aBoolean757 = false;
					else if (j == 19)
					{
						i = stream.ReadByte();
						if (i == 1)
							HasActions = true;
					}
					else if (j == 21)
						RotateOnGround = true;
					else if (j == 22)
						FlatShading = true;
					else if (j == 23)
						aBoolean764 = true;
					else if (j == 24)
					{
						AnimationID = stream.ReadShort();
						if (AnimationID == 65535)
							AnimationID = -1;
					}
					else if (j == 28)
						anInt775 = stream.ReadByte();
					else if (j == 29)
						LightIntensity = stream.ReadSignedByte();
					else if (j == 39)
						LightDistance = stream.ReadSignedByte();
					else if (j >= 30 && j < 39)
					{
						if (Actions == null)
							Actions = new String[5];
						Actions[j - 30] = stream.ReadString();
						if (Actions[j - 30].ToLower().Equals("hidden"))
							Actions[j - 30] = null;
					}
					else if (j == 40)
					{
						int i1 = stream.ReadByte();
						ModelRecolorIndexes = new int[i1];
						ModelRecolorColors = new int[i1];
						for (int i2 = 0; i2 < i1; i2++)
						{
							ModelRecolorIndexes[i2] = stream.ReadShort();
							ModelRecolorColors[i2] = stream.ReadShort();
						}

					}
					else if (j == 60)
						MinimapFunction = stream.ReadShort();
					else if (j == 62)
						MirrorAlongX = true;
					else if (j == 64)
						aBoolean779 = false;
					else if (j == 65)
						ScaleX = stream.ReadShort();
					else if (j == 66)
						ScaleY = stream.ReadShort();
					else if (j == 67)
						ScaleZ = stream.ReadShort();
					else if (j == 68)
						MinimapIcon = stream.ReadShort();
					else if (j == 69)
						anInt768 = stream.ReadByte();
					else if (j == 70)
						TranslationX = stream.ReadSignedShort();
					else if (j == 71)
						TranslationY = stream.ReadSignedShort();
					else if (j == 72)
						TranslationZ = stream.ReadSignedShort();
					else if (j == 73)
						aBoolean736 = true;
					else if (j == 74)
					{
						aBoolean766 = true;
					}
					else
					{
						if (j != 75)
							continue;
						anInt760 = stream.ReadByte();
					}
					continue;
				} while (j != 77);

				anInt774 = stream.ReadShort();
				if (anInt774 == 65535)
					anInt774 = -1;
				anInt749 = stream.ReadShort();
				if (anInt749 == 65535)
					anInt749 = -1;
				int j1 = stream.ReadByte();
				SubObjectIDs = new int[j1 + 1];
				for (int j2 = 0; j2 <= j1; j2++)
				{
					SubObjectIDs[j2] = stream.ReadShort();
					if (SubObjectIDs[j2] == 65535)
						SubObjectIDs[j2] = -1;
				}

			} while (true);

		Finalize:
			if (i == -1)
			{
				HasActions = ModelIndexes != null && (anIntArray776 == null || anIntArray776[0] == 10);
				if (Actions != null)
					HasActions = true;
			}
			if (aBoolean766)
			{
				Clips = false;
				aBoolean757 = false;
			}
			if (anInt760 == -1)
				anInt760 = Clips ? 1 : 0;
		}

		private ObjectDefinition()
		{
			ID = -1;
		}

		[Category("Basic"), Description("The ID of the object.")]
		public int ID { get; private set; }
		[Category("Basic"), Description("The name of the object.")]
		public string Name { get; set; }
		[Category("Basic"), Description("The examine text.")]
		public string Description { get; set; }
		[Category("Basic"), Description("The additional right-click options when the item is in an interface.")]
		public string[] Actions { get; set; }
		[Category("Basic"), Description("Whether or not the object is right-clickable.")]
		public bool HasActions { get; private set; }
		[Category("Basic"), Description("The suggested object type. This is acquired by finding the first type that will return a model in the GetModel functions.")]
		public int GuessedType { get; private set; }
		[Category("Basic"), Description("The other objects rendered in this object."), ChangesModel(true)]
		public int[] SubObjectIDs { get; set; }

		[Category("Model"), Description("The indexes of the colors to change in the model."), ChangesModel(true)]
		public int[] ModelRecolorIndexes { get; set; }
		[Category("Model"), Description("The new color values, indexed in accordance to ModelRecolorIndexes."), ChangesModel(true)]
		public int[] ModelRecolorColors { get; set; }

		[Category("Model"), Description("The intensity of the light source."), ChangesModel(true)]
		public sbyte LightIntensity { get; set; }
		[Category("Model"), Description("The  distance the light source is from the model."), ChangesModel(true)]
		public sbyte LightDistance { get; set; }
		[Category("Model"), Description("The other models rendered in this object."), ChangesModel(true)]
		public int[] ModelIndexes { get; set; }

		[Category("Model"), Description("The X scale of the model."), ChangesModel(true)]
		public int ScaleX { get; set; }
		[Category("Model"), Description("The Y scale of the model."), ChangesModel(true)]
		public int ScaleY { get; set; }
		[Category("Model"), Description("The Z scale of the model."), ChangesModel(true)]
		public int ScaleZ { get; set; }
		[Category("Model"), Description("The position displacement along the X axis."), ChangesModel(true)]
		public int TranslationX { get; set; }
		[Category("Model"), Description("The position displacement along the Y axis."), ChangesModel(true)]
		public int TranslationY { get; set; }
		[Category("Model"), Description("The position displacement along the Z axis."), ChangesModel(true)]
		public int TranslationZ { get; set; }
		[Category("Model"), Description("Whether or not the model gets mirrored along the X axis."), ChangesModel(true)]
		public bool MirrorAlongX { get; set; }
		[Category("Model"), Description("Whether or not the model's triangles are shaded. This also has unknown side effects and almost always makes models black if this is natively set."), ChangesModel(true)]
		public bool FlatShading { get; set; }
		[Category("Model"), Description("The default animation ID."), ChangesModel(true)]
		public int AnimationID { get; set; }

		[Category("Map"), Description("The icon that gets displayed on the minimap.")]
		public int MinimapIcon { get; set; }
		[Category("Map"), Description("Whether or not the tile rotates so its bottom is parallel with the underlying tile."), ChangesModel(true)]
		public bool RotateOnGround { get; set; }
		[Category("Map"), Description("The width of the object for clipping, measured in tiles."), ChangesModel(true)]
		public int TileSizeX { get; set; }
		[Category("Map"), Description("The length of the object for clipping, measured in tiles."), ChangesModel(true)]
		public int TileSizeZ { get; set; }
		[Category("Map"), Description("Whether or not the object cannot be walked on."), ChangesModel(true)]
		public bool Clips { get; set; }


		public static Cache LoadedCache { get; set; }
		[ChangesModel(true)]
		public bool aBoolean736 { get; set; }
		public static Model[] aModelArray741s = new Model[4];
		[ChangesModel(true)]
		public int MinimapFunction { get; set; }
		[ChangesModel(true)]
		public int anInt749 { get; set; }
		public static bool lowMem;
		public static DataBuffer stream;
		public static int[] streamIndices;
		[ChangesModel(true)]
		public bool aBoolean757 { get; set; }
		[ChangesModel(true)]
		public int anInt760 { get; set; }
		[ChangesModel(true)]
		public bool aBoolean764 { get; set; }
		[ChangesModel(true)]
		public bool aBoolean766 { get; set; }
		[ChangesModel(true)]
		public int anInt768 { get; set; }
		public static int cacheIndex;
		[ChangesModel(true)]
		public int anInt774 { get; set; }
		[ChangesModel(true)]
		public int anInt775 { get; set; }
		[ChangesModel(true)]
		public int[] anIntArray776 { get; set; }
		[ChangesModel(true)]
		public bool aBoolean779 { get; set; }
		public static ObjectDefinition[] cache;
	}
}