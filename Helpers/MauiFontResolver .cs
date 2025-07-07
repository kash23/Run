using PdfSharpCore.Fonts;
using System.IO;
using System.Reflection;

public class MauiFontResolver : IFontResolver
{
    public string DefaultFontName => "OpenSans"; 
    public byte[] GetFont(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resource = "Run.Resources.Fonts.OpenSans-Regular.ttf"; 

        using Stream stream = assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException($"Font not found: {resource}");

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        return new FontResolverInfo("OpenSans");
    }
}
