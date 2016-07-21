using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoScript.Core
{
    static class VariableManager
    {
        private static readonly Dictionary<string, string> _variables = new Dictionary<string, string>(); // name, value

        public static void AddVariable(string name, string value)
        {
            _variables.Add(name, value);
        }

        public static string GetValue(string name)
        {
            return _variables[name];
        }

        public static void SetValue(string name, string value)
        {
            _variables[name] = value;
        }

        public static bool Exists(string name)
        {
            return _variables.ContainsKey(name);
        }
    }
}
