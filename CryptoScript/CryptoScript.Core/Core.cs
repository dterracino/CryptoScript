using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryptoScript.Classes;

namespace CryptoScript.Core
{
    public class Script
    {
        public static void Analyze(string line)
        {
            if (line.StartsWith("print ")) // print
            {
                line = line.Replace("print ", "");
                Console.Write(line[0] == '$'
                    ? VariableManager.GetValue(line.Replace("$", ""))
                    : line.Replace("$", ""));
            }
            else if (line.StartsWith("println ")) // print new line
            {
                line = line.Replace("println ", "");
                Console.WriteLine(line[0] == '$'
                    ? VariableManager.GetValue(line.Replace("$", "").Trim())
                    : line.Replace("$", "").Trim());
            }
            else if (line.StartsWith("read")) // user input
            {
                Console.Read();
            }
            else if (line.StartsWith("readln ")) // user input whole line
            {
                line = line.Replace("readln ", "");
                if (line[0] == '$') // everything ok
                {
                    string value = Console.ReadLine();
                    string varName = line.Replace("$", "").Trim();
                    VariableManager.SetValue(varName, value);
                }
                else
                {
                    Exceptions.Print(Exceptions.UserInputOnlyToVar);  // wrong usage
                }
            }
            else if (line.StartsWith("write "))
            {
                line = line.Replace("write ", "");
                if (line[0] == '$') // everything ok
                {
                    string varNameRaw = line.Replace("$", "");
                    int index = varNameRaw.IndexOf(" ", StringComparison.Ordinal);
                    if (index > 0)
                    {
                        string varName = varNameRaw.Substring(0, index);
                        string filepath = varNameRaw.Replace(varName, "").Trim();
                        if (filepath.EndsWith(" -nl"))
                        {
                            filepath = filepath.Replace(" -nl", "");
                            File.WriteAllText(filepath, VariableManager.GetValue(varName) + Environment.NewLine);
                        }
                        else
                        {
                            File.WriteAllText(filepath, VariableManager.GetValue(varName));
                        }
                    }
                    else
                    {
                        Exceptions.Print(Exceptions.AssignmentNeedsEqual);
                    }
                }
                else
                {
                    Exceptions.Print(Exceptions.UserInputOnlyToVar);  // wrong usage
                }
            }
            else if (line.StartsWith("append "))
            {
                line = line.Replace("append ", "");
                if (line[0] == '$') // everything ok
                {
                    string varNameRaw = line.Replace("$", "");
                    int index = varNameRaw.IndexOf(" ", StringComparison.Ordinal);
                    if (index > 0)
                    {
                        string varName = varNameRaw.Substring(0, index);
                        string filepath = varNameRaw.Replace(varName, "").Trim();
                        if (filepath.EndsWith(" -nl"))
                        {
                            filepath = filepath.Replace(" -nl", "");
                            File.AppendAllText(filepath, VariableManager.GetValue(varName) + Environment.NewLine);
                        }
                        else
                        {
                            File.AppendAllText(filepath, VariableManager.GetValue(varName));
                        }
                    }
                    else
                    {
                        Exceptions.Print(Exceptions.AssignmentNeedsEqual);
                    }
                }
                else
                {
                    Exceptions.Print(Exceptions.UserInputOnlyToVar);  // wrong usage
                }
            }
            else if (line.StartsWith("$$")) // assign var with function
            {
                string varName = BetterReplace(line.Remove(line.IndexOf("=", StringComparison.Ordinal) + 1), new[] { "$$", " ", "=" });
                if (BetterReplace(line, new[] { varName, " ", "$$" }).Contains("sha256(")) // sha256 method detected
                {
                    string varNameRaw = line.Substring(line.IndexOf("sha256(", StringComparison.Ordinal));
                    varNameRaw = varNameRaw.Replace("sha256(", "");
                    if (varNameRaw[0] == '$') // value from var
                    {
                        string varNameNew = BetterReplace(varNameRaw, new[] { "$", ")" });
                        string hash = SHA256.GenSHA256(VariableManager.GetValue(varNameNew));
                        VariableManager.SetValue(varName, hash);
                    }
                    else // value preset
                    {
                        string value = varNameRaw.Replace(")", "").Trim();
                        string hash = SHA256.GenSHA256(value);
                        VariableManager.SetValue(varName, hash);
                    }
                }
            }
            else if (line[0] == '$') // add variable
            {
                int index = line.IndexOf('=');
                if (index > 0)
                {
                    string name = BetterReplace(line.Substring(0, index), new[] { "$", " " });
                    string value = BetterReplace(line, new[] { "=", " ", "$", name });
                    VariableManager.AddVariable(name, value);
                }
                else
                {
                    Exceptions.Print(Exceptions.AssignmentNeedsEqual);
                }
            }
        }

        private static string BetterReplace(string text, string[] items)
        {
            foreach (var item in items)
            {
                text = text.Replace(item, "");
            }

            return text;
        }
    }
}
