using System;
using System.Collections.Generic;
using System.Text;

namespace CutMp3.Domain
{
    public interface IConverter
    {
        void ConvertMp4ToMp3(string input, string output);
    }
}
