namespace MHXXSaveEditor.Data
{
    public class Player
    {
        public int SaveOffset { get; set; }

        //General Info
        public string Name { get; set; }
        public int PlayTime { get; set; }
        public int Funds { get; set; }
        public int HunterRank { get; set; }
        //public int HunterArt1 { get; set; }
        //public int HunterArt2 { get; set; }
        //public int HunterArt3 { get; set; }

        //Character Details
        public byte Voice { get; set; }
        public byte EyeColor { get; set; }
        public byte Clothing { get; set; }
        public byte Gender { get; set; } //TWO GENDERS Male = 0; Female = 1;
        //public byte HuntingStyle { get; set; }
        public byte HairStyle { get; set; }
        public byte Face { get; set; }
        public byte Features { get; set; }

        //Character Colors
        public byte[] SkinColorRGBA { get; set; }
        public byte[] HairColorRGBA { get; set; }
        public byte[] FeaturesColorRGBA { get; set; }
        public byte[] ClothingColorRGBA { get; set; }

        //Points
        public int HRPoints { get; set; }
        public int AcademyPoints { get; set; }
        public int BhernaPoints { get; set; }
        public int KokotoPoints { get; set; }
        public int PokkePoints { get; set; }
        public int YukumoPoints { get; set; }

        //Item Box
        public string[] itemId { get; set; }
        public string[] itemCount { get; set; }

        //Equipment Box
        public byte[] equipmentInfo { get; set; }
    }
}
