using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkromPlexer.PacketHandlers;

namespace SkromPlexerPacketGenerator
{
    public class FileCreator
    {
        public string Name;
        public List<PacketCreatorFunction> PCFcts;
        
        public FileCreator(string name)
        {
            Name = name;
            PCFcts = new List<PacketCreatorFunction>();
        }

        public void CreatePacketHandler()
        {
            Directory.CreateDirectory("Output");

            string str = String.Format("public class {0}PacketHandler : APacketHandler\n{{\n{1}\n}}", Name, BuildMethods());

            Console.WriteLine("Writing " + Name + "...");
            File.WriteAllText("Output/" + Name.Replace("PacketCreator", "PacketHandler") + ".cs", str);
        }

        public string BuildMethods()
        {
            string str = "";

            foreach (PacketCreatorFunction fct in PCFcts)
            {
                str += String.Format("\tpublic static List<Packet> {0}(Core core, Client client, Packet packet)\n\t{{{1}\n\t}}\n\n", fct.Packet, GenArgs(fct));
            }

            return (str);
        }

        public string GenArgs(PacketCreatorFunction fct)
        {
            string str = "";

            if (fct.Args == null)
                return ("\n\t\treturn (null);");

            str += "\n\t\tPacketArgs args = packet.GetArgs(new Type[]\n\t\t{\n";

            foreach (Type arg in fct.Args)
            {
                string t = "";

                if (arg == typeof(string))
                    t = "string";
                if (arg == typeof(int))
                    t = "int";
                if (arg == typeof(float))
                    t = "float";
                if (arg == typeof(bool))
                    t = "bool";

                str += "\t\t\ttypeof(" + t + "),\n";
            }
            str += "\t\t});\n\n";
            str += "\t\treturn (null);";

            return (str);
        }
    }
}
