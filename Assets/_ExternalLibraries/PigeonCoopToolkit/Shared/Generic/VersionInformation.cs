namespace PigeonCoopToolkit.Generic
{
    [System.Serializable]
    public class VersionInformation
    {
        public string Name;
        public int Major = 1;
        public int Minor = 0;
        public int Patch = 0;

        public VersionInformation(string name, int major, int minor, int patch)
        {
            Name = name;
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}.{2}.{3}", Name, Major, Minor, Patch);
        }

        public bool Match(VersionInformation other, bool looseMatch)
        {
            if(looseMatch)
            {
                return other.Name == Name &&
                       other.Major == Major &&
                       other.Minor == Minor;
            }
            else
            {
                return other.Name == Name &&
                       other.Major == Major &&
                       other.Minor == Minor &&
                       other.Patch == Patch;
            }
        }

    }
}
