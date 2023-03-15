using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.Models;
public class Language
{
    [NotNull]
    public string? Key { get; set; }

    [NotNull]
    public string? DisplayName { get; set; }
}
