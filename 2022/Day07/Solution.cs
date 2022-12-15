namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]      
class Solution : Solver {

    public object PartOne(string input)
    {
        const int limit = 100000;
        var actual = SolveDayOne(TestCase, limit);
        var expected = 95437;
        Console.WriteLine($"Part 1 test case expected {expected} actual was {actual} which is {expected == actual}");

        return SolveDayOne(input, limit);
    }

    public object PartTwo(string input) {
        const int limit = 8381165;
        var actual = SolveDayTwo(TestCase, limit);
        var expected = 24933642;
        Console.WriteLine($"Part 2 test case expected {expected} actual was {actual} which is {expected == actual}");

        return SolveDayTwo(input, limit);
    }

    private int SolveDayOne(string input, int limit)
    {
        var q = new Queue<string>();
        input.Split('\n').Skip(1).ToList().ForEach(q.Enqueue);
        var dirStructure = CreateStructure("/", ref q);
        var size = SearchStructureForMaxSize(dirStructure, limit);
        return size;
    }

    private int SolveDayTwo(string input, int limit)
    {
        const int totalDiskSpace = 70000000;
        const int targetFreeSpace = 30000000;
        var q = new Queue<string>();
        input.Split('\n').Skip(1).ToList().ForEach(q.Enqueue);
        var dirStructure = CreateStructure("/", ref q);
        var dirs = FlattenDirectoryStructure(dirStructure).ToList();
        var root = dirs.Single(x => x.Name == "/");
        var diskSpaceUsed = root.TotalSize;
        var currentFreeSpace = totalDiskSpace - diskSpaceUsed;
        var amountToFree = targetFreeSpace - currentFreeSpace;

        //var recursiveResult = SearchStructureSmallestOverSize(dirStructure, amountToFree);
        //Console.WriteLine($"Recursive solution was {recursiveResult}");

        var filtered = dirs.Where(d => d.TotalSize >= amountToFree).OrderBy(d => d.TotalSize).ToList();
        //dirs.ToList().ForEach(x => Console.WriteLine($"{x.TotalSize} {x.Name}"));
        //filtered.ToList().ForEach(x => Console.WriteLine($"{x.TotalSize} {x.Name}"));
        return filtered.First().TotalSize;
    }

    record Dir(string Name, List<Dir> SubDirectories, int FileSize, int TotalSize);

    Dir CreateStructure(string dirName, ref Queue<string> q)
    {
        //Console.WriteLine($"Dir: {dirName} len(q)={q.Count}");
        var subDirectories = new List<Dir>();
        var fileSize = 0;
        var totalSize = 0;
        bool isDone = false;
        while (q.Count > 0 && !isDone)
        {
            var line = q.Dequeue();
            var firstChar = line[0];
            //Console.WriteLine($"char:{firstChar} line:{line}");
            switch (firstChar)
            {
                case '$':
                    var commandParts = line.Split(' ');
                    if (commandParts.Length > 2) // ignore ls command
                    {
                        var cdDestination = commandParts[2];
                        if (cdDestination == "..")
                        {
                            isDone = true;
                        }
                        else
                        {
                            var subDir = CreateStructure(cdDestination, ref q);
                            totalSize += subDir.TotalSize;
                            subDirectories.Add(subDir);
                        }
                    }
                    break;
                case 'd':
                    // ignore since we don't care about directories we never cd into
                    break;
                default:
                    // this is a file size
                    var fileParts = line.Split(' ');
                    fileSize += int.Parse(fileParts[0]);
                    break;
            }
        }

        totalSize += fileSize;
        //Console.WriteLine($"Dir {dirName} Total: {totalSize} files: {fileSize}");
        return new Dir(dirName, subDirectories, fileSize, totalSize);
    }

    int SearchStructureForMaxSize(Dir dir, int maxSize)
    {
        //Console.WriteLine($"SearchStructureForMaxSize: {dir.Name} size {dir.TotalSize} match {dir.TotalSize <= maxSize}");
        return dir.SubDirectories.Sum(d => SearchStructureForMaxSize(d, maxSize)) 
               + ((dir.TotalSize <= maxSize) ? dir.TotalSize : 0);
    }

    IEnumerable<Dir> FlattenDirectoryStructure(Dir dir)
    {
        return dir.SubDirectories
            .Select(FlattenDirectoryStructure) // recursively call ourselves on all our subdirectories
            .SelectMany(dirs => dirs) // flatten the nested directory lists
            .Append(dir); // don't forget to add ourselves to the list.
    }

    /* Initial recursive solution which also works but I like the FlattenDirectoryStructure better */
    int SearchStructureSmallestOverSize(Dir dir, int minSize)
    {
        //Console.WriteLine($"SearchStructureSmallestOverSize: {dir.Name} size {dir.TotalSize} target {minSize} match {dir.TotalSize >= minSize}");
        var items = dir.SubDirectories.Select(d => SearchStructureSmallestOverSize(d, minSize))
            .ToList()
            .Append(dir.TotalSize >= minSize ? dir.TotalSize : 0)
            .Where(i => i > 0)
            .ToList();
        return items.Count > 0 ? items.Min() : 0;
    }

    const string TestCase =
@"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

}
