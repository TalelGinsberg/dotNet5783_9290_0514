﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalApi;
using System.Xml.Linq;


static class DalConfig
{
    internal static string? s_dalName;
    internal static Dictionary<string, string> s_dalPackages;

    static DalConfig()
    {
        XElement dalConfig = XElement.Load(@"xml\dal-config.xml") // xml information
            ?? throw new DO.DalConfigException("dal-config.xml file is not found");
        s_dalName = dalConfig?.Element("dal")?.Value // elemnt object with 'dal'
            ?? throw new DO.DalConfigException("<dal> element is missing");
        var packages = dalConfig?.Element("dal-packages")?.Elements() // childtren of element 'dat-packages'
            ?? throw new DO.DalConfigException("<dal-packages> element is missing");
        s_dalPackages = packages.ToDictionary(p => "" + p.Name, p => p.Value);
    }
}



