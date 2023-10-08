using System.Text;
using JetBrains.Annotations;

namespace TitularRoyalty
{
    public readonly struct TRVersion
    {
        public readonly int major;
        public readonly int minor;
        public readonly int patch;
        public readonly string label;
        
        public TRVersion(int major, int minor, int patch = 0, string label = "")
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
            this.label = label;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(major);
            sb.Append('.');
            sb.Append(minor);
            
            if (patch != 0)
            {
                sb.Append('.');
                sb.Append(patch);
            }
            
            if (label.Length > 0)
            {
                sb.Append('-');
                sb.Append(label);
            }
            else
            {
#if DEBUG
                sb.Append("-dev");
#endif
            }

            
            return sb.ToString();
        }
    }
}