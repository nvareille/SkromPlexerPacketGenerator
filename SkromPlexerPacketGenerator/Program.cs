using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkromPlexer.PacketHandlers;

namespace SkromPlexerPacketGenerator
{
    class Program
    {
        public static void LoadLocalDLL()
        {
            string[] files = Directory.GetFiles(".");

            foreach (string file in files)
            {
                if (file.Contains(".dll"))
                {
                    Assembly.LoadFrom(file);
                }
            }
        }

        static void Main(string[] args)
        {
            List<FileCreator> files = new List<FileCreator>();

            try
            {

                LoadLocalDLL();
                
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
            catch (Exception e)
            {
                Console.WriteLine("Impossible to generate the files, check the argument");
                Console.WriteLine(e.StackTrace);
                throw e;
            }
        }
    }
}
