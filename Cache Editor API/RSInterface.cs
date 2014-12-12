using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cache_Editor_API.Graphics3D;

namespace Cache_Editor_API
{
	public class RSInterface
	{
		public void swapInventoryItems(int i, int j)
		{
			int k = inv[i];
			inv[i] = inv[j];
			inv[j] = k;
			k = invStackSizes[i];
			invStackSizes[i] = invStackSizes[j];
			invStackSizes[j] = k;
		}

		public static void unpack(DataBuffer stream, RSFont[] textDrawingAreas, SubArchive streamLoader_1)
		{
			int i = -1;
			int j = stream.ReadShort();
			interfaceCache = new RSInterface[j];
			while (stream.Location < stream.Buffer.Length)
			{
				int k = stream.ReadShort();
				if (k == 65535)
				{
					i = stream.ReadShort();
					k = stream.ReadShort();
				}
				RSInterface rsInterface = interfaceCache[k] = new RSInterface();
				rsInterface.id = k;
				rsInterface.parentID = i;
				rsInterface.type = stream.ReadByte();
				rsInterface.atActionType = stream.ReadByte();
				rsInterface.anInt214 = stream.ReadShort();
				rsInterface.width = stream.ReadShort();
				rsInterface.height = stream.ReadShort();
				rsInterface.aByte254 = (byte)stream.ReadByte();
				rsInterface.anInt230 = stream.ReadByte();
				if (rsInterface.anInt230 != 0)
					rsInterface.anInt230 = (rsInterface.anInt230 - 1 << 8) + stream.ReadByte();
				else
					rsInterface.anInt230 = -1;
				int i1 = stream.ReadByte();
				if (i1 > 0)
				{
					rsInterface.anIntArray245 = new int[i1];
					rsInterface.anIntArray212 = new int[i1];
					for (int j1 = 0; j1 < i1; j1++)
					{
						rsInterface.anIntArray245[j1] = stream.ReadByte();
						rsInterface.anIntArray212[j1] = stream.ReadShort();
					}

				}
				int k1 = stream.ReadByte();
				if (k1 > 0)
				{
					rsInterface.valueIndexArray = new int[k1][];
					for (int l1 = 0; l1 < k1; l1++)
					{
						int i3 = stream.ReadShort();
						rsInterface.valueIndexArray[l1] = new int[i3];
						for (int l4 = 0; l4 < i3; l4++)
							rsInterface.valueIndexArray[l1][l4] = stream.ReadShort();

					}

				}
				if (rsInterface.type == 0)
				{
					rsInterface.scrollMax = stream.ReadShort();
					rsInterface.aBoolean266 = stream.ReadByte() == 1;
					int i2 = stream.ReadShort();
					rsInterface.children = new int[i2];
					rsInterface.childX = new int[i2];
					rsInterface.childY = new int[i2];
					for (int j3 = 0; j3 < i2; j3++)
					{
						rsInterface.children[j3] = stream.ReadShort();
						rsInterface.childX[j3] = stream.ReadSignedShort();
						rsInterface.childY[j3] = stream.ReadSignedShort();
					}

				}
				if (rsInterface.type == 1)
				{
					stream.ReadShort();
					stream.ReadByte();
				}
				if (rsInterface.type == 2)
				{
					rsInterface.inv = new int[rsInterface.width * rsInterface.height];
					rsInterface.invStackSizes = new int[rsInterface.width * rsInterface.height];
					rsInterface.aBoolean259 = stream.ReadByte() == 1;
					rsInterface.isInventoryInterface = stream.ReadByte() == 1;
					rsInterface.usableItemInterface = stream.ReadByte() == 1;
					rsInterface.aBoolean235 = stream.ReadByte() == 1;
					rsInterface.invSpritePadX = stream.ReadByte();
					rsInterface.invSpritePadY = stream.ReadByte();
					rsInterface.spritesX = new int[20];
					rsInterface.spritesY = new int[20];
					rsInterface.sprites = new RSImage[20];
					for (int j2 = 0; j2 < 20; j2++)
					{
						int k3 = stream.ReadByte();
						if (k3 == 1)
						{
							rsInterface.spritesX[j2] = stream.ReadSignedShort();
							rsInterface.spritesY[j2] = stream.ReadSignedShort();
							String s1 = stream.ReadString();
							if (streamLoader_1 != null && s1.Length > 0)
							{
								int i5 = s1.LastIndexOf(",");
								rsInterface.sprites[j2] = LoadSprite(int.Parse(s1.Substring(i5 + 1)), streamLoader_1, s1.Substring(0, i5));
							}
						}
					}

					rsInterface.actions = new String[5];
					for (int l3 = 0; l3 < 5; l3++)
					{
						rsInterface.actions[l3] = stream.ReadString();
						if (rsInterface.actions[l3].Length == 0)
							rsInterface.actions[l3] = null;
					}

				}
				if (rsInterface.type == 3)
					rsInterface.aBoolean227 = stream.ReadByte() == 1;
				if (rsInterface.type == 4 || rsInterface.type == 1)
				{
					rsInterface.aBoolean223 = stream.ReadByte() == 1;
					int k2 = stream.ReadByte();
					if (textDrawingAreas != null)
						rsInterface.textDrawingAreas = textDrawingAreas[k2];
					rsInterface.aBoolean268 = stream.ReadByte() == 1;
				}
				if (rsInterface.type == 4)
				{
					rsInterface.message = stream.ReadString();
					rsInterface.aString228 = stream.ReadString();
				}
				if (rsInterface.type == 1 || rsInterface.type == 3 || rsInterface.type == 4)
					rsInterface.textColor = stream.ReadInteger();
				if (rsInterface.type == 3 || rsInterface.type == 4)
				{
					rsInterface.anInt219 = stream.ReadInteger();
					rsInterface.anInt216 = stream.ReadInteger();
					rsInterface.anInt239 = stream.ReadInteger();
				}
				if (rsInterface.type == 5)
				{
					String s = stream.ReadString();
					if (streamLoader_1 != null && s.Length > 0)
					{
						int i4 = s.LastIndexOf(",");
						rsInterface.sprite1 = LoadSprite(int.Parse(s.Substring(i4 + 1)), streamLoader_1, s.Substring(0, i4));
					}
					s = stream.ReadString();
					if (streamLoader_1 != null && s.Length > 0)
					{
						int j4 = s.LastIndexOf(",");
						rsInterface.sprite2 = LoadSprite(int.Parse(s.Substring(j4 + 1)), streamLoader_1, s.Substring(0, j4));
					}
				}
				if (rsInterface.type == 6)
				{
					int l = stream.ReadByte();
					if (l != 0)
					{
						rsInterface.anInt233 = 1;
						rsInterface.mediaID = (l - 1 << 8) + stream.ReadByte();
					}
					l = stream.ReadByte();
					if (l != 0)
					{
						rsInterface.anInt255 = 1;
						rsInterface.anInt256 = (l - 1 << 8) + stream.ReadByte();
					}
					l = stream.ReadByte();
					if (l != 0)
						rsInterface.anInt257 = (l - 1 << 8) + stream.ReadByte();
					else
						rsInterface.anInt257 = -1;
					l = stream.ReadByte();
					if (l != 0)
						rsInterface.anInt258 = (l - 1 << 8) + stream.ReadByte();
					else
						rsInterface.anInt258 = -1;
					rsInterface.anInt269 = stream.ReadShort();
					rsInterface.anInt270 = stream.ReadShort();
					rsInterface.anInt271 = stream.ReadShort();
				}
				if (rsInterface.type == 7)
				{
					rsInterface.inv = new int[rsInterface.width * rsInterface.height];
					rsInterface.invStackSizes = new int[rsInterface.width * rsInterface.height];
					rsInterface.aBoolean223 = stream.ReadByte() == 1;
					int l2 = stream.ReadByte();
					if (textDrawingAreas != null)
						rsInterface.textDrawingAreas = textDrawingAreas[l2];
					rsInterface.aBoolean268 = stream.ReadByte() == 1;
					rsInterface.textColor = stream.ReadInteger();
					rsInterface.invSpritePadX = stream.ReadSignedShort();
					rsInterface.invSpritePadY = stream.ReadSignedShort();
					rsInterface.isInventoryInterface = stream.ReadByte() == 1;
					rsInterface.actions = new String[5];
					for (int k4 = 0; k4 < 5; k4++)
					{
						rsInterface.actions[k4] = stream.ReadString();
						if (rsInterface.actions[k4].Length == 0)
							rsInterface.actions[k4] = null;
					}

				}
				if (rsInterface.atActionType == 2 || rsInterface.type == 2)
				{
					rsInterface.selectedActionName = stream.ReadString();
					rsInterface.spellName = stream.ReadString();
					rsInterface.spellUsableOn = stream.ReadShort();
				}

				if (rsInterface.type == 8)
					rsInterface.message = stream.ReadString();

				if (rsInterface.atActionType == 1 || rsInterface.atActionType == 4 || rsInterface.atActionType == 5 || rsInterface.atActionType == 6)
				{
					rsInterface.tooltip = stream.ReadString();
					if (rsInterface.tooltip.Length == 0)
					{
						if (rsInterface.atActionType == 1)
							rsInterface.tooltip = "Ok";
						if (rsInterface.atActionType == 4)
							rsInterface.tooltip = "Select";
						if (rsInterface.atActionType == 5)
							rsInterface.tooltip = "Select";
						if (rsInterface.atActionType == 6)
							rsInterface.tooltip = "Continue";
					}
				}
			}
		}

		/*private Model method206(int i, int j)
		{
			Model model = (Model)aMRUNodes_264.insertFromCache((i << 16) + j);
			if (model != null)
				return model;
			if (i == 1)
				model = Model.method462(j);
			if (i == 2)
				model = EntityDef.forID(j).method160();
			if (i == 3)
				model = client.myPlayer.method453();
			if (i == 4)
				model = ItemDef.forID(j).method202(50);
			if (i == 5)
				model = null;
			if (model != null)
				aMRUNodes_264.removeFromCache(model, (i << 16) + j);
			return model;
		}*/

		private static RSImage LoadSprite(int i, SubArchive sub_archive, String s)
		{
			//TODO: Caching	
			return new RSImage(sub_archive, s, i);
		}

		/*public static void method208(bool flag, Model model)
		{
			int i = 0;//was parameter
			int j = 5;//was parameter
			if (flag)
				return;
			aMRUNodes_264.unlinkAll();
			if (model != null && j != 4)
				aMRUNodes_264.removeFromCache(model, (j << 16) + i);
		}*/

		/*public Model method209(int j, int k, bool flag)
		{
			Model model;
			if (flag)
				model = method206(anInt255, anInt256);
			else
				model = method206(anInt233, mediaID);
			if (model == null)
				return null;
			if (k == -1 && j == -1 && model.anIntArray1640 == null)
				return model;
			Model model_1 = new Model(true, Class36.method532(k) & Class36.method532(j), false, model);
			if (k != -1 || j != -1)
				model_1.method469();
			if (k != -1)
				model_1.method470(k);
			if (j != -1)
				model_1.method470(j);
			model_1.method479(64, 768, -50, -10, -50, true);
			return model_1;
		}*/

		public RSInterface()
		{
		}

		public RSImage sprite1;
		public int anInt208;
		public RSImage[] sprites;
		public static RSInterface[] interfaceCache;
		public int[] anIntArray212;
		public int anInt214;
		public int[] spritesX;
		public int anInt216;
		public int atActionType;
		public String spellName;
		public int anInt219;
		public int width;
		public String tooltip;
		public String selectedActionName;
		public bool aBoolean223;
		public int scrollPosition;
		public String[] actions;
		public int[][] valueIndexArray;
		public bool aBoolean227;
		public String aString228;
		public int anInt230;
		public int invSpritePadX;
		public int textColor;
		public int anInt233;
		public int mediaID;
		public bool aBoolean235;
		public int parentID;
		public int spellUsableOn;
		public int anInt239;
		public int[] children;
		public int[] childX;
		public bool usableItemInterface;
		public RSFont textDrawingAreas;
		public int invSpritePadY;
		public int[] anIntArray245;
		public int anInt246;
		public int[] spritesY;
		public String message;
		public bool isInventoryInterface;
		public int id;
		public int[] invStackSizes;
		public int[] inv;
		public byte aByte254;
		private int anInt255;
		private int anInt256;
		public int anInt257;
		public int anInt258;
		public bool aBoolean259;
		public RSImage sprite2;
		public int scrollMax;
		public int type;
		public int anInt263;
		public int anInt265;
		public bool aBoolean266;
		public int height;
		public bool aBoolean268;
		public int anInt269;
		public int anInt270;
		public int anInt271;
		public int[] childY;

	}
}
