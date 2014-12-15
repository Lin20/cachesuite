using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Cache_Editor_API.Graphics3D;

namespace Cache_Editor_API.Config
{
	public class NPCDefinition
	{
		public static NPCDefinition GetNPC(int i)
		{
			for (int j = 0; j < 20; j++)
				if (cache[j].ID == (long)i)
					return cache[j];

			StorageCacheIndex = (StorageCacheIndex + 1) % 20;
			NPCDefinition entityDef = cache[StorageCacheIndex] = new NPCDefinition();
			stream.Location = streamIndices[i];
			entityDef.ID = i;
			entityDef.ReadNPC(stream);
			return entityDef;
		}

		public Model GetFaceModel()
		{
			/*if (childrenIDs != null)
			{
				NPCDefinition entityDef = method161();
				if (entityDef == null)
					return null;
				else
					return entityDef.method160();
			}*/
			if (FaceModels == null)
				return null;

			Model[] aclass30_sub2_sub4_sub6s = new Model[FaceModels.Length];
			for (int j = 0; j < FaceModels.Length; j++)
				aclass30_sub2_sub4_sub6s[j] = Model.LoadModel(LoadedCache, FaceModels[j]);

			Model model;
			if (aclass30_sub2_sub4_sub6s.Length == 1)
				model = aclass30_sub2_sub4_sub6s[0];
			else
				model = new Model(aclass30_sub2_sub4_sub6s.Length, aclass30_sub2_sub4_sub6s);
			if (ModelRecolorIndexes != null)
			{
				for (int k = 0; k < ModelRecolorIndexes.Length; k++)
					model.SetColor(ModelRecolorIndexes[k], ModelRecolorColors[k]);

			}
			return model;
		}

		/*public NPCDefinition method161()
		{
			int j = -1;
			if(anInt57 != -1)
			{
				VarBit varBit = VarBit.cache[anInt57];
				int k = varBit.anInt648;
				int l = varBit.anInt649;
				int i1 = varBit.anInt650;
				int j1 = client.anIntArray1232[i1 - l];
				j = clientInstance.variousSettings[k] >> l & j1;
			} else
			if(anInt59 != -1)
				j = clientInstance.variousSettings[anInt59];
			if(j < 0 || j >= childrenIDs.Length || childrenIDs[j] == -1)
				return null;
			else
				return forID(childrenIDs[j]);
		}*/

		public static void UnpackNPCs(Cache loaded_cache, DataBuffer data_file, DataBuffer index_file)
		{
			LoadedCache = loaded_cache;
			stream = data_file;
			DataBuffer index_stream = index_file;
			int totalNPCs = index_stream.ReadShort();
			streamIndices = new int[totalNPCs];
			int i = 2;
			for (int j = 0; j < totalNPCs; j++)
			{
				streamIndices[j] = i;
				i += index_stream.ReadShort();
			}

			cache = new NPCDefinition[20];
			for (int k = 0; k < 20; k++)
				cache[k] = new NPCDefinition();

		}

		public static void nullLoader()
		{
			streamIndices = null;
			cache = null;
			stream = null;
		}

		public Model GetModel(int j, int k, int[] ai)
		{
			/*if (childrenIDs != null)
			{
				NPCDefinition entityDef = method161();
				if (entityDef == null)
					return null;
				else
					return entityDef.method164(j, k, ai);
			}*/
			Model model = null;//(Model) mruNodes.insertFromCache(type);
			if (model == null)
			{
				if (ModelIndexes == null)
					return null;
				Model[] aclass30_sub2_sub4_sub6s = new Model[ModelIndexes.Length];
				for (int j1 = 0; j1 < ModelIndexes.Length; j1++)
					aclass30_sub2_sub4_sub6s[j1] = Model.LoadModel(LoadedCache, ModelIndexes[j1]);

				if (aclass30_sub2_sub4_sub6s.Length == 1)
					model = aclass30_sub2_sub4_sub6s[0];
				else
					model = new Model(aclass30_sub2_sub4_sub6s.Length, aclass30_sub2_sub4_sub6s);
				if (model == null)
					return null;
				if (ModelRecolorIndexes != null)
				{
					for (int k1 = 0; k1 < ModelRecolorIndexes.Length; k1++)
						model.SetColor(ModelRecolorIndexes[k1], ModelRecolorColors[k1]);

				}
				model.method469();
				model.Light(64 + LightIntensity, 850 + LightDistance, -30, -50, -30, true);
			}
			Model model_1 = Model.aModel_1621;
			model_1.method464(model, (k == -1) & (j == -1));
			if (k != -1 && j != -1)
				model_1.method471(ai, j, k);
			else
				if (k != -1)
					model_1.method470(k);
			if (ScaleXZ != 128 || ScaleY != 128)
				model_1.Scale(ScaleXZ, ScaleXZ, ScaleY);
			model_1.method466();
			model_1.anIntArrayArray1658 = null;
			model_1.anIntArrayArray1657 = null;
			if (Size == 1)
				model_1.aBoolean1659 = true;
			return model_1;
		}

		private void ReadNPC(DataBuffer stream)
		{
			do
			{
				int i = stream.ReadByte();
				if (i == 0)
					return;
				if (i == 1)
				{
					int j = stream.ReadByte();
					ModelIndexes = new int[j];
					for (int j1 = 0; j1 < j; j1++)
						ModelIndexes[j1] = stream.ReadShort();

				}
				else if (i == 2)
					Name = stream.ReadString();
				else if (i == 3)
					Description = stream.ReadString();
				else if (i == 12)
					Size = stream.ReadSignedByte();
				else if (i == 13)
					StandAnimation = stream.ReadShort();
				else if (i == 14)
					WalkAnimation = stream.ReadShort();
				else if (i == 17)
				{
					WalkAnimation = stream.ReadShort();
					Turn180Animation = stream.ReadShort();
					Turn90CWAnimation = stream.ReadShort();
					Turn90CCWAnimation = stream.ReadShort();
				}
				else if (i >= 30 && i < 40)
				{
					if (Actions == null)
						Actions = new String[5];
					Actions[i - 30] = stream.ReadString();
					if (Actions[i - 30].ToLower().Equals("hidden"))
						Actions[i - 30] = null;
				}
				else if (i == 40)
				{
					int k = stream.ReadByte();
					ModelRecolorIndexes = new int[k];
					ModelRecolorColors = new int[k];
					for (int k1 = 0; k1 < k; k1++)
					{
						ModelRecolorIndexes[k1] = stream.ReadShort();
						ModelRecolorColors[k1] = stream.ReadShort();
					}

				}
				else if (i == 60)
				{
					int l = stream.ReadByte();
					FaceModels = new int[l];
					for (int l1 = 0; l1 < l; l1++)
						FaceModels[l1] = stream.ReadShort();

				}
				else if (i == 90)
					stream.ReadShort();
				else if (i == 91)
					stream.ReadShort();
				else if (i == 92)
					stream.ReadShort();
				else if (i == 93)
					ShowMinimapDot = false;
				else if (i == 95)
					CombatLevel = stream.ReadShort();
				else if (i == 97)
					ScaleXZ = stream.ReadShort();
				else if (i == 98)
					ScaleY = stream.ReadShort();
				else if (i == 99)
					HasVisibilityPriority = true;
				else if (i == 100)
					LightIntensity = stream.ReadSignedByte();
				else if (i == 101)
					LightDistance = stream.ReadSignedByte() * 5;
				else if (i == 102)
					HeadIcon = stream.ReadShort();
				else if (i == 103)
					TurnAmount = stream.ReadShort();
				else if (i == 106)
				{
					VarBitIndex = stream.ReadShort();
					if (VarBitIndex == 65535)
						VarBitIndex = -1;
					SettingsID = stream.ReadShort();
					if (SettingsID == 65535)
						SettingsID = -1;
					int i1 = stream.ReadByte();
					SubNPCIDs = new int[i1 + 1];
					for (int i2 = 0; i2 <= i1; i2++)
					{
						SubNPCIDs[i2] = stream.ReadShort();
						if (SubNPCIDs[i2] == 65535)
							SubNPCIDs[i2] = -1;
					}

				}
				else if (i == 107)
					Clickable = false;
			} while (true);
		}

		private NPCDefinition()
		{
			Turn90CCWAnimation = -1;
			VarBitIndex = -1;
			Turn180Animation = -1;
			SettingsID = -1;
			CombatLevel = -1;
			WalkAnimation = -1;
			Size = 1;
			HeadIcon = -1;
			StandAnimation = -1;
			ID = -1L;
			TurnAmount = 32;
			Turn90CWAnimation = -1;
			Clickable = true;
			ScaleY = 128;
			ShowMinimapDot = true;
			ScaleXZ = 128;
			HasVisibilityPriority = false;
		}

		public static Cache LoadedCache { get; set; }

		[Category("Basic"), Description("The ID of the object.")]
		public long ID { get; private set; }
		[Category("Basic"), Description("The name of the object.")]
		public string Name { get; set; }
		[Category("Basic"), Description("The examine text.")]
		public string Description { get; set; }
		[Category("Basic"), Description("The additional right-click options when the item is in an interface.")]
		public string[] Actions { get; set; }
		[Category("Basic"), Description("The combat level.")]
		public int CombatLevel { get; set; }
		[Category("Basic"), Description("The other NPCs rendered in this NPC."), ChangesModel(true)]
		public int[] SubNPCIDs { get; set; }
		[Category("Basic"), Description("The head icon of the NPC.")]
		public int HeadIcon { get; set; }

		[Category("Interaction"), Description("Whether or not the NPC is interactable.")]
		public bool Clickable { get; set; }
		[Category("Interaction"), Description("Whether or not the NPC will be drawn in the first NPC render loop.")]
		public bool HasVisibilityPriority { get; set; }
		[Category("Interaction"), Description("The size of the NPC in tiles.")]
		public sbyte Size { get; set; }
		[Category("Interaction"), Description("Whether or not the NPC shows up on the minimap.")]
		public bool ShowMinimapDot { get; set; }

		[Category("Model"), Description("The indexes of the colors to change in the model."), ChangesModel(true)]
		public int[] ModelRecolorIndexes { get; set; }
		[Category("Model"), Description("The new color values, indexed in accordance to ModelRecolorIndexes."), ChangesModel(true)]
		public int[] ModelRecolorColors { get; set; }
		[Category("Model"), Description("The intensity of the light source."), ChangesModel(true)]
		public sbyte LightIntensity { get; set; }
		[Category("Model"), Description("The  distance the light source is from the model."), ChangesModel(true)]
		public int LightDistance { get; set; }
		[Category("Model"), Description("The other models rendered in this object."), ChangesModel(true)]
		public int[] ModelIndexes { get; set; }
		[Category("Model"), Description("The models that compose the head of the NPC."), ChangesModel(true)]
		public int[] FaceModels { get; set; }

		[Category("Model"), Description("The X and Z scale of the model."), ChangesModel(true)]
		public int ScaleXZ { get; set; }
		[Category("Model"), Description("The Y scale of the model."), ChangesModel(true)]
		public int ScaleY { get; set; }

		[Category("Animations"), Description("The animation for standing.")]
		public int StandAnimation { get; set; }
		[Category("Animations"), Description("The animation for walking.")]
		public int WalkAnimation { get; set; }
		[Category("Animations"), Description("The animation for turning 90 degrees clockwise.")]
		public int Turn90CWAnimation { get; set; }
		[Category("Animations"), Description("The animation for turning 90 degrees counter-clockwise.")]
		public int Turn90CCWAnimation { get; set; }
		[Category("Animations"), Description("The animation for turning around.")]
		public int Turn180Animation { get; set; }
		[Category("Animations"), Description("Unknown exactly, used for turning.")]
		public int TurnAmount { get; set; }

		[ChangesModel(true), Description("\"varbit\" variables index.")]
		public int VarBitIndex { get; set; }
		[ChangesModel(true), Description("Client settings index.")]
		public int SettingsID { get; set; }

		public static int[] streamIndices;

		private static DataBuffer stream;
		private static NPCDefinition[] cache;
		private static int StorageCacheIndex { get; set; }

	}

}
