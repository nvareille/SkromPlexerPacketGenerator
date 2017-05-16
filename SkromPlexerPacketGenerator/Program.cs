using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkromPlexer.PacketHandlers;

namespace SkromPlexerPacketGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FileCreator> files = new List<FileCreator>();

            try
            {
                Assembly assembly = Assembly.LoadFrom(args[0]);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<PacketCreator>() != null)
                    {
                        FileCreator fc = new FileCreator(type.Name);

                        files.Add(fc);
                        Console.WriteLine("Extracting " + type.Name + ":\n");
                        foreach (var method in type.GetMethods())
                        {
                            PacketCreatorFunction fct = method.GetCustomAttribute<PacketCreatorFunction>();

                            if (fct != null)
                            {
                                fc.PCFcts.Add(fct);
                                Console.WriteLine("\t" + fct.Packet);
                            }
                        }

                        Console.WriteLine();
                    }
                }

                foreach (FileCreator file in files)
                {
                    file.CreatePacketHandler();
                }

                Console.Write("Done !");
            }
            catch (Exception)
            {
                Console.WriteLine("Impossible to generate the files, check the argument");
                
            }
        }
    }
}
