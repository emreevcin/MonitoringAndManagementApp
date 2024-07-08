using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsApplication
{
    public class IntegerValidator : IValidator<string>
    {
        public bool Validate(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }
    }
}
