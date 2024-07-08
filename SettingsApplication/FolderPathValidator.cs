using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsApplication
{
    public class FolderPathValidator : IValidator<string>
    {
        public bool Validate(string path)
        {
            try
            {
                System.IO.Path.GetFullPath(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
