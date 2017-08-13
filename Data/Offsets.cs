namespace MHXXSaveEditor.Data
{
    class Offsets
    {
        //Header Data
        public const int FIRST_CHAR_SLOT_USED = 0x04; //Size 1
        public const int SECOND_CHAR_SLOT_USED = 0x05; //Size 1
        public const int THIRD_CHAR_SLOT_USED = 0x06; //Size 1
        public const int FIRST_CHARACTER_OFFSET = 0x10; //Size 4
        public const int SECOND_CHARACTER_OFFSET = 0x14; //Size 4
        public const int THIRD_CHARACTER_OFFSET = 0x18; //Size 4

        //Character Offsets [CHARACTER BASE +  CHARACTER OFFSET]
        public const int NAME_OFFSET = 0x0; //Size 32
        public const int PLAY_TIME_OFFSET = 0x20; //Size 4
        public const int FUNDS_OFFSET = 0x24; //Size 4 ;
        public const int HUNTER_RANK_OFFSET = 0x28; //Size 2 
        public const int HUNTER_GENDER_OFFSET = 0x2A;
        //public const int CHARACTER_HUNTING_STYLE_OFFSET = 0x0261; //Size 1
        //public const int HUNTER_ART_1_OFFSET = 0x2C; //Size 2
        //public const int HUNTER_ART_2_OFFSET = 0x2E; //Size 2
        //public const int HUNTER_ART_3_OFFSET = 0X30; //Size 2
        //public const int EQUIPPED_WEAPON_OFFSET = 0x010C; //Size 48
        //public const int EQUIPPED_HEAD_OFFSET = 0x013C; //Size 48
        //public const int EQUIPPED_CHEST_OFFSET = 0x016C; //Size 48
        //public const int EQUIPPED_ARMS_OFFSET = 0x019c; //Size 48
        //public const int EQUIPPED_WAIST_OFFSET = 0x01CC; //Size 48
        //public const int EQUIPPED_LEG_OFFSET = 0x01FC; //Size 48
        //public const int EQUIPPED_TALISMAN_OFFSET = 0x022C; //Size 48
        //public const int WEAPON_TYPE_OFFSET = 0x025C; //Size 1
        public const int CHARACTER_VOICE_OFFSET = 0x241; //Size 1
        public const int CHARACTER_EYE_COLOR_OFFSET = 0x242; //Size 1
        public const int CHARACTER_CLOTHING_OFFSET = 0x243; //Size 1
        public const int CHARACTER_GENDER_OFFSET = 0x244;  //Size 1
        public const int CHARACTER_HAIRSTYLE_OFFSET = 0x246; //Size 1
        public const int CHARACTER_FACE_OFFSET = 0x247; //Size 1
        public const int CHARACTER_FEATURES_OFFSET = 0x248; //Size 1
        public const int CHARACTER_SKIN_COLOR_OFFSET = 0x260; //Size 4 
        public const int CHARACTER_HAIR_COLOR_OFFSET = 0x264; //Size 4
        public const int CHARACTER_FEATURES_COLOR_OFFSET = 0x268; //Size 4
        public const int CHARACTER_CLOTHING_COLOR_OFFSET = 0x26C; //Size 4
        //public const int CHEST_ARMOR_PIGMENT_OFFSET = 0x0268; //Size 4
        //public const int ARMS_ARMOR_PIGMENT_OFFSET = 0x026C; //Size 4 
        //public const int WAIST_ARMOR_PIGMENT_OFFSET = 0x0270;  //Size 4
        //public const int LEG_ARMOR_PIGMENT_OFFSET = 0x0274; //Size 4
        //public const int HEAD_ARMOR_PIGMENT_OFFSET = 0x0278; //Size 4
        //public const int CHARACTER_SKIN_COLOR_OFFSET = 0x027C; //Size 4 ;This only used in loading screen

        // Items & Equips
        public const int ITEM_BOX_OFFSET = 0x0278; //Size 5463 (2300 of them each 19 bits long)
        public const int EQUIPMENT_BOX_OFFSET = 0x62EE; //Size 72000 (2000 of them each 36 bytes long)
        //public const int ITEM_SET_OFFSET = 0x0EDE; //Size 1360 (8 of them each 170 bytes long)
        //public const int POUCH_OFFSET = 0x142E; //Size 72 (32 Items each 18 bits long)
        //public const int PALICO_OFFSET = 0x019426; //Size 19140 (60 of them each 319 bytes long)
        //public const int PALICO_EQUIPMENT_OFFSET = 0x10B47; //Size 25200 (700 of them 36 bytes long)

        // Secondary offsets ??
        public const int NAME_OFFSET2 = 0x23B7E; // size 4
        public const int FUNDS_OFFSET2 = 0x280F; // size 4
        public const int PLAY_TIME_OFFSET2 = 0x2248B; //Size 4
        public const int CHARACTER_VOICE_OFFSET2 = 0x23B48; //Size 1
        public const int CHARACTER_EYE_COLOR_OFFSET2 = 0x23B49; //Size 1
        public const int CHARACTER_CLOTHING_OFFSET2 = 0x23B4A; //Size 1
        public const int CHARACTER_GENDER_OFFSET2 = 0x23B4B; // Size 1
        public const int CHARACTER_HAIRSTYLE_OFFSET2 = 0x23B4C; //Size 1
        public const int CHARACTER_FACE_OFFSET2 = 0x23B4D; //Size 1
        public const int CHARACTER_FEATURES_OFFSET2 = 0x23B4E; //Size 1

        public const int CHARACTER_VOICE_OFFSET3 = 0XC71D7; //Size 1
        public const int CHARACTER_EYE_COLOR_OFFSET3 = 0XC71D8; //Size 1
        public const int CHARACTER_CLOTHING_OFFSET3 = 0XC71D9; //Size 1
        public const int CHARACTER_GENDER_OFFSET3 = 0xC71DA; // Size 1
        public const int CHARACTER_HAIRSTYLE_OFFSET3 = 0xC71DB; //Size 1
        public const int CHARACTER_FACE_OFFSET3 = 0xC71DC; //Size 1
        public const int CHARACTER_FEATURES_OFFSET3 = 0xC71DD; //Size 1

        public const int CHARACTER_SKIN_COLOR_OFFSET2 = 0x23B67; //Size 4 
        public const int CHARACTER_HAIR_COLOR_OFFSET2 = 0X23B6B; //Size 4
        public const int CHARACTER_FEATURES_COLOR_OFFSET2 = 0x23B6F; //Size 4
        public const int CHARACTER_CLOTHING_COLOR_OFFSET2 = 0X23B73; //Size 4
        public const int CHARACTER_SKIN_COLOR_OFFSET3 = 0XC71F5; //Size 4
        public const int CHARACTER_HAIR_COLOR_OFFSET3 = 0XC71F9; //Size 4
        public const int CHARACTER_FEATURES_COLOR_OFFSET3 = 0XC71FD; //Size 4
        public const int CHARACTER_CLOTHING_COLOR_OFFSET3 = 0XC7201; //Size 4

        // Points
        public const int HR_POINTS_OFFSET = 0x280B; //Size 4
        public const int ACADEMY_POINTS_OFFSET = 0x2817; //Size 4
        public const int BHERNA_POINTS_OFFSET = 0x281B; //Size 4
        public const int KOKOTO_POINTS_OFFSET = 0x281F; //Size 4
        public const int POKKE_POINTS_OFFSET = 0x2823; //Size 4
        public const int YUKUMO_POINTS_OFFSET = 0x2827; //Size 4

        //public const int SHOP_OFFSETS = 0x1D76;
        //public const int CRAFTABLE_WEAPONS_OFFSET = 0x20BE;
        //public const int CRAFTABLE_ARMOR_SHOP_OFFSET = 0x2316;
        //public const int CRAFTABLE_PALICO_GEAR_OFFSET = 0x02ABE;

        //public const int FOOD_FLAG_OFFSETS = 0x1A32; //Size 4
        //public const int AWARD_FLAG_OFFSETS = 0x1B8A; //Size 13

        //public const int PLAYER_GUILD_CARD_OFFSET = 0x09F0FE; //Size 5208

        //public const int UNLOCKED_BOXES_OFFSET = 0x1A22; //Size 8

        //public const int MONSTERHUNT_OFFSETS = 0x42E7; //71 Monsters 2 bytes each
        //public const int MONSTERCAPTURE_OFFSETS = 0x43C7; //71 Monsters 2 bytes each

        //public const int SHOUTOUT_OFFSETS = 0xEAD6E;
        //public const int AUTOMATIC_SHOUTOUT_OFFSETS = 0xEB72E;
    }
}
