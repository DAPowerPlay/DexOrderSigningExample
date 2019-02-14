namespace NethereumSignerTests
{
    public class Signature
    {
        public Signature(int v, string r, string s)
        {
            this.v = v;
            this.r = r;
            this.s = s;
        }

        public int v { get; set; }
        public string r { get; set; }
        public string s { get; set; }
    }
}
}