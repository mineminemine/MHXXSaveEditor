using System;
using System.Linq;
using System.Text;
using MHXXSaveEditor.Data;

namespace MHXXSaveEditor.Util
{
    class DataExtractor
    {
        public void getInfo(byte[] saveFile, int slot, Player player)
        {
            byte[] charNameByte = new byte[32];
            byte[] playTimeByte = new byte[4];
            byte[] fundsByte = new byte[4];
            byte[] rankByte = new byte[2];
            byte[] hrPointsByte = new byte[4];
            byte[] acaPointsByte = new byte[4];
            byte[] skinRGBA = new byte[4];
            byte[] hairRGBA = new byte[4];
            byte[] featuresRGBA = new byte[4];
            byte[] clothingRGBA = new byte[4];
            byte[] itemBytes = new byte[5463];
            string[] itemId = new string[2300];
            string[] itemCount = new string[2300];
            string[] item = new string[] { };

            if (slot == 1)
            {
                string firstSlot = saveFile[0x13].ToString("X2") + saveFile[0x12].ToString("X2") + saveFile[0x11].ToString("X2") + saveFile[0x10].ToString("X2");
                player.SaveOffset = Int32.Parse(firstSlot, System.Globalization.NumberStyles.HexNumber);
            }
            else if (slot == 2)
            {
                string secondSlot = saveFile[0x17].ToString("X2") + saveFile[0x16].ToString("X2") + saveFile[0x15].ToString("X2") + saveFile[0x14].ToString("X2");
                player.SaveOffset = Int32.Parse(secondSlot, System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                string thirdSlot = saveFile[0x21].ToString("X2") + saveFile[0x20].ToString("X2") + saveFile[0x19].ToString("X2") + saveFile[0x18].ToString("X2");
                player.SaveOffset = Int32.Parse(thirdSlot, System.Globalization.NumberStyles.HexNumber);
            }

            // Character Name
            Array.Copy(saveFile, player.SaveOffset, charNameByte, 0, Constants.SIZEOF_NAME); // copies from savefile to buffer
            player.Name = Encoding.UTF8.GetString(charNameByte);

            // Play Time
            Array.Copy(saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET, playTimeByte, 0, 4);
            player.PlayTime = BitConverter.ToInt32(playTimeByte, 0);

            // Funds
            Array.Copy(saveFile, player.SaveOffset + Offsets.FUNDS_OFFSET, fundsByte, 0, 4);
            player.Funds = BitConverter.ToInt32(fundsByte, 0);

            // Hunter Rank
            Array.Copy(saveFile, player.SaveOffset + Offsets.HUNTER_RANK_OFFSET, rankByte, 0, 2);
            player.HunterRank = BitConverter.ToInt16(rankByte, 0);

            // Hunter Rank Points
            Array.Copy(saveFile, player.SaveOffset + Offsets.HR_POINTS_OFFSET, hrPointsByte, 0, 4);
            player.HRPoints = BitConverter.ToInt32(hrPointsByte, 0);

            // Academy Points
            Array.Copy(saveFile, player.SaveOffset + Offsets.ACADEMY_POINTS_OFFSET, acaPointsByte, 0, 4);
            player.AcademyPoints = BitConverter.ToInt32(acaPointsByte, 0);

            // Character Info
            player.Voice = saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET];
            player.EyeColor = saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET];
            player.Clothing = saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET];
            player.Gender = saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET];
            player.HairStyle = saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET];
            player.Face = saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET];
            player.Features = saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET];

            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET, skinRGBA, 0, 4);
            player.SkinColorRGBA = skinRGBA;
            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET, hairRGBA, 0, 4);
            player.HairColorRGBA = hairRGBA;
            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET, featuresRGBA, 0, 4);
            player.FeaturesColorRGBA = featuresRGBA;
            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET, clothingRGBA, 0, 4);
            player.ClothingColorRGBA = clothingRGBA;

            // Items
            Array.Copy(saveFile, player.SaveOffset + Offsets.ITEM_BOX_OFFSET, itemBytes, 0, Constants.SIZEOF_ITEMBOX);
            Array.Reverse(itemBytes);
            var result = string.Concat(itemBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            result = result.Substring(4,result.Length-4); // To remove the unnecessary/extra '0000'
            for (int a = 2299; a >= 0; a--)
            {
                itemCount[a] = Convert.ToInt32(result.Substring(0, 7), 2).ToString();
                itemId[a] = Convert.ToInt32(result.Substring(7, 12), 2).ToString();
                result = result.Substring(19, result.Length - 19);
            }
            player.itemCount = itemCount;
            player.itemId = itemId;
        }
    }
}
