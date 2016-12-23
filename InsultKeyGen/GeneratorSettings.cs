namespace InsultKeyGen
{
    public class GeneratorSettings
    {
        private int blockcount;

        public int Blockcount
        {
            get { return blockcount; }
            set { blockcount = value; }
        }

        private int blocklength;

        public int Blocklength
        {
            get { return blocklength; }
            set { blocklength = value; }
        }

        private Separator separator;

        public Separator Separator
        {
            get { return separator; }
            set { separator = value; }
        }
    }
}