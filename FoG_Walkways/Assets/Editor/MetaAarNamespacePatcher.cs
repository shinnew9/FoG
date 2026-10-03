using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Works around the AGP 9 (Unity 6000.4.4+) requirement that every Android library
// declare a unique namespace: several Meta XR SDK AARs ship with the same
// package "com.oculus.Integration", which fails the Gradle manifest merge.
// Rewrites the package attribute inside the duplicated AARs' AndroidManifest.xml.
// Runs automatically before every Android build; can also be run from the menu.
// Patches live in Library/PackageCache, so a package re-resolve reverts them —
// the build-time hook re-applies as needed.
public class MetaAarNamespacePatcher : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.Android)
            PatchDuplicateAarNamespaces();
    }

    [MenuItem("Build/Patch Meta AAR Namespaces")]
    public static void PatchDuplicateAarNamespaces()
    {
        string projectRoot = Path.GetDirectoryName(Application.dataPath);
        var searchRoots = new[]
        {
            Path.Combine(projectRoot, "Library", "PackageCache"),
            Application.dataPath,
        };

        var byPackage = new Dictionary<string, List<string>>();
        foreach (string root in searchRoots.Where(Directory.Exists))
        {
            foreach (string aar in Directory.EnumerateFiles(root, "*.aar", SearchOption.AllDirectories))
            {
                string pkg = ReadPackage(aar);
                if (pkg == null)
                    continue;
                if (!byPackage.TryGetValue(pkg, out var list))
                    byPackage[pkg] = list = new List<string>();
                list.Add(aar);
            }
        }

        int patched = 0;
        foreach (var group in byPackage.Where(kv => kv.Value.Count > 1))
        {
            // Keep the original namespace on OVRPlugin (or the first file) and rename the rest.
            var ordered = group.Value
                .OrderByDescending(p => Path.GetFileName(p).StartsWith("OVRPlugin", StringComparison.OrdinalIgnoreCase))
                .ThenBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (string aar in ordered.Skip(1))
            {
                string newPkg = group.Key + "." + Sanitize(Path.GetFileNameWithoutExtension(aar));
                RewritePackage(aar, newPkg);
                Debug.Log($"[MetaAarNamespacePatcher] Patched {Path.GetFileName(aar)} -> {newPkg}");
                patched++;
            }
        }

        Debug.Log(patched == 0
            ? "[MetaAarNamespacePatcher] No duplicate AAR namespaces found."
            : $"[MetaAarNamespacePatcher] Patched {patched} AAR(s).");
    }

    static string ReadPackage(string aarPath)
    {
        using var zip = ZipFile.OpenRead(aarPath);
        var entry = zip.GetEntry("AndroidManifest.xml");
        if (entry == null)
            return null;
        using var reader = new StreamReader(entry.Open());
        var match = Regex.Match(reader.ReadToEnd(), "package=\"([^\"]+)\"");
        return match.Success ? match.Groups[1].Value : null;
    }

    // Fully extract and repack instead of editing the zip in place:
    // in-place updates can produce entries with EXT descriptors that
    // Java tooling (jetifier) rejects with "only DEFLATED entries can
    // have EXT descriptor".
    static void RewritePackage(string aarPath, string newPackage)
    {
        string tmp = Path.Combine(Path.GetTempPath(), "aarpatch_" + Guid.NewGuid().ToString("N"));
        try
        {
            ZipFile.ExtractToDirectory(aarPath, tmp);
            string manifestPath = Path.Combine(tmp, "AndroidManifest.xml");
            string xml = File.ReadAllText(manifestPath);
            xml = new Regex("package=\"[^\"]+\"").Replace(xml, $"package=\"{newPackage}\"", 1);
            File.WriteAllText(manifestPath, xml, new UTF8Encoding(false));
            File.Delete(aarPath);
            ZipFile.CreateFromDirectory(tmp, aarPath);
        }
        finally
        {
            if (Directory.Exists(tmp))
                Directory.Delete(tmp, true);
        }
    }

    static string Sanitize(string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s.ToLowerInvariant())
            if (char.IsLetterOrDigit(c) || c == '_')
                sb.Append(c);
        if (sb.Length == 0 || char.IsDigit(sb[0]))
            sb.Insert(0, 'a');
        return sb.ToString();
    }
}
