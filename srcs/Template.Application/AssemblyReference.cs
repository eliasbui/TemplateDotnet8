using System.Reflection;

namespace Template.Application;

public static class AssemblyReference
{
    // This is a reference to the assembly that contains this class.
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
