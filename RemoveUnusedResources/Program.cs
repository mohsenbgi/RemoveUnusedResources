Console.WriteLine("Enter project full path:");
var projectPath = Console.ReadLine().ToString();

Console.WriteLine("Enter destination file full path:");
var destinationFilePath = Console.ReadLine().ToString();

var directories1 = GetDirectories(projectPath);

var Files = new List<string>();

foreach (var d in directories1)
{
    Files.AddRange(Directory.GetFiles(d).Where(x => x.EndsWith(".cs") || x.EndsWith(".cshtml") || x.EndsWith(".js")));
}

List<string> GetDirectories(string path)
{

    var directories1 = new List<string>();

    directories1.Add(path);

    var newDiectory = new List<string>();
    newDiectory.AddRange(directories1);

    var tempDirectory = new List<string>();

    while (true)
    {
        foreach (var item in newDiectory)
        {
            tempDirectory.AddRange(Directory.GetDirectories(item));
        }

        if (tempDirectory.Count == 0) break;

        newDiectory.Clear();
        newDiectory.AddRange(tempDirectory);
        directories1.AddRange(tempDirectory);
        tempDirectory.Clear();
    }

    return directories1;
}

var allNames = new List<string>();

foreach (var file in Files)
{
    var lines = File.ReadAllLines(file).ToList();

    foreach (var prevLine in lines)
    {
        var line = prevLine.Trim();
        while (true)
        {
            var findIndex = 0;

            if (line.Replace(" ", "").Contains("[Display(Name="))
            {
                var name = line.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("[Display(Name=") + "[Display(Name=".Length;
            }

            else if (line.Contains("Localizer[\""))
            {
                var slicedLine = line.Substring(line.IndexOf("Localizer[\"")).ToString();
                var name = slicedLine.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("Localizer[\"") + "Localizer[\"".Length;
            }

            else if (line.Contains("localizer[\""))
            {
                var slicedLine = line.Substring(line.IndexOf("localizer[\"")).ToString();
                var name = slicedLine.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("localizer[\"") + "localizer[\"".Length;
            }

            else if (line.Contains("getResource(\""))
            {
                var slicedLine = line.Substring(line.IndexOf("getResource(\"")).ToString();
                var name = slicedLine.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("getResource(\"") + "getResource(\"".Length;
            }

            else if (line.Contains("jsLocalizer(\""))
            {
                var slicedLine = line.Substring(line.IndexOf("jsLocalizer(\"")).ToString();
                var name = slicedLine.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("jsLocalizer(\"") + "jsLocalizer(\"".Length;
            }

            else if (line.Contains("[Permission(\""))
            {
                var name = line.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("[Permission(\"") + "[Permission(\"".Length;
            }

            else if (line.Contains("\"Error."))
            {
                var name = line.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("\"Error.") + "\"Error.".Length;
            }

            else if (line.Contains("\"Success."))
            {
                var name = line.Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("\"Success.") + "\"Success.".Length;
            }

            else if (file.Contains("PermissionsList.cs") && line.Contains("UniqueName") && line.Contains("="))
            {
                var name = line.Replace(" ", "").Split("\"")[1];
                if (!allNames.Contains(name))
                {
                    allNames.Add(name);
                }

                findIndex = line.IndexOf("UniqueName") + "UniqueName".Length;
            }

            else
            {
                break;
            }

            line = line.Substring(findIndex > 0 ? findIndex : 0).ToString();
        }
    }
}

var destinationFile = File.ReadAllLines(destinationFilePath).ToList();

var namesInDestinationFile = new List<string>();

foreach (var line in destinationFile)
{
    if (line.Contains("<data"))
    {
        var name = line.Split("\"")[1];
        if (!namesInDestinationFile.Contains(name))
        {
            namesInDestinationFile.Add(name);
        }
    }
}

var unusedNames = new List<string>();

foreach (var name in namesInDestinationFile)
{
    if (!allNames.Contains(name))
    {
        unusedNames.Add(name);
    }
}

// remove unused names from destination file
foreach (var name in unusedNames)
{
    var index = destinationFile.IndexOf(destinationFile.Where(x => x.Contains($"\"{name}\"")).FirstOrDefault());

    if (index != -1)
    {
        destinationFile.RemoveAt(index);
        destinationFile.RemoveAt(index);
        destinationFile.RemoveAt(index);
    }
}

File.WriteAllLines(destinationFilePath, destinationFile);


