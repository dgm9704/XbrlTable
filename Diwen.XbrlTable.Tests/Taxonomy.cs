namespace Diwen.XbrlTable.Tests
{
    using Xunit;
    using XbrlTable;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.IO;

    public class Taxonomy
    {
        string rootFolder = Environment.GetEnvironmentVariable("XBRL_ROOT");

        [Theory]
        [InlineData("http://www.eba.europa.eu/eu/fr/xbrl/crr/fws/fp/gl-2014-04/2015-05-29/mod/fp_ind.xsd")]
        [InlineData("http://eiopa.europa.eu/eu/xbrl/s2md/fws/solvency/solvency2/2016-07-15/mod/adh.xsd")]
        public void ModuleDatapoints(string moduleUrl)
        {
            var datapoints = new List<Tuple<Signature, Address>>();
            var modulePath = moduleUrl.Replace("http:/", rootFolder);

            var tables = Parsing.ParseTables(modulePath);

            foreach (var table in tables)
            {
                datapoints.AddRange(Parsing.ParseDatapoints(table));
            }

            var outpufile = Path.ChangeExtension(modulePath, "datapoints");
            Helper.WriteDatapoints(datapoints, outpufile);
        }
    }
}