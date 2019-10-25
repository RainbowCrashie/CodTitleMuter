using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeactivisionInfiniteWordTrayarko
{
    public class Settings
    {
        public int[] SecondsAfterOptions { get; set; }
        public UnmuteSetting UnmuteOn { get; set; }

        public Settings()
        {
            SecondsAfterOptions = new int[] { 60, 45, 30, 20, 10 };
        }
    }


    public enum UnmuteSetting
    {
        Delayed,
        Shortcut
    }
}
