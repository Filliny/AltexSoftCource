﻿using System;
using System.IO;
using System.Linq;
using ConsoleApp.InputWorkers;

namespace ConsoleApp.FileHelpers
{
    class FileBrowser : Processor
    {
        public void ShowContent(string path, MyArgs args)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            var root = new DirectoryInfo(path);

            var folders = dir.GetDirectories().OrderBy(x => x.Name).ToArray();
            var files = dir.GetFiles().OrderBy(x => x.Name).ToArray();

            var dirDictionary = folders.ToDictionary(x => Array.IndexOf(folders, x) + "D");
            var fileDictionary = files.ToDictionary(x => Array.IndexOf(files, x) + "F");

            //Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nContent of folder {root.Name} : ");
            Console.WriteLine("ID | NAME");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(" R | ..");

            foreach (var folder in dirDictionary)
            {
                Console.WriteLine(folder.Key + " | " + folder.Value.Name);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;

            foreach (var file in fileDictionary)
            {
                Console.WriteLine(file.Key + " | " + file.Value.Name);
            }

            Console.ResetColor();
            Console.WriteLine("");


            Console.WriteLine("Enter ID of folder to open or \n" +
                              "file to process with last options\n" +
                              "or leave blank to exit:");
            while (true)
            {
                string id = Console.ReadLine();

                if (string.IsNullOrEmpty(id))
                {
                    return;
                }
                else if (id == "R") //go to parent
                {
                    if (dir.Parent != null)
                    {
                        args.DirPath = dir.Parent.FullName;
                        ShowContent(args.DirPath, args);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\nCant go top! Choose another: ");
                    }
                }
                else if (dirDictionary.ContainsKey(id))
                {
                    if (dirDictionary.TryGetValue(id, out DirectoryInfo takeFoldr))
                    {
                        args.DirPath = takeFoldr.FullName;
                        ShowContent(args.DirPath, args);
                    }

                    return;
                }
                else if (fileDictionary.ContainsKey(id))
                {
                    if (fileDictionary.TryGetValue(id, out FileInfo takeFile))
                    {
                        args.FilePath = takeFile.FullName;
                        //args.DirPath = null;
                    }

                    Process(args);
                    return;
                }
                else
                {
                    Console.WriteLine("Wrong ID !");
                }
            }
        }
    }
}