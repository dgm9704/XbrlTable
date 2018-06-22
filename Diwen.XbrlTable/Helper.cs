namespace Diwen.XbrlTable
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Helper
    {
        public static void DumpDatapoints(Table table)
        {
            Console.WriteLine($"{table.Code} datapoints");
            var datapoints = Parsing.ParseDatapoints(table);
            Console.WriteLine(datapoints.Select(p => $"{p.Item1}->{p.Item2}").Join("\n"));
        }

        public static void DumpTable(Table table)
        {
            Console.WriteLine(table.Code);

            foreach (var axis in table.Axes.Where(a => a.Direction == Direction.Z).Where(a => !a.IsOpen))
            {
                Console.Write($"{axis.Direction}\t");
                Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Signature}").Join("\t"));
            }

            foreach (var axis in table.Axes.Where(a => a.IsOpen))
            {
                Console.Write($"{axis.Direction}\t");
                Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Signature}").Join("\t")); ;
            }

            Console.Write("Y \\ X\t");

            var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
            var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
            Console.WriteLine(xOrdinates.Select(o => o.Code.PadRight(10)).Join("\t"));

            var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

            var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

            if (!yOrdinates.Any())
            {
                yOrdinates = new List<Ordinate>() { new Ordinate("999", "0", new Signature()) };

            }

            foreach (var y in yOrdinates)
            {
                Console.Write($"{y.Code}\t");
                Console.Write(xOrdinates.Select(x => ".".PadRight(10)).Join("\t"));
                Console.WriteLine($"{y.Signature}");
            }

            var max = xOrdinates.Select(o => o.Signature.Count).Max();
            for (int i = 0; i < max; i++)
            {
                Console.Write("\t");
                foreach (var x in xOrdinates)
                {
                    var e = x.Signature.ElementAtOrDefault(i);
                    if (!string.IsNullOrEmpty(e.Key))
                    {
                        if (e.Key == "met")
                        {
                            Console.Write($"{e.Value.Split(':').Last()}".PadRight(10));
                        }
                        else
                        {
                            Console.Write($"{e.Key.Split(':').Last()}/{e.Value}".PadRight(10));
                        }
                    }
                    else
                    {
                        Console.Write(new string(' ', 10));
                    }

                    Console.Write("\t");
                }
                Console.WriteLine();
            }
        }

        public static void WriteDatapoints(List<Tuple<Signature, Address>> datapoints, string outpufile)
        {
            File.WriteAllLines(outpufile, datapoints.Select(d => $"{d.Item1}->{d.Item2}"));
        }

        public static void DumpHypercubes(IEnumerable<Hypercube> cubes)
        {
            foreach (var cube in cubes)
            {
                Console.WriteLine(cube);
            }
        }

        public static void DumpAxesAndMetrics(Table table)
        {
            Console.WriteLine(table.Code);

            foreach (var axis in table.Axes.Where(a => a.Direction == Direction.Z).Where(a => !a.IsOpen))
            {
                Console.Write($"{axis.Direction}\t");
                Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
            }

            foreach (var axis in table.Axes.Where(a => a.IsOpen))
            {
                Console.Write($"{axis.Direction}\t");
                Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Signature["met"].Split(':').Last()}").Join("\t")); ;
            }

            Console.Write("Y \\ X\t");

            var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
            var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
            Console.WriteLine(xOrdinates.Select(o => o.Code).Join("\t"));

            var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

            var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

            if (!yOrdinates.Any())
            {
                yOrdinates = new List<Ordinate>() { new Ordinate("999", "0", new Signature()) };
            }

            foreach (var y in yOrdinates)
            {
                Console.Write($"{y.Code}\t");
                Console.WriteLine(xOrdinates.Select(x => $"{x.Signature["met"].Split(':').Last()}{y.Signature["met"].Split(':').Last()}").Join("\t"));
            }
        }

        public static void DumpAxes(Table table)
        {
            Console.WriteLine(table.Code);

            foreach (var axis in table.Axes.OrderBy(a => a.Order))
            {
                Console.Write($"{axis.Direction}\t");
                Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
            }
        }

        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

        public static void DumpAll(string taxonomyPath, string metricPath, string dimensionPath, string domainPath, string tableCode)
        {
            var table = Parsing.ParseTable(taxonomyPath, tableCode);

            var metFile = $"{metricPath}/met.xsd";
            var metrics = Parsing.ParseNames(metFile);

            var dimFile = $"{dimensionPath}/dim.xsd";
            var dimensions = Parsing.ParseNames(dimFile);
            var typedDimensions = Parsing.ParseTypedDimensions(dimFile);

            var expFile = $"{domainPath}/exp.xsd";
            var typFile = $"{domainPath}/typ.xsd";
            var domains = Parsing.ParseNames(expFile);
            var typDomains = Parsing.ParseNames(typFile);

            typDomains.ToList().ForEach(x => domains[x.Key] = x.Value);
            var cubes = Parsing.ParseHypercubes(taxonomyPath, tableCode, metrics, dimensions, domains, typedDimensions);

            DumpAxes(table);
            DumpTable(table);
            DumpDatapoints(table);
            DumpHypercubes(cubes);

        }
    }
}